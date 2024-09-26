using System.Collections.Generic;
using UnityEngine;

namespace Furniture
{
    [CreateAssetMenu(menuName = "Config/Purchasable Items")]
    public class PurchasableItemsConfig : ScriptableObject
    {
        [field: SerializeField] public List<PurchasableItemsCategoryConfig> Categories { get; set; }
    }
}