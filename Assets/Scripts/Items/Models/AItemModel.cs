using UnityEngine;

namespace Items.Models
{
    public abstract class AItemModel : MonoBehaviour
    {
        public abstract void Refresh(object item);
    }
}