using System.Collections.Generic;
using System.Linq;
using Furniture;
using Grids;
using Newtonsoft.Json;
using UnityEngine;
using Utilities;

namespace Saving.Proxies
{
    public class PlacedFurnitureSaveData
    {
        [JsonProperty("category")] public FurnitureCategory Category { get; set; }
        [JsonProperty("cells")] public List<GridCell> Cells { get; set; }
        [JsonProperty("pieceIndex")] public int PieceIndex { get; set; }
        [JsonProperty("modelId")] public string ModelId { get; set; }
    }
    
    public class FurnitureHandlerSaveProxy : ASaveProxy
    {
        [SerializeField] private PurchasableItemsConfig itemsForSale;

        private const string PlacedFurnitureVariableName = "_placedFurniture";
        private const string AddRangeMethodName = "AddRange";
        private const string ClearMethodName = "Clear";
        
        public override void Read(string json)
        {
            ClearMethodName.CallMethod(GetPlacedFurniture());
            AddRangeMethodName.CallMethod(GetPlacedFurniture(), GetPlacedFurniture(json));
        }

        public override string Write()
        {
            List<PlacedFurnitureSaveData> data = GetPlacedFurniture().Select(furniture => new PlacedFurnitureSaveData
            {
                PieceIndex = itemsForSale.Categories.First(item => item.Category == furniture.Piece.Category).Set.FindIndex(item => item.Prefab == furniture.Piece.Prefab),
                ModelId = furniture.Model.GetComponent<FurniturePieceSaveProxy>().Id,
                Category = furniture.Piece.Category,
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
                Piece = itemsForSale.Categories.First(config => config.Category == furnitureData.Category).Set.ElementAt(furnitureData.PieceIndex),
                Model = proxies.First(proxy => proxy.Id == furnitureData.ModelId).GameObject,
                Cells = furnitureData.Cells
            }).ToList();
        }
        
        private List<PlacedFurniture> GetPlacedFurniture() => PlacedFurnitureVariableName.GetValue<FurnitureHandler, List<PlacedFurniture>>(FindObjectOfType<FurnitureHandler>());
    }
}