using Internal.Dependencies.Core;
using UnityEngine;

namespace Customers
{
    public interface IDoorHinge : IDependency
    {
        void Toggle(bool state);
    }
    
    public class DoorHinge : ADependency<IDoorHinge>, IDoorHinge
    {
        [SerializeField] private float openedRotation;
        [SerializeField] private float closedRotation;
        [SerializeField] private Transform hinge;

        public void Toggle(bool state) => hinge.localRotation = Quaternion.Euler(0f, state ? openedRotation : closedRotation, 0f);
    }
}