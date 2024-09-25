using System;
using System.Collections.Generic;
using System.Linq;
using Furniture;
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
        public Func<bool> IsSelectedCallback { get; set; }
        public Action SelectionCallback { get; set; }
        public FurnitureCategory Category { get; set; }
        public Sprite Icon { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int Cost { get; set; }
    }
    
    public class BedroomNightPanel : AUIPanel, ISelectableFurniturePanel, IFurnishingListener
    {
        [SerializeField] private CategoryButtonWrapper[] categoryButtonWrappers;
        [SerializeField] private SelectionButton selectionButtonPrefab;
        [SerializeField] private RectTransform[] selectionElements;
        [SerializeField] private Transform selectionButtonsParent;
        [SerializeField] private TextMeshProUGUI costContainer;
        [SerializeField] private TextMeshProUGUI textContainer;
        [SerializeField] private TextMeshProUGUI nameContainer;
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private Button toggleButton;

        private List<SelectionButton> _selectionButtons = new();
        private Action _toggleCallback;

        private void Start() => AddCallbacks();

        private void OnEnable() => DependencyInjector.AddRecipeElement(GetComponent<IFurnishingListener>());

        private void OnDisable() => DependencyInjector.RemoveRecipeElement(GetComponent<IFurnishingListener>());

        public void Present(FurnishingData data)
        {
            foreach (SelectableFurniturePieceData pieceData in data.PiecesData)
            {
                SelectionButton selectionButton = Instantiate(selectionButtonPrefab, selectionButtonsParent);
                Init(selectionButton, pieceData);
            }
            
            SelectCategory(GetInitialCategory());
            Cache(data);
        }

        public void Toggle(bool state)
        {
            ToggleSelectionElements(state);
            ToggleButton(state);
        }

        private FurnitureCategory GetInitialCategory() => categoryButtonWrappers[0].Category;

        private void Init(SelectionButton selectionButton, SelectableFurniturePieceData pieceData)
        {
            selectionButton.Init(pieceData, () =>
            {
                foreach (SelectionButton otherButton in _selectionButtons)
                {
                    otherButton.RefreshSelection();
                }

                if (pieceData.IsSelectedCallback.Invoke())
                {
                    costContainer.text = pieceData.Cost.ToString();
                    nameContainer.text = pieceData.Name;
                    textContainer.text = pieceData.Text;
                    infoPanel.SetActive(true);
                }
                else
                {
                    infoPanel.SetActive(false);
                }
            });
            
            _selectionButtons.Add(selectionButton);
        }

        private void ToggleSelectionElements(bool state)
        {
            foreach (RectTransform selectionElement in selectionElements)
            {
                selectionElement.localScale = state ? Vector3.one : Vector3.zero;
            }
        }

        private void ToggleButton(bool state) => toggleButton.transform.localScale = state ? -Vector3.one : Vector3.one;

        private void SelectCategory(FurnitureCategory category)
        {
            foreach (SelectionButton selectionButton in _selectionButtons)
            {
                selectionButton.gameObject.SetActive(selectionButton.PresentedCategory == category);
            }
            
            foreach (CategoryButtonWrapper categoryButton in categoryButtonWrappers)
            {
                categoryButton.Toggle(categoryButton.Category == category);
            }
        }

        private void AddCallbacks()
        {
            foreach (CategoryButtonWrapper wrapper in categoryButtonWrappers)
            {
                wrapper.Init(() => SelectCategory(wrapper.Category));
            }
            
            toggleButton.onClick.AddListener(() => _toggleCallback?.Invoke());
        }

        private void Cache(FurnishingData data) => _toggleCallback = data.ToggleCallback;
    }
}