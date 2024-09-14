using System.Linq;
using Currency;
using Furniture;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using Trading;
using UI.Core;
using UI.Shared;
using UnityEngine.InputSystem;

namespace States
{
    public class ItemShopState : AState
    {
        private DependencyRecipe<DependencyList<IFurniturePiece>> _pieces = DependencyInjector.GetRecipe<DependencyList<IFurniturePiece>>();
        private IFurnishingPanel _furnishingPanel;
        private ICurrencyHolder _currencyHolder;
        private InputActionReference _controls;
        private IItemShopPanel _itemShopPanel;
        private InputActionReference _back;
        private TradeItemsConfig _tradeItems;

        public ItemShopState(TradeItemsConfig tradeItems, IItemShopPanel itemShopPanel, InputActionReference controls, InputActionReference back, IFurnishingPanel furnishingPanel, ICurrencyHolder currencyHolder)
        {
            _furnishingPanel = furnishingPanel;
            _currencyHolder = currencyHolder;
            _itemShopPanel = itemShopPanel;
            _tradeItems = tradeItems;
            _controls = controls;
            _back = back;
        }

        public override void OnEnter()
        {
            ToggleControls(false);
            EnableFurniturePreview();
            PresentShop(GetPreviews());
        }

        public override void OnExit() => ToggleControls(true);

        protected override void AddConditions() => AddCondition<BedroomState>(ReceivedBackInput);

        private bool ReceivedBackInput() => _back.action.triggered;

        private void ToggleControls(bool state)
        {
            if (state)
                _controls.action.Enable();
            else
                _controls.action.Disable();
        }

        private void EnableFurniturePreview() => _furnishingPanel.Present(true);

        private void PresentShop(ItemPreview[] previews) => _itemShopPanel.Present(previews);

        private ItemPreview[] GetPreviews() => _tradeItems.Set.Select(item => new ItemPreview
            {
                CanSellCallback = () => CanSellItem(item),
                SellCallback = () => TrySellingItem(item),
                CanBuyCallback = () => CanBuyItem(item),
                BuyCallback = () => TryBuyingItem(item),
                Icon = item.Piece.Icon,
                Name = item.Name,
                Cost = item.Cost
            })
            .ToArray();

        private bool CanBuyItem(TradeItem item)
        {
            if (!_currencyHolder.Has(item.Cost))
                return false;

            if (_pieces.Value.Any(piece => piece == null))
                return true;

            return _pieces.Value.Any(piece => piece?.Prefab == item.Piece.Prefab);
        }

        private bool CanSellItem(TradeItem item)
        {
            IFurniturePiece piece = _pieces.Value.FirstOrDefault(piece => piece?.Prefab == item.Piece.Prefab);

            if (piece == null)
                return false;

            return piece.Count > 0;
        }
        
        private void TryBuyingItem(TradeItem item)
        {
            if (!_currencyHolder.TrySpend(item.Cost))
                return;
            
            int index = _pieces.Value.FindIndex(piece => piece?.Prefab == item.Piece.Prefab);

            if (index < 0)
                index = _pieces.Value.FindIndex(piece => piece == null);

            if (_pieces.Value[index] != null)
                _pieces.Value[index].Count++;
            else
                _pieces.Value[index] = new FurniturePiece(item.Piece);
            
            _furnishingPanel.Present(_pieces.Value.Select(piece => new FurniturePieceData { Icon = piece?.Icon, Count = piece?.Count ?? 0}).ToList());
        }

        private void TrySellingItem(TradeItem item)
        {
            int index = _pieces.Value.FindIndex(piece => item.Piece.Prefab == piece?.Prefab);
            
            if (index < 0)
                return;
            
            if (_pieces.Value[index] == null)
                return;
            
            _currencyHolder.Add(item.Cost);
            _pieces.Value[index].Count--;

            if (_pieces.Value[index].Count < 1)
                _pieces.Value[index] = null;
            
            _furnishingPanel.Present(_pieces.Value.Select(piece => new FurniturePieceData { Icon = piece?.Icon, Count = piece?.Count ?? 0}).ToList());
        }
    }
}