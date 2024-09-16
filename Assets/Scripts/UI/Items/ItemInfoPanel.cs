using System.Linq;
using System.Text.RegularExpressions;
using Items.Holders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Items
{
    public class ItemInfoPanel : AItemPanel<object>
    {
        [SerializeField] private TextMeshProUGUI itemNameTextContainer;
        [SerializeField] private Image holderTypeIconHolder;
        [SerializeField] private Image itemTypeIconHolder;
        [SerializeField] private GameObject itemTypeIconWrapper;
        [SerializeField] private GameObject panel;
        [SerializeField] private Sprite[] icons;

        private IItemHolder _presentedItemHolder;
        private object _presentedItem;

        private void Start() => Toggle();

        private void Update()
        {
            if (!HasItemChanged())
                return;

            Present(_presentedItemHolder);
        }

        public override void Present(IItemHolder itemHolder)
        {
            Cache(itemHolder);
            Toggle();
            TryUpdatingInfo();
        }

        private bool HasItemChanged()
        {
            if (_presentedItemHolder == null)
                return false;

            return _presentedItemHolder.Value != _presentedItem;
        }

        private void Cache(IItemHolder itemHolder)
        {
            _presentedItemHolder = itemHolder;
            _presentedItem = itemHolder.Value;
        }

        private void Toggle() => panel.SetActive((_presentedItemHolder != null && _presentedItemHolder is not CachedItemHolder) || _presentedItemHolder?.Value != null);

        private void TryUpdatingInfo()
        {
            string holderName = _presentedItemHolder.Name;
            string text = _presentedItemHolder.Value?.Name ?? holderName;
            holderTypeIconHolder.sprite = icons.FirstOrDefault(icon => holderName.Equals(icon.name.Replace("T_", "")));
            itemNameTextContainer.text = Regex.Replace(text.Replace("P_", ""), "(\\B[A-Z])", " $1");
            itemTypeIconHolder.sprite = icons.FirstOrDefault(icon => icon.name == $"T_{_presentedItemHolder.Value?.Name}");
            itemTypeIconWrapper.SetActive(itemTypeIconHolder.sprite != null);
        }
    }
}