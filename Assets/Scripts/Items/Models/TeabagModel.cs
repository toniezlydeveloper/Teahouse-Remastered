using Items.Implementations;
using UnityEngine;

namespace Items.Models
{
    public class TeabagModel : AItemModel
    {
        [SerializeField] private Renderer teabagRenderer;
        
        public override void Refresh(object item)
        {
            if (item is not Teabag teabag)
                return;
            
            teabagRenderer.material.color = teabag.TeabagType == TeabagType.Default
                ? Color.magenta
                : Color.red;
        }
    }
}