using System;
using System.Collections.Generic;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
using UI.Helpers;
using UnityEngine;

namespace UI.Core
{
    public interface IItemShopPanel : IDependency
    {
        void Present(ItemPreview[] previews);
    }

    public class ItemPreview
    {
        public Func<bool> CanSellCallback { get; set; }
        public Func<bool> CanBuyCallback { get; set; }
        public Action SellCallback { get; set; }
        public Action BuyCallback { get; set; }
        public Sprite Icon { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
    }
    
    public class ItemShopPanel : AUIPanel, IItemShopPanel
    {
        [SerializeField] private Transform barParent;
        [SerializeField] private ItemBar barPrefab;

        private List<string> _previewNames = new();

        public void Present(ItemPreview[] previews)
        {
            foreach (ItemPreview preview in previews)
                Present(preview);
        }

        private void Present(ItemPreview preview)
        {
            if (!TryAdd(preview.Name))
                return;
            
            Instantiate(barPrefab, barParent).Present(preview);
        }

        private bool TryAdd(string previewName)
        {
            if (_previewNames.Contains(previewName))
                return false;

            _previewNames.Add(previewName);
            return true;
        }
    }
}