using System.Collections.Generic;
using UnityEngine;

namespace Trading
{
    [CreateAssetMenu(menuName = "Setup/Trade Items")]
    public class TradeItemsConfig : ScriptableObject
    {
        [field:SerializeField] public List<TradeItem> Set { get; set; }
    }
}