using System.Linq;
using Grids;
using Internal.Dependencies.Core;
using Player;
using UI.Core;

namespace Furniture
{
    public class FurnitureSelector
    {
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private ISelectableFurniturePanel _selectablePanel = DependencyInjector.Get<ISelectableFurniturePanel>();
        private GridDimensions _selectedPieceDimensions;
        private IFurniturePiece _selectedPiece;

        public GridDimensions Dimensions => _selectedPieceDimensions;
        
        private static readonly GridDimensions DefaultDimensions = new GridDimensions { Width = 1, Height = 1 };
        
        public FurnitureSelector(PurchasableItemsConfig config, PlayerModeProxy playerMode) => _selectablePanel.Present(new FurnishingData
        {
            ToggleCallback = () => _playerModeToggle.Value.Toggle(playerMode.Value == PlayerMode.Modification ? PlayerMode.Organization : PlayerMode.Modification),
            PiecesData = config.Set.Select(item => new SelectableFurniturePieceData
            {
                SelectionCallback = () => HandleSelection(item),
                Icon = item.Icon,
                Cost = item.Cost
            }).ToArray()
        });

        public void HandleSelection(out IFurniturePiece selectedPiece) => ReadOutput(out selectedPiece);

        private void HandleSelection(IFurniturePiece piece)
        {
            _selectedPiece = _selectedPiece == piece ? null : piece;
            _selectedPieceDimensions = _selectedPiece != null ? _selectedPiece.Dimensions : DefaultDimensions;
        }

        private void ReadOutput(out IFurniturePiece selectedPiece) => selectedPiece = _selectedPiece;
    }
}