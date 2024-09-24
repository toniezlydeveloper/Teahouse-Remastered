using System.Collections.Generic;
using Grids;
using UnityEngine;

namespace Furniture
{
    public static class Helpers
    {
        private static readonly Dictionary<Orientation, GridItemOrientation> GridByFurniture = new()
        {
            { Orientation.AgainstColumn, GridItemOrientation.ColumnWise },
            { Orientation.AlongColumn, GridItemOrientation.ColumnWise },
            { Orientation.AgainstRow, GridItemOrientation.RowWise },
            { Orientation.AlongRow, GridItemOrientation.RowWise }
        };
        private static readonly Dictionary<Orientation, Vector3> ForwardByFurniture = new()
        {
            { Orientation.AgainstColumn, Vector3.back },
            { Orientation.AlongColumn, Vector3.forward },
            { Orientation.AgainstRow, Vector3.left },
            { Orientation.AlongRow, Vector3.right },
        };
        private static readonly List<Orientation> AllOrientations = new()
        {
            Orientation.AlongColumn,
            Orientation.AlongRow,
            Orientation.AgainstColumn,
            Orientation.AgainstRow
        };
        
        public static GridItemOrientation ReadGridOrientation(this int orientationIndex) => GridByFurniture[AllOrientations[orientationIndex]];
        
        public static Orientation ReadFurnitureOrientation(this int orientationIndex) => AllOrientations[orientationIndex];
        
        public static int Clamp(this int orientationIndex) => (orientationIndex + AllOrientations.Count) % AllOrientations.Count;

        public static Vector3 ToForward(this Orientation orientation) => ForwardByFurniture[orientation];
    }
}