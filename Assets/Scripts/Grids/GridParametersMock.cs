using UnityEngine;

namespace Grids
{
    public class GridParametersMock : MonoBehaviour, IGridItemHolder, IGridPointer
    {
        [field: SerializeField] public GridItemOrientation Orientation { get; set; }
        [field: SerializeField] public GridDimensions Dimensions { get; set; }
        [field: SerializeField] public Vector3 Position { get; set; }
        [field: SerializeField] public Vector3 PlayerPosition { get; set;  }
    }
}