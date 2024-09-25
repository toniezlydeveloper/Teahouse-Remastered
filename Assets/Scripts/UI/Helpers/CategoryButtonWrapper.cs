using System;
using Furniture;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers
{
    public class CategoryButtonWrapper : MonoBehaviour
    {
        [SerializeField] private FurnitureCategory category;
        [SerializeField] private Color unselectedColor;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Button button;
        [SerializeField] private Image icon;

        private Action _selectionCallback;
        
        public FurnitureCategory Category => category;

        private void Awake() => button.onClick.AddListener(() => _selectionCallback?.Invoke());

        public void Init(Action selectionCallback) => _selectionCallback = selectionCallback;
        
        public void Toggle(bool state) => icon.color = state ? selectedColor : unselectedColor;
    }
}