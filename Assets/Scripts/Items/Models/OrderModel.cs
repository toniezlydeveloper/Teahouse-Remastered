using Items.Implementations;
using UnityEngine;

namespace Items.Models
{
    public class OrderModel : AItemModel
    {
        [SerializeField] private GameObject waitingForTakingModel;
        [SerializeField] private GameObject completeModel;
        [SerializeField] private GameObject waitingModel;
        
        public override void Refresh(object item)
        {
            if (item is not Order order)
                return;
            
            waitingForTakingModel.SetActive(!order.WasTaken);
            completeModel.SetActive(order.WasCompleted);
            waitingModel.SetActive(order.WasTaken && !order.WasCompleted);
        }
    }
}