using System.Collections.Generic;
using Furniture;
using Internal.Dependencies.Core;
using UnityEngine;

namespace Inventory
{
    public interface IInventory : IDependency
    {
        List<FurniturePiece> Pieces { get; }
    }

    public class Inventory : MonoBehaviour, IInventory
    {
        public List<FurniturePiece> Pieces { get; } = new();

        private void Start()
        {
            Pieces.Add(null);
            Pieces.Add(null);
            Pieces.Add(null);
            Pieces.Add(null);
            Pieces.Add(null);
        }
    }
}