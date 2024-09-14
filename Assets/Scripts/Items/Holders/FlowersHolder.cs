using Items.Implementations;
using UnityEngine;

namespace Items.Holders
{
    public class FlowersHolder : AddInItemHolder<FlowerType>
    {
        [SerializeField] private AddInsConfig config;
        [SerializeField] private Renderer[] models;
        
        protected override void Setup()
        {
            for (int i = 0; i < config.FlowerColors.Count; i++)
            {
                models[i].material.color = config.FlowerColors[i].Color;
            }
        }
    }
}