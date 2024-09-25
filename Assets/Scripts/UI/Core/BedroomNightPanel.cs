using System;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
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
    
    public class BedroomNightPanel : AUIPanel, ISelectableFurniturePanel
    {
        [SerializeField] private SelectionButton selectionButtonPrefab;
        [SerializeField] private Transform selectionButtonsParent;
        [SerializeField] private TextMeshProUGUI costContainer;
        [SerializeField] private TextMeshProUGUI textContainer;
        [SerializeField] private Button toggleButton;
        [SerializeField] private RectTransform stuff1;
        [SerializeField] private RectTransform stuff2;

        private Action _toggleCallback;
        private bool _isEnabled;

        private void Start()
        {
            AddCallbacks();
            
            stuff1.localScale = _isEnabled ? Vector3.one : Vector3.zero;
            stuff2.localScale = _isEnabled ? Vector3.one : Vector3.zero;
        }

        public void Present(FurnishingData data)
        {
            foreach (SelectableFurniturePieceData pieceData in data.PiecesData)
            {
                Instantiate(selectionButtonPrefab, selectionButtonsParent).Init(pieceData);
            }
            
            Cache(data);
        }

        private void AddCallbacks() => toggleButton.onClick.AddListener(() => _toggleCallback?.Invoke());

        private void Cache(FurnishingData data) => _toggleCallback = data.ToggleCallback + Toggle;

        // todo: finish panel in general and fix this organization
        private void Toggle()
        {
            _isEnabled = !_isEnabled;
            stuff1.localScale = _isEnabled ? Vector3.one : Vector3.zero;
            stuff2.localScale = _isEnabled ? Vector3.one : Vector3.zero;
        }
    }
}