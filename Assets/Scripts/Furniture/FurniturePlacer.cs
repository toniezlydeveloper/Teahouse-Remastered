using System.Collections.Generic;
using System.Linq;
using Grids;
using Internal.Dependencies.Core;
using UI.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Furniture
{
    public class FurniturePlacer : ADependency<IGridItemHolder>, IGridItemHolder
    {
        [SerializeField] private List<InputActionReference> selectionsInput;
        [SerializeField] private InputActionReference rotateRightInput;
        [SerializeField] private InputActionReference rotateLeftInput;
        [SerializeField] private InputActionReference placeInput;
        [SerializeField] private Material previewMaterial;
        [SerializeField] private Color invalidSpotColor;
        [SerializeField] private Color validSpotColor;
        [SerializeField] private GameObject uiParent;
        
        [SerializeField] private FurniturePiece test;

        private DependencyRecipe<IGrid> _grid = DependencyInjector.GetRecipe<IGrid>();
        private List<FurniturePiece> _furniturePieces;
        private IFurnishingPanel _furnishingPanel;
        private Material _originalPreviewMaterial;
        private FurniturePiece _selectedPiece;
        private List<GridCell> _takenCells;
        private Vector3 _previewForward;
        private bool _isAllowedToPlacePiece;
        private int _lastSelectedPieceIndex = -1;
        private int _selectedPieceIndex;
        private int _orientationIndex;
        private GameObject _preview;

        public GridItemOrientation Orientation { get; private set; }
        public GridItem Item { get; private set; }

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
        private static readonly int PreviewColorId = Shader.PropertyToID("_Base");

        private void Start()
        {
            GetReferences();
            RefreshPiecesUI();
        }

        private void Update()
        {
            HandleSelection();
            
            if (!HasPreview())
                return;
            
            HandleOrientation();
            ShowPreview(out List<GridCell> cells, out Vector3 center);
            MovePreview(center);

            if (!TryPlacingPiece(cells))
                return;
            
            RefreshPiecesUI();
        }

        private void HandleSelection()
        {
            if (!TrySelectingOtherPiece())
                return;

            RefreshSelectionUI();
            PreparePreview();
        }

        private bool HasPreview() => _preview != null;

        private bool TrySelectingOtherPiece()
        {
            if (_grid.Value == null)
                return false;
            
            foreach (InputActionReference input in selectionsInput.Where(input => input.action.triggered))
                _selectedPieceIndex = selectionsInput.IndexOf(input);

            _selectedPieceIndex = (_selectedPieceIndex + selectionsInput.Count) % selectionsInput.Count;

            if (_selectedPieceIndex == _lastSelectedPieceIndex)
                return false;

            _selectedPiece = _furniturePieces[_selectedPieceIndex];
            _lastSelectedPieceIndex = _selectedPieceIndex;
            return true;
        }

        private void PreparePreview()
        {
            if (_preview != null)
                Destroy(_preview);

            if (_selectedPiece?.Prefab == null)
                return;
            
            _preview = Instantiate(_selectedPiece.Prefab);
                
            foreach (Collider previewCollider in _preview.GetComponentsInChildren<Collider>())
                Destroy(previewCollider);

            foreach (Renderer previewRenderer in _preview.GetComponentsInChildren<Renderer>())
                previewRenderer.material = _originalPreviewMaterial;

            Item = _selectedPiece.Item;
            _orientationIndex = 0;
        }

        private void HandleOrientation()
        {
            if (rotateLeftInput.action.triggered)
                _orientationIndex--;

            if (rotateRightInput.action.triggered)
                _orientationIndex++;

            _orientationIndex = (_orientationIndex + AllOrientations.Count) % AllOrientations.Count;
            _previewForward = ForwardByFurniture[AllOrientations[_orientationIndex]];
            Orientation = GridByFurniture[AllOrientations[_orientationIndex]];
        }

        private void ShowPreview(out List<GridCell> cells, out Vector3 center)
        {
            _isAllowedToPlacePiece = _grid.Value.TryGetInsideCells(out cells, out center);

            foreach (GridCell cell in cells)
            {
                if (!_isAllowedToPlacePiece)
                    break;
                
                if (_takenCells.All(takenCell => takenCell.Column != cell.Column || takenCell.Row != cell.Row))
                    continue;

                _isAllowedToPlacePiece = false;
            }
            
            _originalPreviewMaterial.SetColor(PreviewColorId, _isAllowedToPlacePiece ? validSpotColor : invalidSpotColor);
        }

        private void MovePreview(Vector3 center)
        {
            _preview.transform.forward = _previewForward;
            _preview.transform.position = center;
        }

        private bool TryPlacingPiece(List<GridCell> cells)
        {
            if (!_isAllowedToPlacePiece)
                return false;
            
            if (!placeInput.action.triggered)
                return false;
            
            GameObject piece = Instantiate(_selectedPiece.Prefab);
            piece.transform.position = _preview.transform.position;
            piece.transform.rotation = _preview.transform.rotation;
            _takenCells.AddRange(cells);

            if (--_selectedPiece.Count != 0)
                return true;
            
            _furniturePieces[_furniturePieces.IndexOf(_selectedPiece)] = null;
            _selectedPiece = null;
            Destroy(_preview);
            return true;
        }

        private void RefreshPiecesUI() => _furnishingPanel.Present(_furniturePieces);
        
        private void RefreshSelectionUI() => _furnishingPanel.Present(_selectedPieceIndex);

        private void GetReferences()
        {
            _furnishingPanel = uiParent.GetComponentInChildren<IFurnishingPanel>();
            _furniturePieces = new List<FurniturePiece>(selectionsInput.Count);
            _originalPreviewMaterial = new Material(previewMaterial);
            _takenCells = new List<GridCell>();

            _furniturePieces.Add(test);
            
            for (int i = 0; i < selectionsInput.Count - 1; i++)
                _furniturePieces.Add(null);
        }
    }
}