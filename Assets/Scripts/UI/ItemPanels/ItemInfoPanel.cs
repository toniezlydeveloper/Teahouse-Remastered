using System.Linq;
using System.Text.RegularExpressions;
using Items.Holders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ItemPanels
{
    public class ItemInfoPanel : AItemPanel<object>
    {
        [SerializeField] private TextMeshProUGUI itemNameTextContainer;
        [SerializeField] private Image holderTypeIconHolder;
        [SerializeField] private Image itemTypeIconHolder;
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

        private void Toggle() => panel.SetActive(_presentedItemHolder?.Value != null);

        private void TryUpdatingInfo()
        {
            if (_presentedItemHolder.Value == null)
                return;

            string holderTypeName;
            
            if (_presentedItemHolder is WorldSpaceItemHolder worldSpaceItemHolder)
                holderTypeName = worldSpaceItemHolder.name.Replace("(Clone)", "");
            else
                holderTypeName = _presentedItemHolder.GetType().Name;
            
            holderTypeIconHolder.sprite = icons.FirstOrDefault(icon => holderTypeName.Contains(icon.name.Replace("T_", "")));
            itemNameTextContainer.text = Regex.Replace(_presentedItemHolder.Value.Name, "(\\B[A-Z])", " $1");
            itemTypeIconHolder.sprite = icons.FirstOrDefault(icon => icon.name == $"T_{_presentedItemHolder.Value.Name}");
        }
    }
}