using System.Collections.Generic;
using System.Linq;
using Grids;
using Internal.Dependencies.Core;
using Player;
using UI.Shared;
using UnityEngine.InputSystem;

namespace Furniture
{
    public class FurnitureSelector
    {
        private readonly List<InputActionReference> _selectionsInput;
        private readonly PlayerModeProxy _playerMode;

        private DependencyRecipe<DependencyList<IFurniturePiece>> _pieces = DependencyInjector.GetRecipe<DependencyList<IFurniturePiece>>();
        private IFurnishingPanel _furnishingPanel = DependencyInjector.Get<IFurnishingPanel>();
        private DependencyRecipe<IGrid> _grid = DependencyInjector.GetRecipe<IGrid>();
        private GridDimensions _selectedPieceDimensions;
        private IFurniturePiece _selectedPiece;
        private bool _isAllowedToPlacePiece;
        private int _selectedPieceIndex;

        public GridDimensions Dimensions => _selectedPieceDimensions;
        
        private static readonly GridDimensions DefaultDimensions = new GridDimensions { Width = 1, Height = 1 };
        
        public FurnitureSelector(List<InputActionReference> selectionsInput, PlayerModeProxy playerMode)
        {
            _selectionsInput = selectionsInput;
            _playerMode = playerMode;
        }

        public void HandleSelection(out IFurniturePiece selectedPiece)
        {
            ReadOutput(out selectedPiece);
            
            if (!TrySelectingOtherPiece())
                return;

            ReadOutput(out selectedPiece);
            RefreshSelectionUI();
        }

        public void RemoveSelection() => _selectedPiece = null;

        private void ReadOutput(out IFurniturePiece selectedPiece) => selectedPiece = _selectedPiece;
        
        private bool TrySelectingOtherPiece()
        {
            if (_playerMode.Value == PlayerMode.Modification)
                return false;
            
            if (_grid.Value == null)
                return false;
            
            foreach (InputActionReference input in _selectionsInput.Where(input => input.action.triggered))
                _selectedPieceIndex = _selectionsInput.IndexOf(input);

            _selectedPieceIndex = (_selectedPieceIndex + _selectionsInput.Count) % _selectionsInput.Count;

            if (_selectedPiece == _pieces.Value[_selectedPieceIndex])
                return false;

            _selectedPiece = _pieces.Value[_selectedPieceIndex];
            _selectedPieceDimensions = _selectedPiece != null ? _selectedPiece.Dimensions : DefaultDimensions;
            return true;
        }

        private void RefreshSelectionUI() => _furnishingPanel.Present(_selectedPieceIndex);
    }
}