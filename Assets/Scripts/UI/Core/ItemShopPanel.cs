using System;
using System.Collections.Generic;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
using TMPro;
using UI.Helpers;
using UnityEngine;

namespace UI.Core
{
    public interface IItemShopPanel : IDependency
    {
        void Present(ItemPreview[] previews, bool isSelling);
    }

    public class ItemPreview
    {
        public string CallbackName { get; set; }
        public Action Callback { get; set; }
        public Sprite Icon { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
    }
    
    public class ItemShopPanel : AUIPanel, IItemShopPanel
    {
        [SerializeField] private GameObject sellingBarsContainer;
        [SerializeField] private GameObject buyingBarsContainer;
        [SerializeField] private Transform sellingBarParent;
        [SerializeField] private Transform buyingBarParent;
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private ItemBar barPrefab;

        private List<string> _previewNames = new();

        private const string SellText = "Sell";
        private const string BuyText = "Buy";
        
        public void Present(ItemPreview[] previews, bool isSelling)
        {
            foreach (ItemPreview preview in previews)
                Present(preview, isSelling);

            RefreshHeader(isSelling);
            Toggle(isSelling);
        }

        private void Present(ItemPreview preview, bool isSelling)
        {
            if (!TryAdd(preview.Name, isSelling))
                return;
            
            Instantiate(barPrefab, isSelling ? sellingBarParent : buyingBarParent).Present(preview);
        }

        private void RefreshHeader(bool isSelling) => header.text = isSelling ? SellText : BuyText;

        private void Toggle(bool isSelling)
        {
            sellingBarsContainer.SetActive(isSelling);
            buyingBarsContainer.SetActive(!isSelling);
        }

        private bool TryAdd(string previewName, bool isSelling)
        {
            string revisedPreviewName = $"{previewName}{isSelling}";
            
            if (_previewNames.Contains(revisedPreviewName))
                return false;

            _previewNames.Add(revisedPreviewName);
            return true;
        }
    }
}