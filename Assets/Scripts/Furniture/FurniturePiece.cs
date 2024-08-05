using System;
using Grids;
using UnityEngine;

namespace Furniture
{
    [Serializable]
    public class FurniturePiece
    {
        [field:SerializeField] public GameObject Prefab { get; set; }
        [field:SerializeField] public GridItem Item { get; set; }
        [field:SerializeField] public int Count { get; set; }
    }
}