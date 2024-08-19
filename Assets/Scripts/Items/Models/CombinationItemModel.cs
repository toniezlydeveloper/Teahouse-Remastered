using Items.Implementations;
using UnityEngine;

namespace Items.Models
{
    public class CombinationItemModel : AItemModel
    {
        [SerializeField] private TeabagModel model1;
        [SerializeField] private TeabagModel model2;
        
        public override void Refresh(object item)
        {
            if (item is not CombinationItem combinationItem)
                return;
            
            model1.gameObject.SetActive(combinationItem.Item1 != null);
            model2.gameObject.SetActive(combinationItem.Item2 != null);
            model1.Refresh(combinationItem.Item1);
            model2.Refresh(combinationItem.Item2);
        }
    }
}