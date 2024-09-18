using System;
using Items.Implementations;
using UnityEngine;

namespace Items.Holders
{
    public class FlowersHolder : AAddInItemHolder<FlowerType>
    {
        [Serializable]
        public class Petals
        {
            [field: SerializeField] public Renderer[] Value { get; set; }
        }
        
        [SerializeField] private AddInsConfig config;
        [SerializeField] private Petals[] petals;
        
        protected override void SetupVisuals()
        {
            for (int i = 0; i < config.FlowerColors.Count; i++)
            {
                foreach (Renderer petal in petals[i].Value)
                {
                    petal.material.color = config.FlowerColors[i].Color;
                }
            }
        }
    }
}