using System;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
using States;
using TMPro;
using UI.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Core
{
    public interface ISelectableFurniturePanel : IDependency
    {
        void Present(FurnishingData data);
    }

    public class FurnishingData
    {
        public SelectableFurniturePieceData[] PiecesData { get; set; }
        public Action ToggleCallback { get; set; }
    }

    public class SelectableFurniturePieceData
    {
        public Action SelectionCallback { get; set; }
        public Sprite Icon { get; set; }
        public string Text { get; set; }
        public int Cost { get; set; }
    }
    
    public class BedroomNightPanel : AUIPanel, ISelectableFurniturePanel, IFurnishingListener
    {
        [SerializeField] private SelectionButton selectionButtonPrefab;
        [SerializeField] private RectTransform[] selectionElements;
        [SerializeField] private Transform selectionButtonsParent;
        [SerializeField] private TextMeshProUGUI costContainer;
        [SerializeField] private TextMeshProUGUI textContainer;
        [SerializeField] private Button toggleButton;

        private Action _toggleCallback;

        private void Start() => AddCallbacks();

        private void OnEnable() => DependencyInjector.AddRecipeElement(GetComponent<IFurnishingListener>());

        private void OnDisable() => DependencyInjector.RemoveRecipeElement(GetComponent<IFurnishingListener>());

        public void Present(FurnishingData data)
        {
            foreach (SelectableFurniturePieceData pieceData in data.PiecesData)
            {
                Instantiate(selectionButtonPrefab, selectionButtonsParent).Init(pieceData);
            }
            
            Cache(data);
        }

        public void Toggle(bool state)
        {
            foreach (RectTransform selectionElement in selectionElements)
            {
                selectionElement.localScale = state ? Vector3.one : Vector3.zero;
            }

            toggleButton.transform.localScale = state ? -Vector3.one : Vector3.one;
        }

        private void AddCallbacks() => toggleButton.onClick.AddListener(() => _toggleCallback?.Invoke());

        private void Cache(FurnishingData data) => _toggleCallback = data.ToggleCallback;
    }
}