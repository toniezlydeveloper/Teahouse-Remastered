using System.Linq;
using Items.Implementations;
using UnityEngine;

namespace Items.Models
{
    public class TeabagModel : AItemModel
    {
        [SerializeField] private Renderer teabagRenderer;
        [SerializeField] private AddInsConfig config;
        
        public override void Refresh(object item)
        {
            if (item is not AddIn<TeabagType> teabag)
            {
                return;
            }
            
            teabagRenderer.material.color = config.TeabagColors.First(teabagColor => teabagColor.AddInType == teabag.Type).Color;
        }
    }
}