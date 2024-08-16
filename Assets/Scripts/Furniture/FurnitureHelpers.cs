using System.Collections.Generic;
using Grids;
using UnityEngine;

namespace Furniture
{
    public static class FurnitureHelpers
    {
        private static readonly Dictionary<FurnitureOrientation, GridItemOrientation> GridByFurniture = new()
        {
            { FurnitureOrientation.AgainstColumn, GridItemOrientation.ColumnWise },
            { FurnitureOrientation.AlongColumn, GridItemOrientation.ColumnWise },
            { FurnitureOrientation.AgainstRow, GridItemOrientation.RowWise },
            { FurnitureOrientation.AlongRow, GridItemOrientation.RowWise }
        };
        private static readonly Dictionary<FurnitureOrientation, Vector3> ForwardByFurniture = new()
        {
            { FurnitureOrientation.AgainstColumn, Vector3.back },
            { FurnitureOrientation.AlongColumn, Vector3.forward },
            { FurnitureOrientation.AgainstRow, Vector3.left },
            { FurnitureOrientation.AlongRow, Vector3.right },
        };
        private static readonly List<FurnitureOrientation> AllOrientations = new()
        {
            FurnitureOrientation.AlongColumn,
            FurnitureOrientation.AlongRow,
            FurnitureOrientation.AgainstColumn,
            FurnitureOrientation.AgainstRow
        };
        
        public static GridItemOrientation ReadGridOrientation(this int orientationIndex) => GridByFurniture[AllOrientations[orientationIndex]];
        
        public static FurnitureOrientation ReadFurnitureOrientation(this int orientationIndex) => AllOrientations[orientationIndex];
        
        public static int Clamp(this int orientationIndex) => (orientationIndex + AllOrientations.Count) % AllOrientations.Count;

        public static Vector3 ToForward(this FurnitureOrientation orientation) => ForwardByFurniture[orientation];
    }
}