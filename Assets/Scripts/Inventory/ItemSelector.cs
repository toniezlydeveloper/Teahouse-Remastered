using System;
using System.Collections.Generic;
using Grids;
using Internal.Dependencies.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory
{
    public enum FurnitureOrientation
    {
        AgainstColumn,
        AlongColumn,
        AgainstRow,
        AlongRow
    }

    [Serializable]
    public class FurniturePiece
    {
        [field:SerializeField] public GameObject Prefab { get; set; }
        [field:SerializeField] public GridItem Item { get; set; }
    }
    
    public class ItemSelector : ADependency<IGridItemHolder>, IGridItemHolder
    {
        [SerializeField] private InputActionReference previousInput;
        [SerializeField] private InputActionReference nextInput;
        [SerializeField] private InputActionReference placeInput;
        [SerializeField] private FurniturePiece piece;

        private DependencyRecipe<IGrid> _grid = DependencyInjector.GetRecipe<IGrid>();
        private int _orientationIndex;
        private GameObject _preview;

        public GridItemOrientation Orientation => AllOrientations[_orientationIndex] switch
        {
            FurnitureOrientation.AgainstColumn => GridItemOrientation.ColumnWise,
            FurnitureOrientation.AlongColumn => GridItemOrientation.ColumnWise,
            FurnitureOrientation.AgainstRow => GridItemOrientation.RowWise,
            FurnitureOrientation.AlongRow => GridItemOrientation.RowWise,
            _ => throw new ArgumentOutOfRangeException()
        };
        public GridItem Item => piece.Item;

        private static readonly List<FurnitureOrientation> AllOrientations = new()
        {
            FurnitureOrientation.AlongColumn,
            FurnitureOrientation.AlongRow,
            FurnitureOrientation.AgainstColumn,
            FurnitureOrientation.AgainstRow
        };

        private void Update()
        {
            HandleOrientation();

            if (_preview == null)
            {
                _preview = Instantiate(piece.Prefab);
                foreach (Collider col in _preview.GetComponentsInChildren<Collider>())
                {
                    Destroy(col);
                }
            }

            _grid.Value.TryGetInsideCells(out List<GridCell> cells, out Vector3 center);
            _preview.transform.position = center;
            _preview.transform.rotation = AllOrientations[_orientationIndex] switch
            {
                FurnitureOrientation.AgainstColumn => Quaternion.LookRotation(Vector3.back),
                FurnitureOrientation.AlongColumn => Quaternion.LookRotation(Vector3.forward),
                FurnitureOrientation.AgainstRow => Quaternion.LookRotation(Vector3.left),
                FurnitureOrientation.AlongRow => Quaternion.LookRotation(Vector3.right),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (placeInput.action.triggered)
                _preview = null;
        }

        private void HandleOrientation()
        {
            if (previousInput.action.triggered)
                _orientationIndex--;

            if (nextInput.action.triggered)
                _orientationIndex++;

            _orientationIndex = (_orientationIndex + AllOrientations.Count) % AllOrientations.Count;
        }
    }
}