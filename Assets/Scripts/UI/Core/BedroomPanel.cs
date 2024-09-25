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
    
    public class BedroomPanel : AUIPanel, ISelectableFurniturePanel
    {
        [SerializeField] private SelectionButton selectionButtonPrefab;
        [SerializeField] private Transform selectionButtonsParent;
        [SerializeField] private TextMeshProUGUI costContainer;
        [SerializeField] private TextMeshProUGUI textContainer;
        [SerializeField] private Button toggleButton;

        private Action _toggleCallback;

        private void Start() => AddCallbacks();

        public void Present(FurnishingData data)
        {
            foreach (SelectableFurniturePieceData pieceData in data.PiecesData)
            {
                Instantiate(selectionButtonPrefab, selectionButtonsParent).Init(pieceData);
            }
            
            Cache(data);
        }

        private void AddCallbacks() => toggleButton.onClick.AddListener(() => _toggleCallback?.Invoke());

        private void Cache(FurnishingData data) => _toggleCallback = data.ToggleCallback;
    }
}