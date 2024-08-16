using System.Collections.Generic;
using System.Linq;
using Grids;
using Internal.Dependencies.Core;
using Player;
using UI.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Furniture
{
    // This can be simplified by changing methods arrangement
    public class FurniturePlacer
    {
        private DependencyRecipe<DependencyList<IFurniturePiece>> _pieces = DependencyInjector.GetRecipe<DependencyList<IFurniturePiece>>();
        private IFurnishingPanel _furnishingPanel = DependencyInjector.Get<IFurnishingPanel>();
        private DependencyRecipe<IGrid> _grid = DependencyInjector.GetRecipe<IGrid>();
        private List<PlacedFurniture> _placedFurniture;
        private Vector3 _worldPositionOffset;
        private InputActionReference _placeInput;
        private List<GridCell> _cells;
        private Quaternion _rotation;
        private Vector3 _position;
        private Vector3 _previewForward;
        private int _orientationIndex;
        private Color _invalidSpotColor;
        private Color _validSpotColor;
        private Material _originalPreviewMaterial;
        private GameObject _preview;
        private IFurniturePiece _previewPiece;
        
        private static readonly int PreviewColorId = Shader.PropertyToID("_Base");

        public FurniturePlacer(List<PlacedFurniture> placedFurniture, InputActionReference placeInput, Material previewMaterial, Color invalidSpotColor, Color validSpotColor)
        {
            GetReferences(placedFurniture, previewMaterial, invalidSpotColor, validSpotColor, placeInput);
            RefreshPiecesUI();
        }

        public bool TryPlacing(IFurniturePiece selectedPiece, FurnitureOrientation orientation)
        {
            if (!TryGettingPlacementData(orientation))
                return false;
            
            if (!AreCellsFree())
                return false;
            
            if (!TryPlacingPiece(selectedPiece))
                return false;
            
            RefreshPiecesUI();
            return true;
        }
        
        public void HandlePreview(FurnitureOrientation orientation)
        {
            if (!HasPreview())
                return;
            
            ShowPreview(out Vector3 position);
            MovePreview(position, orientation);
        }

        public void TryPrepareNewPreview(IFurniturePiece selectedPiece)
        {
            if (HasPreview(selectedPiece))
                return;
            
            Destroy();

            if (HasSelectedPiece(selectedPiece))
                return;
            
            PrepareNewPreview(selectedPiece);
        }

        public void TryTogglingPreview(PlayerMode mode)
        {
            if (!HasPreview())
                return;

            TogglePreview(mode);
        }

        public void Destroy()
        {
            if (!HasPreview())
                return;
            
            Object.Destroy(_preview);
            ClearPreview();
        }

        private void ClearPreview() => _previewPiece = null;

        private bool TryGettingPlacementData(FurnitureOrientation orientation)
        {
            if (!_grid.Value.TryGetInsideCells(out _cells, out _position))
                return false;

            _rotation = Quaternion.LookRotation(orientation.ToForward());
            return true;
        }

        private bool AreCellsFree() => _placedFurniture.All(furniture => furniture.Cells.All(takenCell => _cells.All(cell => takenCell.Column != cell.Column || takenCell.Row != cell.Row)));

        private bool TryPlacingPiece(IFurniturePiece selectedPiece)
        {
            if (!_placeInput.action.triggered)
                return false;
            
            GameObject model = Object.Instantiate(selectedPiece.Prefab);
            model.transform.position = _worldPositionOffset + _position;
            model.transform.rotation = _rotation;
            
            _placedFurniture.Add(new PlacedFurniture
            {
                Piece = selectedPiece,
                Cells = _cells,
                Model = model
            });

            if (--selectedPiece.Count != 0)
                return true;
            
            _pieces.Value[_pieces.Value.IndexOf(selectedPiece)] = null;
            return true;
        }

        private bool HasPreview(IFurniturePiece selectedPiece) => selectedPiece?.Prefab == _previewPiece?.Prefab;

        private bool HasPreview() => _preview != null;

        private void TogglePreview(PlayerMode mode) => _preview.SetActive(mode == PlayerMode.Organization);

        private bool HasSelectedPiece(IFurniturePiece selectedPiece) => selectedPiece?.Prefab == null;

        private void PrepareNewPreview(IFurniturePiece selectedPiece)
        {
            _worldPositionOffset = selectedPiece.Offset;
            _preview = Object.Instantiate(selectedPiece.Prefab);

            foreach (Collider previewCollider in _preview.GetComponentsInChildren<Collider>())
                previewCollider.enabled = false;

            foreach (Renderer previewRenderer in _preview.GetComponentsInChildren<Renderer>())
                previewRenderer.material = _originalPreviewMaterial;
            
            _previewPiece = selectedPiece;
        }

        private void ShowPreview(out Vector3 center)
        {
            bool isAllowedToPlacePiece = _grid.Value.TryGetInsideCells(out List<GridCell> cells, out center);
            
            center += _worldPositionOffset;

            foreach (GridCell cell in cells)
            {
                if (!isAllowedToPlacePiece)
                    break;
                
                if (_placedFurniture.All(furniture => furniture.Cells.All(takenCell => takenCell.Column != cell.Column || takenCell.Row != cell.Row)))
                    continue;

                isAllowedToPlacePiece = false;
            }
            
            _originalPreviewMaterial.SetColor(PreviewColorId, isAllowedToPlacePiece ? _validSpotColor : _invalidSpotColor);
        }

        private void MovePreview(Vector3 center, FurnitureOrientation orientation)
        {
            _preview.transform.forward = orientation.ToForward();
            _preview.transform.position = center;
        }

        private void GetReferences(List<PlacedFurniture> placedFurniture, Material previewMaterial, Color invalidSpotColor, Color validSpotColor, InputActionReference placeInput)
        {
            _originalPreviewMaterial = new Material(previewMaterial);
            _invalidSpotColor = invalidSpotColor;
            _placedFurniture = placedFurniture;
            _validSpotColor = validSpotColor;
            _placeInput = placeInput;
        }

        private void RefreshPiecesUI() => _furnishingPanel.Present(_pieces.Value.Select(piece => new FurniturePieceData { Icon = piece?.Icon, Count = piece?.Count ?? 0}).ToList());
    }
}