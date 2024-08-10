using System;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers
{
    public class ItemBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI callbackNameContainer;
        [SerializeField] private TextMeshProUGUI nameContainer;
        [SerializeField] private TextMeshProUGUI costContainer;
        [SerializeField] private Image iconContainer;
        [SerializeField] private Button buyButton;

        private Action _clickCallback;

        private void Awake() => buyButton.onClick.AddListener(() => _clickCallback?.Invoke());

        public void Present(ItemPreview preview)
        {
            CacheCallback(preview);
            RefreshUI(preview);
        }

        private void RefreshUI(ItemPreview preview)
        {
            callbackNameContainer.text = preview.CallbackName;
            costContainer.text = preview.Cost.ToString();
            iconContainer.sprite = preview.Icon;
            nameContainer.text = preview.Name;
        }

        private void CacheCallback(ItemPreview preview) => _clickCallback = preview.Callback;
    }
}