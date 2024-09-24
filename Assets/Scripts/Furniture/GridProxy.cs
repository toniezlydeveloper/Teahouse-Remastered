using Grids;
using Internal.Dependencies.Core;
using UnityEngine;

namespace Furniture
{
    public class GridProxy : ADependency<IGridPointer>, IGridPointer
    {
        [SerializeField] private Transform pointer;

        public Vector3 Position => pointer.position;
    }
}