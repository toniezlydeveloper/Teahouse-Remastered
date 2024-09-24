using System.Collections.Generic;
using System.Linq;
using Currency;
using Grids;
using Internal.Dependencies.Core;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Furniture
{
    public class FurniturePicker
    {
        private ICurrencyHolder _currencyHolder = DependencyInjector.Get<ICurrencyHolder>();
        private DependencyRecipe<IGrid> _grid = DependencyInjector.GetRecipe<IGrid>();
        private Dictionary<Renderer, Material> _originalsByRenderer = new();
        private List<PlacedFurniture> _placedFurniture;
        private Material _originalPreviewMaterial;
        private InputActionReference _pickInput;
        private GameObject _model;

        public FurniturePicker(List<PlacedFurniture> placedFurniture, InputActionReference pickInput, Material previewMaterial)
        {
            _originalPreviewMaterial = new Material(previewMaterial);
            _placedFurniture = placedFurniture;
            _pickInput = pickInput;
        }

        public void TryTogglingPreview(PlayerMode mode)
        {
            if (ShouldShowPreview(mode))
                return;
            
            Restore();
        }

        public bool HasPickUp() => IsGridPointingAtPlacedPiece();

        public void Restore()
        {
            if (HasModel())
                return;
            
            RestoreMaterials();
            ClearModel();
        }

        public void HandlePickUp()
        {
            if (!TryGetFurniture(out PlacedFurniture furniture))
            {
                Restore();
                return;
            }

            if (HasSwitchedModels(furniture))
            {
                RestoreMaterials();
                CacheMaterials(furniture);
            }
            
            if (!ReceivedPickInput())
                return;
            
            ReturnMoney(furniture);
            ClearModel(furniture);
            Clear(furniture);
        }

        private static void ClearModel(PlacedFurniture furniture) => Object.Destroy(furniture.Model);

        private bool TryGetFurniture(out PlacedFurniture furniture)
        {
            furniture = null;
            
            if (_grid.Value == null)
                return false;
            
            _grid.Value.GetPointerCell(out GridCell pointer);
            furniture = _placedFurniture.FirstOrDefault(furniture => furniture.Cells.Any(takenCell => takenCell.Column == pointer.Column && takenCell.Row == pointer.Row));
            return furniture != null;
        }

        private bool IsGridPointingAtPlacedPiece()
        {
            if (_grid.Value == null)
                return false;
            
            _grid.Value.GetPointerCell(out GridCell pointer);
            return _placedFurniture.Any(furniture => furniture.Cells.Any(takenCell => takenCell.Column == pointer.Column && takenCell.Row == pointer.Row));
        }

        private static bool ShouldShowPreview(PlayerMode mode) => mode == PlayerMode.Organization;

        private bool HasModel() => _model == null;

        private void RestoreMaterials()
        {
            foreach ((Renderer originalRenderer, Material originalMaterial) in _originalsByRenderer)
            {
                if (!originalRenderer)
                    continue;
                
                originalRenderer.material = originalMaterial;
            }
            
            _originalsByRenderer.Clear();
        }

        private void CacheMaterials(PlacedFurniture furniture)
        {
            foreach (Renderer originalRenderer in furniture.Model.GetComponentsInChildren<Renderer>())
            {
                _originalsByRenderer.Add(originalRenderer, originalRenderer.material);
                originalRenderer.material = _originalPreviewMaterial;
            }
            
            _model = furniture.Model;
        }

        private bool HasSwitchedModels(PlacedFurniture furniture) => _model != furniture.Model;

        private void ClearModel() => _model = null;

        private void ReturnMoney(PlacedFurniture furniture) => _currencyHolder.Add(furniture.Piece.Cost);

        private void Clear(PlacedFurniture furniture) => _placedFurniture.Remove(furniture);

        private bool ReceivedPickInput() => _pickInput.action.triggered;
    }
}