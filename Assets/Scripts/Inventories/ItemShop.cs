using System;
using System.Collections.Generic;
using System.Linq;
using Furniture;
using Internal.Dependencies.Core;
using UI.Core;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class SaleItem
    {
        [field:SerializeField] public FurniturePiece Piece { get; set; }
        [field:SerializeField] public string Name { get; set; }
        [field:SerializeField] public int Cost { get; set; }
    }

    public class ItemShop : MonoBehaviour
    {
        [SerializeField] private SaleItem[] saleItems;

        private IItemShopPanel _panel = DependencyInjector.Get<IItemShopPanel>();
        private IInventory _inventory = DependencyInjector.Get<IInventory>();

        public void Show()
        {
            _panel.Present(saleItems.Select(item => new SaleItemPreview { BuyCallback = () =>
                {
                    int index = _inventory.Pieces.IndexOf(null);
                    _inventory.Pieces[index] = new FurniturePiece(item.Piece);
                },
            Icon = item.Piece.Icon, Name = item.Name }).ToArray());
        }
    }
}