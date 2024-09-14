using System.Linq;
using Items.Implementations;
using UnityEngine;

namespace Items.Effectors
{
    public class HerbTypeTracker : AAddInTypeTracker<HerbType>
    {
        [SerializeField] private AddInsConfig config;
        [SerializeField] private Renderer herb;
        
        protected override void ChangeType(HerbType type)
        {
            herb.material.color = config.HerbColors.First(herbColor => herbColor.AddInType == type).Color;
        }
    }
}