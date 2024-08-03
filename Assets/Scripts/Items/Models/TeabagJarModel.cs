using Items.Implementations;
using UnityEngine;

namespace Items.Models
{
    public class TeabagJarModel : AItemModel
    {
        [SerializeField] private Renderer ren;
        
        public override void Refresh(object item)
        {
            if (item is not TeabagJar jar)
                return;
            
            ren.material.color = jar.TeabagType == TeabagType.Default
                ? Color.magenta
                : Color.red;
        }
    }
}