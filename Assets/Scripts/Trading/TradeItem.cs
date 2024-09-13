using System;
using UnityEngine;

namespace Furniture
{
    [Serializable]
    public class TradeItem
    {
        [field:SerializeField] public FurniturePiece Piece { get; set; }
        [field:SerializeField] public string Name { get; set; }
        [field:SerializeField] public int Cost { get; set; }
    }
}