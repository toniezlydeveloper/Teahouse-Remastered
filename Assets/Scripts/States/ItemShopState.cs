using System;
using System.Collections.Generic;
using System.Linq;
using Currency;
using Furniture;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using UI.Core;
using UI.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace States
{
    public interface IInventory : IDependency
    {
        List<FurniturePiece> Pieces { get; }
    }

    [Serializable]
    public class SaleItem
    {
        [field:SerializeField] public FurniturePiece Piece { get; set; }
        [field:SerializeField] public string Name { get; set; }
        [field:SerializeField] public int Cost { get; set; }
    }

    public class ItemShopState : AState, IInventory
    {
        private IFurnishingPanel _furnishingPanel;
        private ICurrencyHolder _currencyHolder;
        private InputActionReference _controls;
        private IItemShopPanel _itemShopPanel;
        private InputActionReference _back;
        private SaleItem[] _saleItems;

        public List<FurniturePiece> Pieces { get; } = new()
        {
            null,
            null,
            null,
            null,
            null
        };

        public ItemShopState(SaleItem[] saleItems, IItemShopPanel itemShopPanel, InputActionReference controls, InputActionReference back, IFurnishingPanel furnishingPanel, ICurrencyHolder currencyHolder)
        {
            _furnishingPanel = furnishingPanel;
            _currencyHolder = currencyHolder;
            _itemShopPanel = itemShopPanel;
            _saleItems = saleItems;
            _controls = controls;
            _back = back;
        }

        public override void OnEnter()
        {
            ToggleControls(false);
            EnableFurniturePreview();
            PresentShop();
        }

        public override void OnExit() => ToggleControls(true);

        protected override void AddConditions() => AddCondition<ShopClosedState>(ReceivedBackInput);

        private bool ReceivedBackInput() => _back.action.triggered;

        private void ToggleControls(bool state)
        {
            if (state)
                _controls.action.Enable();
            else
                _controls.action.Disable();
        }

        private void EnableFurniturePreview() => _furnishingPanel.Present(true);

        private void PresentShop() => _itemShopPanel.Present(_saleItems.Select(item => new SaleItemPreview
            {
                BuyCallback = () => TryBuyingItem(item),
                Icon = item.Piece.Icon,
                Name = item.Name
            })
            .ToArray());

        private void TryBuyingItem(SaleItem item)
        {
            if (!_currencyHolder.TrySpend(item.Cost))
                return;
            
            int index = Pieces.FindIndex(piece => piece?.Prefab == item.Piece.Prefab);

            if (index < 0)
                index = Pieces.FindIndex(piece => piece == null);

            if (Pieces[index] != null)
                Pieces[index].Count++;
            else
                Pieces[index] = new FurniturePiece(item.Piece);
            
            _furnishingPanel.Present(Pieces.Select(piece => new FurniturePieceData { Icon = piece?.Icon, Count = piece?.Count ?? 0}).ToList());
        }
    }
}