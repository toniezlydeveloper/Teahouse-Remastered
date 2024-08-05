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
        public GridItem Item { get; }
    }
    
    public interface IGridPointer : IDependency
    {
        public Vector3 PointerPosition { get; }
        public Vector3 PlayerPosition { get; }
    }

    public interface IGrid : IDependency
    {
        bool TryGetInsideCells(out List<GridCell> cells, out Vector3 center);
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
        private Vector3 _adjustedPointer;

        private int _adjustedPointerColumn;
        private int _adjustedPointerRow;
        private int _pointerColumn;
        private int _pointerRow;
        private int _playerColumn;
        private int _playerRow;

        private List<GridCell> _cells;
        private bool _areCellsValid;

        private bool _isPlayerSameColumn;
        private bool _isPlayerSameRow;
        private bool _isPlayerForward;
        private bool _isPlayerRight;
        
        private const int SafetyRowColumnCount = 10;

        private void Start() => GetValues();

        public bool TryGetInsideCells(out List<GridCell> cells, out Vector3 cellsCenter)
        {
            GetGridPositions();
            AdjustPointer(GetPointerAdjustment());
            GetAdjustedPointerPosition();
            GetPointedCells();
            return ReadOutput(out cells, out cellsCenter);
        }
        private void GetGridPositions()
        {
            GetColumn(_pointer.Value.PointerPosition, out _pointerColumn);
            GetRow(_pointer.Value.PointerPosition, out _pointerRow);
            GetColumn(_pointer.Value.PlayerPosition, out _playerColumn);
            GetRow(_pointer.Value.PlayerPosition, out _playerRow);
        }

        private void GetAdjustedPointerPosition()
        {
            GetColumn(_adjustedPointer, out _adjustedPointerColumn);
            GetRow(_adjustedPointer, out _adjustedPointerRow);
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

        private void AdjustPointer(Vector3 pointerAdjustment) => _adjustedPointer = _pointer.Value.PointerPosition + pointerAdjustment;
        
        private Vector3 GetPointerAdjustment()
        {
            float worldHeight = cellSize * _itemHolder.Value.Item.Height;
            float worldWidth = cellSize * _itemHolder.Value.Item.Width;
            float columnFactor = 0;
            float rowFactor = 0;

            _isPlayerSameColumn = _playerColumn == _pointerColumn;
            _isPlayerRight = _playerColumn > _pointerColumn;
            _isPlayerSameRow = _playerRow == _pointerRow;
            _isPlayerForward = _playerRow > _pointerRow;

            switch (_itemHolder.Value.Orientation)
            {
                case GridItemOrientation.ColumnWise:
                    if (!_isPlayerSameColumn)
                        columnFactor += _isPlayerRight ? cellSize * 0.5f : -cellSize * 0.5f;

                    if (_isPlayerRight)
                        columnFactor += worldWidth * 0.5f;

                    if (_isPlayerSameColumn)
                        columnFactor += worldWidth * 0.25f;
                    
                    if (_isPlayerSameColumn)
                        rowFactor = _isPlayerForward ? worldHeight - cellSize * 0.5f : -cellSize * 0.5f;
                    else
                        rowFactor = worldHeight * 0.5f - cellSize * 0.5f;
                    
                    break;
                
                case GridItemOrientation.RowWise:
                    if (!_isPlayerSameRow)
                        rowFactor += _isPlayerForward ? cellSize * 0.5f : -cellSize * 0.5f;
                    
                    if (_isPlayerForward)
                        rowFactor += worldWidth * 0.5f;

                    if (_isPlayerSameRow)
                        rowFactor += worldWidth * 0.25f;

                    if (_isPlayerSameRow)
                        columnFactor = _isPlayerRight ? worldHeight - cellSize * 0.5f : -cellSize * 0.5f;
                    else
                        columnFactor = worldHeight * 0.5f - cellSize * 0.5f;
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return -rowFactor * Vector3.forward + -columnFactor * Vector3.right;
        }

        private void GetPointedCells()
        {
            _cells = new List<GridCell>();
            
            switch (_itemHolder.Value.Orientation)
            {
                case GridItemOrientation.ColumnWise:
                    for (int i = 0; i < _itemHolder.Value.Item.Height; i++)
                    for (int j = 0; j < _itemHolder.Value.Item.Width; j++)
                        _cells.Add(new GridCell
                        {
                            Column = _adjustedPointerColumn + j,
                            Row = _adjustedPointerRow + i
                        });
                    break;
                case GridItemOrientation.RowWise:
                    for (int i = 0; i < _itemHolder.Value.Item.Height; i++)
                    for (int j = 0; j < _itemHolder.Value.Item.Width; j++)
                        _cells.Add(new GridCell
                        {
                            Column = _adjustedPointerColumn + i,
                            Row = _adjustedPointerRow + j
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
        
        #region Drawing
        
        private void OnDrawGizmos()
        {
            GetMock();
            
            GetValues();
            
            GetGridPositions();
            AdjustPointer(GetPointerAdjustment());
            GetAdjustedPointerPosition();
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_pointer.Value.PlayerPosition + Vector3.up * center.y * 2f, 0.125f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_pointer.Value.PointerPosition + Vector3.up * center.y * 2f, 0.25f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_adjustedPointer, 0.125f);
        }

        private void DrawPointerCells()
        {
            Func<GridCell, Vector3> calculateWorldPosition = cell =>
            {
                Vector3 cellPosition = new Vector3(cellSize * cell.Column, 0f, cellSize * cell.Row);
                return _cellWorldPositionDelta + cellPosition;
            };
            
            Gizmos.color = Color.magenta;
            
            GridCell pointerCell = new GridCell { Column = _adjustedPointerColumn, Row = _adjustedPointerRow };
            Gizmos.DrawWireCube(calculateWorldPosition.Invoke(pointerCell), new Vector3(cellSize, 0.25f, cellSize));
            
            Gizmos.color = _areCellsValid ? Color.blue : Color.black;

            foreach (GridCell gridCell in _cells)
                Gizmos.DrawWireCube(calculateWorldPosition.Invoke(gridCell), new Vector3(cellSize, 0.125f, cellSize));
        }
        
        #endregion
    }
}