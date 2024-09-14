using System.Linq;
using Items.Implementations;
using UnityEngine;

namespace Items.Models
{
    public class FlowerModel : AItemModel
    {
        [SerializeField] private AddInsConfig config;
        [SerializeField] private Renderer[] petals;
        
        public override void Refresh(object item)
        {
            if (item is not AddIn<FlowerType> herb)
            {
                return;
            }
            
            Color color = config.FlowerColors.First(herbColor => herbColor.AddInType == herb.Type).Color;

            foreach (Renderer petal in petals)
            {
                petal.material.color = color;
            }
        }
    }
}