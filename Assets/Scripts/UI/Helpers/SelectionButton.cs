using System;
using Furniture;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers
{
    public class SelectionButton : MonoBehaviour
    {
        [SerializeField] private Image iconContainer;
        [SerializeField] private Button button;
        [SerializeField] private Color unselectedColor;
        [SerializeField] private Color selectedColor;

        private FurnitureCategory _presentedCategory;
        private Func<bool> _isSelectedCallback;
        private Action _selectionCallback;
        private Image _background;

        public FurnitureCategory PresentedCategory => _presentedCategory;

        private void Awake() => _background = GetComponent<Image>();

        private void Start()
        {
            button.onClick.AddListener(RaiseSelectionCallback);
            button.onClick.AddListener(RefreshSelection);
        }

        public void Init(SelectableFurniturePieceData pieceData, Action callback)
        {
            Cache(pieceData, callback);
            Setup(pieceData);
        }

        public void RefreshSelection() => _background.color = _isSelectedCallback.Invoke() ? selectedColor : unselectedColor;

        public void RaiseSelectionCallback() => _selectionCallback.Invoke();

        private void Cache(SelectableFurniturePieceData pieceData, Action callback)
        {
            _selectionCallback = pieceData.SelectionCallback + callback;
            _isSelectedCallback = pieceData.IsSelectedCallback;
            _presentedCategory = pieceData.Category;
        }

        private void Setup(SelectableFurniturePieceData pieceData) => iconContainer.sprite = pieceData.Icon;
    }
}