using System.Linq;
using Items.Holders;
using Items.Implementations;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Effectors
{
    public class KettleWaterBoiler : AEffector<Kettle>
    {
        [SerializeField] private Image temperatureImage;
        [SerializeField] private AddInsConfig config;
        [SerializeField] private float boilingSpeed;

        private WaterType? _latestWaterType;
        
        private const float MaxTemperature = 100f;
        
        protected override bool TryEffecting(Kettle kettle)
        {
            temperatureImage.fillAmount = 0f;
            
            if (kettle == null)
            {
                return false;
            }

            if (!kettle.Contains<WaterType>())
            {
                return false;
            }

            bool hasChanged = _latestWaterType != (WaterType)kettle.HeldAddIns[0];
            kettle.WaterTemperature = Mathf.Clamp(kettle.WaterTemperature + Time.deltaTime * boilingSpeed, 0f, MaxTemperature);
            kettle.HeldAddIns[0] = kettle.WaterTemperature switch
            {
                > 75f => WaterType.Hot,
                > 50f => WaterType.Medium,
                _ => WaterType.Low
            };

            temperatureImage.fillAmount = kettle.WaterTemperature / MaxTemperature;
            temperatureImage.color = config.WaterColors.First(waterColor => waterColor.AddInType == (WaterType)kettle.HeldAddIns[0]).Color;
            
            return hasChanged;
        }
    }
}