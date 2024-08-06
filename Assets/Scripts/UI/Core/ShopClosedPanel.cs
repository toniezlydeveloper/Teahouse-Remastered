using System;
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

    public class ShopClosedPanel : AUIPanel, IItemShopPanel
    {
        [SerializeField] private SaleItemBar barPrefab;
        [SerializeField] private Transform barParent;
        [SerializeField] private GameObject[] panels;

        private void OnEnable()
        {
            panels[0].SetActive(false);
        }

        public void Present(SaleItemPreview[] previews)
        {
            foreach (SaleItemPreview preview in previews)
                Instantiate(barPrefab, barParent).Present(preview);

            foreach (GameObject panel in panels)
                panel.SetActive(true);
        }
    }
}