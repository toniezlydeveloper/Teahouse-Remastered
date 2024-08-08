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
        void Present(SaleItemPreview[] previews);
    }

    public class SaleItemPreview
    {
        public Action BuyCallback { get; set; }
        public Sprite Icon { get; set; }
        public string Name { get; set; }
    }
    
    public class ItemShopPanel : AUIPanel, IItemShopPanel
    {
        [SerializeField] private SaleItemBar barPrefab;
        [SerializeField] private Transform barParent;

        private List<string> _previewNames = new();
        
        public void Present(SaleItemPreview[] previews)
        {
            foreach (SaleItemPreview preview in previews)
                Present(preview);
        }

        private void Present(SaleItemPreview preview)
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