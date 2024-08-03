using Items.Implementations;
using TMPro;
using UnityEngine;

namespace UI.ItemPanels
{
    public class WaterItemPanel : AItemPanel<IWaterItem>
    {
        [SerializeField] private TextMeshProUGUI temperatureTextContainer;

        private void Update()
        {
            TryGetItem(out IWaterItem waterItem);
            RefreshText(GetText(waterItem));
        }

        private void RefreshText(string text)
        {
            if (temperatureTextContainer.text == text)
                return;

            temperatureTextContainer.gameObject.SetActive(text != "");
            temperatureTextContainer.text = text;
        }

        private string GetText(IWaterItem waterItem)
        {
            if (waterItem?.HasWater != true)
                return "";

            return $"{(int)waterItem.WaterTemperature}\u00b0C";
        }
    }
}