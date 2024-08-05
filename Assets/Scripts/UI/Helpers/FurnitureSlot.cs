using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers
{
    public class FurnitureSlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countContainer;
        [SerializeField] private Image iconContainer;
        [SerializeField] private Image highlightContainer;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color unselectedColor;
        
        public void Present(Sprite icon, int? count)
        {
            countContainer.text = count.HasValue ? count.ToString() : "";
            iconContainer.sprite = icon;
        }

        public void Present(bool isSelected) => highlightContainer.color = isSelected ? selectedColor : unselectedColor;
    }
}