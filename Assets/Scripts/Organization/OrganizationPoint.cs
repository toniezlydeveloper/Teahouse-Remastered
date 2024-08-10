using Internal.Dependencies.Core;
using Internal.Pooling;
using UnityEngine;

namespace Organization
{
    public interface IOrganizationPoint : IDependency
    {
        void Populate();
    }
    
    public class OrganizationPoint : ADependencyElement<IOrganizationPoint>, IOrganizationPoint
    {
        [SerializeField] private float originalRotation;
        [SerializeField] private GameObject prefab;
        
        private IPoolsProxy _poolsProxy = DependencyInjector.Get<IPoolsProxy>();

        public bool IsTaken => TryGetOrganizable(out Organizable _);

        public void Populate()
        {
            if (TryGetOrganizable(out Organizable organizable))
                return;
            
            TrySpawningOrganizable();
            
            if (!TryGetOrganizable(out organizable))
                return;
            
            ApplyOffset(organizable);
        }

        public void RotateLeft()
        {
            if (!TryGetOrganizable(out Organizable organizable))
                return;
            
            RotateLeft(organizable);
            ApplyOffset(organizable);
        }

        public void RotateRight()
        {
            if (!TryGetOrganizable(out Organizable organizable))
                return;
            
            RotateRight(organizable);
            ApplyOffset(organizable);
        }

        private void RotateLeft(Organizable organizable) => organizable.transform.Rotate(0f, -90f, 0f);
        
        private void RotateRight(Organizable organizable) => organizable.transform.Rotate(0f, 90f, 0f);

        private static void ApplyOffset(Organizable organizable) => organizable.transform.localPosition = organizable.transform.forward * organizable.ForwardOffset;

        private void TrySpawningOrganizable()
        {
            if (!prefab)
                return;

            _poolsProxy.Get(prefab.name, transform.position, Quaternion.Euler(0f, originalRotation, 0f), transform);
        }
        
        private bool TryGetOrganizable(out Organizable organizable)
        {
            organizable = GetComponentInChildren<Organizable>();
            return organizable != null;
        }
    }
}