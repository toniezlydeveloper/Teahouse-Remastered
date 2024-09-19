using System.Collections.Generic;
using System.Linq;
using Furniture;
using Internal.Dependencies.Core;
using Newtonsoft.Json;
using Trading;
using UnityEngine;

namespace Saving.Proxies
{
    public class InventorySaveData
    {
        [JsonProperty("pI")] public int PieceIndex { get; set; }
        [JsonProperty("c")] public int Count { get; set; }
    }
    
    public class InventorySaveProxy : ASaveProxy
    {
        [SerializeField] private TradeItemsConfig itemsForSale;
        
        public override void Read(string json)
        {
            List<InventorySaveData> data = JsonConvert.DeserializeObject<List<InventorySaveData>>(json);
            DependencyInjector.GetRecipe<DependencyList<IFurniturePiece>>().Value.Clear();
            DependencyInjector.GetRecipe<DependencyList<IFurniturePiece>>().Value.AddRange(data.Select(saveData => saveData.PieceIndex != -1 ?
                new FurniturePiece(itemsForSale.Set[saveData.PieceIndex].Piece)
                {
                    Count = saveData.Count
                }
                : null)
            );
        }

        public override string Write()
        {
            List<InventorySaveData> data = DependencyInjector.GetRecipe<DependencyList<IFurniturePiece>>().Value.Select(piece => new InventorySaveData
            {
                PieceIndex = piece != null ? itemsForSale.Set.FindIndex(item => item.Piece.Prefab == piece.Prefab) : -1,
                Count = piece?.Count ?? 0
            }).ToList();
            
            return JsonConvert.SerializeObject(data);
        }
    }
}