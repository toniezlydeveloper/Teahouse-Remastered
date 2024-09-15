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
            if (item is not AddIn<FlowerType> flower)
            {
                return;
            }
            
            Color color = config.FlowerColors.First(flowerColor => flowerColor.AddInType == flower.Type).Color;

            foreach (Renderer petal in petals)
            {
                petal.material.color = color;
            }
        }
    }
}