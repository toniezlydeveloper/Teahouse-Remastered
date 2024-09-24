using System;
using System.Collections.Generic;
using System.Linq;
using Internal.Dependencies.Core;
using UnityEngine;

namespace Grids
{
    public interface IGridItemHolder : IDependency
    {
        public GridItemOrientation Orientation { get; }
        public GridDimensions Dimensions { get; }
    }
    
    public interface IGridPointer : IDependency
    {
        public Vector3 Position { get; }
    }

    public interface IGrid : IDependency
    {
        bool TryGetInsideCells(out List<GridCell> cells, out Vector3 center);
        void GetPointerCell(out GridCell cell);
    }
    
    // Along column: positive X (Vector3.right)
    // Along row: positive Z (Vector3.forward)
    public class GridController : ADependency<IGrid>, IGrid
    {
        [SerializeField] private int columnCount;
        [SerializeField] private int rowCount;
        [SerializeField] private float cellSize;
        [SerializeField] private Vector3 center;

        private DependencyRecipe<IGridItemHolder> _itemHolder = DependencyInjector.GetRecipe<IGridItemHolder>();
        private DependencyRecipe<IGridPointer> _pointer = DependencyInjector.GetRecipe<IGridPointer>();
        
        private Vector3 _cellWorldPositionDelta;
        private Vector3 _rightUpperCorner;
        private Vector3 _leftLowerCorner;

        private int _pointerColumn;
        private int _pointerRow;

        private List<GridCell> _cells;
        private bool _areCellsValid;
        
        private const int SafetyRowColumnCount = 10;

        private void Start() => GetValues();

        public bool TryGetInsideCells(out List<GridCell> cells, out Vector3 cellsCenter)
        {
            GetGridPositions();
            GetPointedCells();
            return ReadOutput(out cells, out cellsCenter);
        }

        public void GetPointerCell(out GridCell cell)
        {
            GetGridPositions();
            ReadOutput(out cell);
        }

        private void GetGridPositions()
        {
            GetColumn(_pointer.Value.Position, out _pointerColumn);
            GetRow(_pointer.Value.Position, out _pointerRow);
        }

        private void GetValues()
        {
            Vector3 halvedSize = new Vector3(columnCount, 0f, rowCount) * cellSize * 0.5f;
            Vector3 halfCellDelta = new Vector3(cellSize, center.y, cellSize) * 0.5f;
            _rightUpperCorner = center + halvedSize;
            _leftLowerCorner = center - halvedSize;
            _cellWorldPositionDelta = _leftLowerCorner + halfCellDelta;
        }

        private void GetColumn(Vector3 position, out int column)
        {
            float safetyFactor = cellSize * SafetyRowColumnCount;
            column = (int)((safetyFactor + position.x - _leftLowerCorner.x) / cellSize);
            column -= SafetyRowColumnCount;
        }

        private void GetRow(Vector3 position, out int row)
        {
            float safetyFactor = cellSize * SafetyRowColumnCount;
            row = (int)((safetyFactor + position.z - _leftLowerCorner.z) / cellSize);
            row -= SafetyRowColumnCount;
        }

        private void GetPointedCells()
        {
            _cells = new List<GridCell>();
            
            switch (_itemHolder.Value.Orientation)
            {
                case GridItemOrientation.ColumnWise:
                    for (int i = 0; i < _itemHolder.Value.Dimensions.Height; i++)
                    for (int j = 0; j < _itemHolder.Value.Dimensions.Width; j++)
                        _cells.Add(new GridCell
                        {
                            Column = _pointerColumn + j,
                            Row = _pointerRow + i
                        });
                    break;
                case GridItemOrientation.RowWise:
                    for (int i = 0; i < _itemHolder.Value.Dimensions.Height; i++)
                    for (int j = 0; j < _itemHolder.Value.Dimensions.Width; j++)
                        _cells.Add(new GridCell
                        {
                            Column = _pointerColumn + i,
                            Row = _pointerRow + j
                        });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool ReadOutput(out List<GridCell> cells, out Vector3 cellsCenter)
        {
            cellsCenter = _cells.Aggregate(Vector3.zero, (current, cell) =>
            {
                Vector3 cellPosition = new Vector3(cellSize * cell.Column, 0f, cellSize * cell.Row);
                return _cellWorldPositionDelta + cellPosition + current;
            }) / _cells.Count;
            cells = _cells;
            
            return _cells.All(cell =>
            {
                if (cell.Column < 0 || cell.Column > columnCount - 1)
                    return false;

                return cell.Row >= 0 && cell.Row <= rowCount - 1;
            });
        }

        private void ReadOutput(out GridCell cell) => cell = new GridCell { Column = _pointerColumn, Row = _pointerRow };
        
        #region Drawing
        
        private void OnDrawGizmos()
        {
            GetMock();
            
            GetValues();
            
            GetGridPositions();
            GetPointedCells();
            
            DrawGrid();
            DrawPointers();
            DrawPointerCells();
        }

        private void GetMock()
        {
            if (Application.isPlaying)
                return;
            
            _itemHolder = new DependencyRecipe<IGridItemHolder> { Value = GetComponent<IGridItemHolder>() };
            _pointer = new DependencyRecipe<IGridPointer> { Value = GetComponent<IGridPointer>() };
        }
        
        private void DrawGrid()
        {
            Vector3 upperPoint = new Vector3(_leftLowerCorner.x, center.y, _rightUpperCorner.z);
            
            for (int i = 0; i < columnCount + 1; i++)
            {
                Vector3 cellDelta = new Vector3(cellSize, 0f, 0f) * i;
                Gizmos.DrawLine(_leftLowerCorner + cellDelta, upperPoint + cellDelta);
            }

            for (int i = 0; i < rowCount + 1; i++)
            {
                Vector3 cellDelta = new Vector3(0f, 0f, cellSize) * i;
                Gizmos.DrawLine(_rightUpperCorner - cellDelta, upperPoint - cellDelta);
            }

            Gizmos.DrawSphere(_rightUpperCorner, 0.125f);
            Gizmos.DrawSphere(_leftLowerCorner, 0.125f);
        }

        private void DrawPointers()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_pointer.Value.Position + Vector3.up * center.y * 2f, 0.25f);
        }

        private void DrawPointerCells()
        {
            Func<GridCell, Vector3> calculateWorldPosition = cell =>
            {
                Vector3 cellPosition = new Vector3(cellSize * cell.Column, 0f, cellSize * cell.Row);
                return _cellWorldPositionDelta + cellPosition;
            };
            
            Gizmos.color = Color.magenta;
            
            GridCell pointerCell = new GridCell { Column = _pointerColumn, Row = _pointerRow };
            Gizmos.DrawWireCube(calculateWorldPosition.Invoke(pointerCell), new Vector3(cellSize, 0.25f, cellSize));
            
            Gizmos.color = _areCellsValid ? Color.blue : Color.black;

            foreach (GridCell gridCell in _cells)
                Gizmos.DrawWireCube(calculateWorldPosition.Invoke(gridCell), new Vector3(cellSize, 0.125f, cellSize));
        }
        
        #endregion
    }
}