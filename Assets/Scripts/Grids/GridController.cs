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

        private int _pointerColumn;
        private int _pointerRow;

        private List<GridCell> _cells;
        private bool _areCellsValid;

        private bool _isPlayerSameColumn;
        private bool _isPlayerSameRow;
        private bool _isPlayerForward;
        private bool _isPlayerRight;

        private void Start() => GetValues();

        public bool TryGetInsideCells(out List<GridCell> cells, out Vector3 cellsCenter)
        {
            TryChangingGridPositions();
            AdjustPointer(GetPointerAdjustment());
            GetPointedCells();
            return ReadOutput(out cells, out cellsCenter);
        }
        
        private void GetValues()
        {
            Vector3 halvedSize = new Vector3(columnCount, 0f, rowCount) * cellSize * 0.5f;
            Vector3 halfCellDelta = new Vector3(cellSize, center.y, cellSize) * 0.5f;
            _rightUpperCorner = center + halvedSize;
            _leftLowerCorner = center - halvedSize;
            _cellWorldPositionDelta = _leftLowerCorner + halfCellDelta + center;
        }

        private void TryChangingGridPositions()
        {
            int pointerColumn = (int)((_pointer.Value.PointerPosition.x - _leftLowerCorner.x) / cellSize);
            int pointerRow = (int)((_pointer.Value.PointerPosition.z - _leftLowerCorner.z) / cellSize);
            int playerColumn = (int)((_pointer.Value.PlayerPosition.x - _leftLowerCorner.x) / cellSize);
            int playerRow = (int)((_pointer.Value.PlayerPosition.z - _leftLowerCorner.z) / cellSize);

            _isPlayerRight = playerColumn > pointerColumn;
            _isPlayerForward = playerRow > pointerRow;
            _isPlayerSameColumn = playerColumn == pointerColumn;
            _isPlayerSameRow = playerRow == pointerRow;
        }

        private void AdjustPointer(Vector3 pointerAdjustment)
        {
            Vector3 adjustedPointer = _pointer.Value.PointerPosition + pointerAdjustment;
            _pointerColumn = (int)((adjustedPointer.x - _leftLowerCorner.x) / cellSize);
            _pointerRow = (int)((adjustedPointer.z - _leftLowerCorner.z) / cellSize);
        }

        private Vector3 GetPointerAdjustment()
        {
            float worldHeight = cellSize * (_itemHolder.Value.Item.Height - 1);
            float worldWidth = cellSize * (_itemHolder.Value.Item.Width - 1);
            float columnFactor = 0;
            float rowFactor = 0;

            switch (_itemHolder.Value.Orientation)
            {
                case GridItemOrientation.ColumnWise:
                    if (!_isPlayerSameColumn)
                        columnFactor += _isPlayerRight ? cellSize * 0.5f : -cellSize * 0.5f;

                    if (_isPlayerRight)
                        columnFactor += worldWidth;

                    if (_isPlayerSameColumn)
                        columnFactor += worldWidth * 0.5f;
                    
                    if (_isPlayerSameColumn)
                        rowFactor = _isPlayerForward ? worldHeight + cellSize * 0.5f : -cellSize * 0.5f;
                    else
                        rowFactor = worldHeight * 0.5f;
                    
                    break;
                
                case GridItemOrientation.RowWise:
                    if (!_isPlayerSameRow)
                        rowFactor += _isPlayerForward ? cellSize * 0.5f : -cellSize * 1.5f;
                    
                    if (_isPlayerForward)
                        rowFactor += worldWidth;

                    if (_isPlayerSameRow)
                        rowFactor += worldWidth * 0.5f;

                    if (_isPlayerSameRow)
                        columnFactor = _isPlayerRight ? worldHeight + cellSize * 1.5f : -cellSize * 0.5f;
                    else
                        columnFactor = worldHeight * 0.5f;
                    
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
                            Column = _pointerColumn + j,
                            Row = _pointerRow + i
                        });
                    break;
                case GridItemOrientation.RowWise:
                    for (int i = 0; i < _itemHolder.Value.Item.Height; i++)
                    for (int j = 0; j < _itemHolder.Value.Item.Width; j++)
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
        
        #region Drawing
        
        private void OnDrawGizmos()
        {
            GetMock();
            
            GetValues();
            
            TryChangingGridPositions();
            AdjustPointer(GetPointerAdjustment());
            GetPointedCells();
            
            DrawGrid();
            DrawPointers();
            DrawPointerCell();
            DrawPointedCells();
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
            for (int i = 0; i < columnCount + 1; i++)
            {
                Vector3 cellDelta = new Vector3(cellSize, 0f, 0f) * i + center;
                Gizmos.DrawLine(_leftLowerCorner + cellDelta, new Vector3(_leftLowerCorner.x, center.y, _rightUpperCorner.z) + cellDelta);
            }

            for (int i = 0; i < rowCount + 1; i++)
            {
                Vector3 cellDelta = new Vector3(0f, 0f, cellSize) * i - center;
                Gizmos.DrawLine(_rightUpperCorner - cellDelta, new Vector3(_leftLowerCorner.x, center.y, _rightUpperCorner.z) - cellDelta);
            }
        }

        private void DrawPointers()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_pointer.Value.PlayerPosition + Vector3.up * center.y * 2f, 0.125f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_pointer.Value.PointerPosition + Vector3.up * center.y * 2f, 0.25f);
        }

        private void DrawPointerCell()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(GetWorldPosition(_pointerColumn, _pointerRow), new Vector3(cellSize, 0.25f, cellSize));
        }

        private void DrawPointedCells()
        {
            Gizmos.color = _areCellsValid ? Color.blue : Color.black;

            foreach (GridCell gridCell in _cells)
                Gizmos.DrawWireCube(GetWorldPosition(gridCell.Column, gridCell.Row), new Vector3(cellSize, 0.125f, cellSize));
        }

        private Vector3 GetWorldPosition(int column, int row)
        {
            Vector3 cellPosition = new Vector3(cellSize * column, 0f, cellSize * row);
            return _cellWorldPositionDelta + cellPosition;
        }
        
        #endregion
    }
}