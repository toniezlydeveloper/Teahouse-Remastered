using System;
using Grids;
using UnityEngine;

namespace Furniture
{
    [Serializable]
    public class FurniturePiece
    {
        [field:SerializeField] public GridDimensions Dimensions { get; set; }
        [field:SerializeField] public GameObject Prefab { get; set; }
        [field:SerializeField] public Sprite Icon { get; set; }
        [field:SerializeField] public int Count { get; set; }
        
        public FurniturePiece(FurniturePiece piece)
        {
            Dimensions = piece.Dimensions;
            Prefab = piece.Prefab;
            Icon = piece.Icon;
            Count = piece.Count;
        }
    }
}