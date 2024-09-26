using System.Collections.Generic;
using UnityEngine;

namespace Furniture
{
    [CreateAssetMenu(menuName = "Config/Purchasable Items Category")]
    public class PurchasableItemsCategoryConfig : ScriptableObject
    {
        [field: SerializeField] public FurnitureCategory Category { get; set; }
        [field: SerializeField] public List<FurniturePiece> Set { get; set; }
    }
}