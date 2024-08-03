using Items.Implementations;
using UnityEngine;

namespace Items.Models
{
    public class TeabagModel : AItemModel
    {
        public override void Refresh(object item)
        {
            if (item is not Teabag jar)
                return;
            
            GetComponentInChildren<Renderer>().material.color = jar.TeabagType == TeabagType.Default
                ? Color.magenta
                : Color.red;
        }
    }
}