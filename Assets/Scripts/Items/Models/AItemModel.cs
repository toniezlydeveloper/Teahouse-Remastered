using UnityEngine;

namespace Items.Models
{
    // todo: remake once final models are there
    public abstract class AItemModel : MonoBehaviour
    {
        public abstract void Refresh(object item);
    }
}