using System;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers
{
    public class ItemBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameContainer;
        [SerializeField] private TextMeshProUGUI costContainer;
        [SerializeField] private Image iconContainer;
        [SerializeField] private Button sellButton;
        [SerializeField] private Button buyButton;

        private Func<bool> _canSellCallback;
        private Func<bool> _canBuyCallback;
        private Action _sellCallback;
        private Action _buyCallback;

        private void Awake()
        {
            sellButton.onClick.AddListener(() =>
            {
                RaiseSellCallback();
                ToggleButtons();
            });
            
            buyButton.onClick.AddListener(() =>
            {
                RaiseBuyCallback();
                ToggleButtons();
            });
        }

        public void Present(ItemPreview preview)
        {
            CacheCallbacks(preview);
            RefreshUI(preview);
            ToggleButtons();
        }

        public void ToggleButtons()
        {
            sellButton.interactable = _canSellCallback.Invoke();
            buyButton.interactable = _canBuyCallback.Invoke();
        }

        private void RefreshUI(ItemPreview preview)
        {
            costContainer.text = preview.Cost.ToString();
            iconContainer.sprite = preview.Icon;
            nameContainer.text = preview.Name;
        }

        private void CacheCallbacks(ItemPreview preview)
        {
            _canSellCallback = preview.CanSellCallback;
            _canBuyCallback = preview.CanBuyCallback;
            _sellCallback = preview.SellCallback;
            _buyCallback = preview.BuyCallback;
        }

        private void RaiseSellCallback() => _sellCallback?.Invoke();

        private void RaiseBuyCallback() => _buyCallback?.Invoke();
    }
}