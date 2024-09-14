using System.Linq;
using Items.Implementations;
using UnityEngine;

namespace Items.Effectors
{
    public class FlowerTypeTracker : AAddInTypeTracker<FlowerType>
    {
        [SerializeField] private AddInsConfig config;
        [SerializeField] private Renderer[] petals;
        
        protected override void ChangeType(FlowerType type)
        {
            Color color = config.FlowerColors.First(herbColor => herbColor.AddInType == type).Color;

            foreach (Renderer petal in petals)
            {
                petal.material.color = color;
            }
        }
    }
}