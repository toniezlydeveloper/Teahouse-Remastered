using System;
using Grids;
using Internal.Dependencies.Core;
using UnityEngine;

namespace Furniture
{
    public interface IFurniturePiece : IDependency
    {
        public GridDimensions Dimensions { get; set; }
        public GameObject Prefab { get; set; }
        public Vector3 Offset { get; set; }
        public Sprite Icon { get; set; }
        public int Count { get; set; }
    }
    
    [Serializable]
    public class FurniturePiece : IFurniturePiece
    {
        [field: SerializeField] public GridDimensions Dimensions { get; set; }
        [field: SerializeField] public GameObject Prefab { get; set; }
        [field: SerializeField] public Vector3 Offset { get; set; }
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public int Count { get; set; }
        
        public FurniturePiece(IFurniturePiece piece)
        {
            Dimensions = piece.Dimensions;
            Prefab = piece.Prefab;
            Offset = piece.Offset;
            Count = piece.Count;
            Icon = piece.Icon;
        }
    }
}