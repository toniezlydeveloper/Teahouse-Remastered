using System.Linq;
using Items.Implementations;
using UnityEngine;

namespace Items.Models
{
    public class KettleModel : AItemModel
    {
        [SerializeField] private Renderer waterRender;
        [SerializeField] private AddInsConfig config;

        private WaterType? _shownWaterType;
        
        public override void Refresh(object item)
        {
            WaterType waterType = item switch
            {
                Kettle kettle => kettle.HeldAddIns.Count > 0 ? (WaterType)kettle.HeldAddIns[0] : WaterType.None,
                _ => WaterType.None
            };

            if (_shownWaterType == waterType)
            {
                return;
            }
            
            waterRender.material.color = config.WaterColors.First(waterColor => waterColor.AddInType == waterType).ModelColor;
            _shownWaterType = waterType;
        }
    }
}