using System;
using Grids;
using Internal.Dependencies.Core;
using UnityEngine;

namespace Furniture
{
    public interface IFurniturePiece : IDependency
    {
        public GridDimensions Dimensions { get; }
        public GameObject Prefab { get; }
        public Vector3 Offset { get; }
        public Sprite Icon { get; }
        public int Cost { get; }
    }

    public enum FurnitureCategory
    {
        Bed,
        Chair,
        Wardrobe,
        Desk
    }
    
    [Serializable]
    public class FurniturePiece : IFurniturePiece
    {
        [field: SerializeField] public FurnitureCategory Category { get; set; }
        [field: SerializeField] public GridDimensions Dimensions { get; set; }
        [field: SerializeField] public GameObject Prefab { get; set; }
        [field: SerializeField] public Vector3 Offset { get; set; }
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public int Cost { get; set; }
        
        public FurniturePiece(IFurniturePiece piece)
        {
            Dimensions = piece.Dimensions;
            Prefab = piece.Prefab;
            Offset = piece.Offset;
            Cost = piece.Cost;
            Icon = piece.Icon;
        }
    }
}