using System;
using UnityEngine;

namespace Grids
{
    [Serializable]
    public class GridItem
    {
        [field:SerializeField] public int Height { get; set; }
        [field:SerializeField] public int Width { get; set; }
    }
}