using Items.Implementations;
using UnityEngine;

namespace Items.Models
{
    public class KettleModel : AItemModel
    {
        [SerializeField] private Renderer waterRender;
        
        public override void Refresh(object item)
        {
            if (item is not Kettle kettle)
                return;

            waterRender.material.color = kettle.HasWater
                ? Color.LerpUnclamped(Color.cyan, Color.blue, kettle.WaterTemperature / 100f)
                : Color.gray;
        }
    }
}