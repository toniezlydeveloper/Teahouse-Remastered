using System;
using System.Collections.Generic;
using System.Linq;
using Currency;
using Furniture;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using UI.Core;
using UI.Shared;
using UnityEngine.InputSystem;

namespace States
{
    public class TradeItemsState : AState
    {
        private DependencyRecipe<DependencyList<IFurniturePiece>> _pieces = DependencyInjector.GetRecipe<DependencyList<IFurniturePiece>>();
        private IFurnishingPanel _furnishingPanel;
        private ICurrencyHolder _currencyHolder;
        private InputActionReference _controls;
        private IItemShopPanel _itemShopPanel;
        private InputActionReference _toggle;
        private InputActionReference _back;
        private TradeItem[] _tradeItems;
        private bool _isSelling;

        private const string SellItemName = "Sell";
        private const string BuyItemName = "Buy";

        public TradeItemsState(TradeItem[] tradeItems, IItemShopPanel itemShopPanel, InputActionReference controls, InputActionReference toggle, InputActionReference back, IFurnishingPanel furnishingPanel, ICurrencyHolder currencyHolder)
        {
            _furnishingPanel = furnishingPanel;
            _currencyHolder = currencyHolder;
            _itemShopPanel = itemShopPanel;
            _tradeItems = tradeItems;
            _controls = controls;
            _toggle = toggle;
            _back = back;
        }

        public override void OnEnter()
        {
            ToggleControls(false);
            EnableFurniturePreview();
            TogglePreview(false);
            PresentShop(GetPreviews());
        }

        public override void OnExit() => ToggleControls(true);

        public override Type OnUpdate()
        {
            if (!ReceivedToggleInput())
                return null;
            
            TogglePreview();
            PresentShop(GetPreviews());
            return null;
        }

        protected override void AddConditions() => AddCondition<ShopClosedState>(ReceivedBackInput);

        private bool ReceivedToggleInput() => _toggle.action.triggered;

        private bool ReceivedBackInput() => _back.action.triggered;

        private void ToggleControls(bool state)
        {
            if (state)
                _controls.action.Enable();
            else
                _controls.action.Disable();
        }

        private void EnableFurniturePreview() => _furnishingPanel.Present(true);

        private void TogglePreview(bool state) => _isSelling = state;

        private void TogglePreview() => _isSelling = !_isSelling;

        private void PresentShop(ItemPreview[] previews) => _itemShopPanel.Present(previews, _isSelling);

        private ItemPreview[] GetPreviews()
        {
            IEnumerable<ItemPreview> previews = ShouldGetSellPreviews() ? GetSellPreviews() : GetBuyPreviews();
            return previews.ToArray();
        }

        private bool ShouldGetSellPreviews() => _isSelling;

        private IEnumerable<ItemPreview> GetSellPreviews() => _tradeItems.Select(item => new ItemPreview
            {
                Callback = () => TrySellingItem(item),
                CallbackName = SellItemName,
                Icon = item.Piece.Icon,
                Name = item.Name,
                Cost = item.Cost
            })
            .ToArray();

        private IEnumerable<ItemPreview> GetBuyPreviews() => _tradeItems.Select(item => new ItemPreview
            {
                Callback = () => TryBuyingItem(item),
                CallbackName = BuyItemName,
                Icon = item.Piece.Icon,
                Name = item.Name,
                Cost = item.Cost
            })
            .ToArray();
        
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