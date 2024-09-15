using Items.Implementations;
using UnityEngine;

namespace Items.Models
{
    public class CupModel : AItemModel
    {
        [SerializeField] private GameObject waterIndicator;
        [SerializeField] private GameObject dirtyIndicator;
        
        public override void Refresh(object item)
        {
            if (item is not Cup cup)
                return;

            waterIndicator.GetComponent<Renderer>().material.color = Color.LerpUnclamped(Color.cyan, Color.blue, cup.WaterTemperature / 100f);
            waterIndicator.SetActive(cup.HasWater);
            dirtyIndicator.SetActive(cup.IsDirty);
        }
    }
}