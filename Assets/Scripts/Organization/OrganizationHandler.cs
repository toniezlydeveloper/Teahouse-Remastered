using Bedroom;
using Interaction;
using Internal.Dependencies.Core;
using Internal.Pooling;
using Player;
using UnityEngine;

namespace Organization
{
    public interface IOrganizer : IDependency
    {
        bool HoldsItem { get; }
    }
    
    public class OrganizationHandler : AInteractionHandler, IOrganizer
    {
        [SerializeField] private GameObject organizationBox;
        
        private IPoolsProxy _poolsProxy = DependencyInjector.Get<IPoolsProxy>();
        private Quaternion _organizableRotation;
        private string _organizableName;

        public override PlayerMode HandledModes => PlayerMode.Organization;
        public override DayTime HandledDayTime => DayTime.Day;

        public bool HoldsItem => _organizableName != null;

        private void OnEnable() => DependencyInjector.InjectRecipe<IOrganizer>(this);

        private void OnDisable() => DependencyInjector.DejectRecipe<IOrganizer>();

        public override void HandleInteractionDownInput(InteractionElement element)
        {
            if (!TryGetInteractionComponent(element, out OrganizationPoint point))
                return;
            
            RotateLeft(point);
        }

        public override void HandleInteractionUpInput(InteractionElement element)
        {
            if (!TryGetInteractionComponent(element, out OrganizationPoint point))
                return;
            
            RotateRight(point);
        }

        public override void HandleInteractionInput(InteractionElement element)
        {
            if (!TryGetInteractionComponent(element, out OrganizationPoint point))
                return;
            
            if (TryPickingUp(point))
                return;
            
            TryPlacing(point);
        }

        private void RotateRight(OrganizationPoint point) => point.RotateRight();

        private void RotateLeft(OrganizationPoint point) => point.RotateLeft();

        private bool TryPickingUp(OrganizationPoint point)
        {
            if (!point.IsTaken)
                return false;

            if (_organizableName != null)
                return false;

            _organizableName = point.GetComponentInChildren<Organizable>().Name;
            _organizableRotation = point.GetComponentInChildren<Organizable>().Rotation;
            _poolsProxy.Release(_organizableName, point.GetComponentInChildren<Organizable>().GameObject);
            organizationBox.SetActive(true);
            return true;
        }

        private void TryPlacing(OrganizationPoint point)
        {
            if (point.IsTaken)
                return;

            if (_organizableName == null)
                return;
            
            Organizable organizable = _poolsProxy.GetTyped<Organizable>(_organizableName, Vector3.zero, Quaternion.identity, point.transform);
            organizable.transform.localPosition = _organizableRotation * Vector3.forward * organizable.ForwardOffset;
            organizable.transform.localRotation = _organizableRotation;
            organizationBox.SetActive(false);
            _organizableName = null;
        }
    }
}