using Items.Implementations;
using TMPro;
using UnityEngine;

namespace UI.ItemPresentation
{
    public class WaterRangeItemPanel : AItemPanel<Order>
    {
        [SerializeField] private TextMeshProUGUI temperatureRangeTextContainer;

        private void Update()
        {
            TryGetItem(out Order order);
            RefreshText(GetText(order));
        }

        private void RefreshText(string text)
        {
            if (temperatureRangeTextContainer.text == text)
                return;

            temperatureRangeTextContainer.gameObject.SetActive(text != "");
            temperatureRangeTextContainer.text = text;
        }

        private string GetText(Order order)
        {
            if (order == null)
                return "";
            
            return $"{(int)order.MinWaterTemperature}-{(int)order.MaxWaterTemperature}\u00b0C";
        }
    }
}