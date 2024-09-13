using System.Collections.Generic;
using UnityEngine;

namespace Furniture
{
    [CreateAssetMenu(menuName = "Sets/Trade Items")]
    public class TradeItemSet : ScriptableObject
    {
        [field:SerializeField] public List<TradeItem> Set { get; set; }
    }
}