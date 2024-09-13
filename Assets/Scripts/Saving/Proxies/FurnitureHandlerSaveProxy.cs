using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Furniture;
using Grids;
using Newtonsoft.Json;
using UnityEngine;

namespace Saving.Items
{
    public class PlacedFurnitureSaveData
    {
        [JsonProperty("c")] public List<GridCell> Cells { get; set; }
        [JsonProperty("mI")] public string ModelId { get; set; }
        [JsonProperty("pI")] public int PieceIndex { get; set; }
    }
    
    public class FurnitureHandlerSaveProxy : ASaveProxy
    {
        [SerializeField] private TradeItemSet itemsForSale;
        
        public override void Read(string json)
        {
            Clear(GetList<PlacedFurniture>());
            AddRange(GetList<PlacedFurniture>(), GetPlacedFurniture(json));
        }

        public override string Write()
        {
            List<PlacedFurnitureSaveData> data = GetList<PlacedFurniture>().Select(furniture => new PlacedFurnitureSaveData
            {
                PieceIndex = itemsForSale.Set.FindIndex(item => item.Piece.Prefab == furniture.Piece.Prefab),
                ModelId = furniture.Model.GetComponent<FurniturePieceSaveProxy>().Id,
                Cells = furniture.Cells
            }).ToList();
            
            return JsonConvert.SerializeObject(data);
        }

        private List<PlacedFurniture> GetPlacedFurniture(string json)
        {
            List<PlacedFurnitureSaveData> data = JsonConvert.DeserializeObject<List<PlacedFurnitureSaveData>>(json);
            FurniturePieceSaveProxy[] proxies = FindObjectsOfType<FurniturePieceSaveProxy>();
            
            return data.Select(furnitureData => new PlacedFurniture
            {
                Model = proxies.First(d => d.Id == furnitureData.ModelId).GameObject,
                Piece = itemsForSale.Set[furnitureData.PieceIndex].Piece,
                Cells = furnitureData.Cells
            }).ToList();
        }

        private static List<TItem> GetList<TItem>()
        {
            FurnitureHandler furnitureHandler = FindObjectOfType<FurnitureHandler>();
            FieldInfo fieldInfo = typeof(FurnitureHandler).GetField("_placedFurniture", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return fieldInfo!.GetValue(furnitureHandler) as List<TItem>;
        }

        private static void Clear<TItem>(IEnumerable<TItem> list)
        {
            MethodInfo clearMethod = typeof(List<TItem>).GetMethod("Clear", BindingFlags.Public | BindingFlags.Instance);
            clearMethod!.Invoke(list, null);
        }

        private static void AddRange<TItem>(object list, IEnumerable<TItem> items)
        {
            MethodInfo addRangeMethod = typeof(List<TItem>).GetMethod("AddRange", BindingFlags.Public | BindingFlags.Instance);
            addRangeMethod!.Invoke(list, new object[] { items });
        }
    }
}