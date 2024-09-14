using System.Linq;
using Items.Implementations;
using UnityEngine;

namespace Items.Models
{
    public class HerbModel : AItemModel
    {
        [SerializeField] private Renderer herbRenderer;
        [SerializeField] private AddInsConfig config;
        
        public override void Refresh(object item)
        {
            if (item is not AddIn<HerbType> herb)
            {
                return;
            }

            herbRenderer.material.color = config.HerbColors.First(herbColor => herbColor.AddInType == herb.Type).Color;
        }
    }
}