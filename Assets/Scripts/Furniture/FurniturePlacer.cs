using System.Collections.Generic;
using System.Linq;
using Grids;
using Interaction;
using Internal.Dependencies.Core;
using Player;
using UI.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Furniture
{
    public class FurniturePlacer : AInteractionHandler, IGridItemHolder
    {
        [SerializeField] private List<InputActionReference> selectionsInput;
        [SerializeField] private InputActionReference rotateRightInput;
        [SerializeField] private InputActionReference rotateLeftInput;
        [SerializeField] private InputActionReference placeInput;
        [SerializeField] private PlayerModeProxy playerMode;
        [SerializeField] private Material previewMaterial;
        [SerializeField] private Color invalidSpotColor;
        [SerializeField] private Color validSpotColor;

        private DependencyRecipe<DependencyList<IFurniturePiece>> _pieces = DependencyInjector.GetRecipe<DependencyList<IFurniturePiece>>();
        private IFurnishingPanel _furnishingPanel = DependencyInjector.Get<IFurnishingPanel>();
        private DependencyRecipe<IGrid> _grid = DependencyInjector.GetRecipe<IGrid>();
        private Material _originalPreviewMaterial;
        private IFurniturePiece _selectedPiece;
        private List<GridCell> _takenCells;
        private Vector3 _previewWorldPositionOffset;
        private Vector3 _previewForward;
        private bool _isAllowedToPlacePiece;
        private int _lastSelectedPieceIndex = -1;
        private int _selectedPieceIndex;
        private int _orientationIndex;
        private GameObject _preview;

        public override PlayerMode HandledModes => PlayerMode.Organization;
        
        public GridItemOrientation Orientation { get; private set; }
        public GridDimensions Dimensions { get; private set; }

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

        private void OnEnable() => playerMode.OnChanged += TogglePreview;
        
        private void OnDisable() => playerMode.OnChanged -= TogglePreview;

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
            if (playerMode.Value == PlayerMode.Modification)
                return false;
            
            if (_grid.Value == null)
                return false;
            
            foreach (InputActionReference input in selectionsInput.Where(input => input.action.triggered))
                _selectedPieceIndex = selectionsInput.IndexOf(input);

            _selectedPieceIndex = (_selectedPieceIndex + selectionsInput.Count) % selectionsInput.Count;

            if (_selectedPieceIndex == _lastSelectedPieceIndex)
                return false;

            _selectedPiece = _pieces.Value[_selectedPieceIndex];
            _lastSelectedPieceIndex = _selectedPieceIndex;
            return true;
        }

        private void PreparePreview()
        {
            if (_preview != null)
                Destroy(_preview);

            if (_selectedPiece?.Prefab == null)
                return;
            
            _previewWorldPositionOffset = _selectedPiece.Offset;
            _preview = Instantiate(_selectedPiece.Prefab);
                
            foreach (Collider previewCollider in _preview.GetComponentsInChildren<Collider>())
                Destroy(previewCollider);

            foreach (Renderer previewRenderer in _preview.GetComponentsInChildren<Renderer>())
                previewRenderer.material = _originalPreviewMaterial;

            Dimensions = _selectedPiece.Dimensions;
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
            center += _previewWorldPositionOffset;

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
            
            _pieces.Value[_pieces.Value.IndexOf(_selectedPiece)] = null;
            _selectedPiece = null;
            Destroy(_preview);
            return true;
        }

        private void TogglePreview(PlayerMode mode)
        {
            if (!_preview)
                return;
            
            _preview.SetActive(mode == PlayerMode.Organization);
        }

        private void RefreshPiecesUI() => _furnishingPanel.Present(_pieces.Value.Select(piece => new FurniturePieceData { Icon = piece?.Icon, Count = piece?.Count ?? 0}).ToList());
        
        private void RefreshSelectionUI() => _furnishingPanel.Present(_selectedPieceIndex);

        private void GetReferences()
        {
            _originalPreviewMaterial = new Material(previewMaterial);
            _takenCells = new List<GridCell>();
        }
    }
}