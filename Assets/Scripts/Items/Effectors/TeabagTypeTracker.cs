using System.Linq;
using Items.Implementations;
using UnityEngine;

namespace Items.Effectors
{
    public class TeabagTypeTracker : AAddInTypeTracker<TeabagType>
    {
        [SerializeField] private AddInsConfig config;
        [SerializeField] private Renderer model;
        
        protected override void ChangeType(TeabagType type)
        {
            model.material.color = config.TeabagColors.First(teabagColor => teabagColor.AddInType == type).Color;
        }
    }
}