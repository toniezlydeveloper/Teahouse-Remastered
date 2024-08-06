using System;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers
{
    public class SaleItemBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameContainer;
        [SerializeField] private Image iconContainer;
        [SerializeField] private Button buyButton;

        private Action _buyCallback;

        private void Awake() => buyButton.onClick.AddListener(() => _buyCallback?.Invoke());

        public void Present(SaleItemPreview preview)
        {
            nameContainer.text = preview.Name;
            iconContainer.sprite = preview.Icon;
            _buyCallback = preview.BuyCallback;
        }
    }
}