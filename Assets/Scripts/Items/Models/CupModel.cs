using System.Linq;
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

            waterIndicator.SetActive(cup.HeldAddIns.Any(addIn => addIn.GetType() == typeof(WaterType)));
            dirtyIndicator.SetActive(cup.IsDirty);
        }
    }
}