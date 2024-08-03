using Items.Implementations;
using TMPro;
using UnityEngine;

namespace Items.Effectors
{
    public class KettleWaterBoiler : AEffector<Kettle>
    {
        [SerializeField] private TextMeshProUGUI temperatureIndicator;
        [SerializeField] private float boilingSpeed;

        protected override bool TryEffecting(Kettle kettle)
        {
            temperatureIndicator.text = kettle?.HasWater == true ? $"{(int)kettle.WaterTemperature}\u00b0C" : "";
            
            if (kettle == null)
                return false;
            
            if (!kettle.HasWater)
                return false;

            kettle.WaterTemperature += Time.deltaTime * boilingSpeed;
            kettle.WaterTemperature = Mathf.Clamp(kettle.WaterTemperature, 0f, 100f);
            return true;
        }
    }
}