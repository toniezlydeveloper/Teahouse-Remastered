using System.Linq;
using Items.Implementations;
using UnityEngine;

namespace Items.Models
{
    public class KettleModel : AItemModel
    {
        [SerializeField] private Renderer waterRender;
        [SerializeField] private AddInsConfig config;
        
        public override void Refresh(object item)
        {
            if (item is not AddIn<WaterType> water)
            {
                return;
            }
            
            waterRender.material.color = config.WaterColors.First(waterColor => waterColor.AddInType == water.Type).ModelColor;
        }
    }
}