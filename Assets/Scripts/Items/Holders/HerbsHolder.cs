using Items.Implementations;
using UnityEngine;

namespace Items.Holders
{
    public class HerbsHolder : AddInItemHolder<HerbType>
    {
        [SerializeField] private AddInsConfig config;
        [SerializeField] private Renderer[] models;
        
        protected override void SetupVisuals()
        {
            for (int i = 0; i < config.HerbColors.Count; i++)
            {
                models[i].material.color = config.HerbColors[i].Color;
            }
        }
    }
}