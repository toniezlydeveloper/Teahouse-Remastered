using Internal.Dependencies.Core;
using UnityEngine;

namespace Internal.Dependencies.Collections
{
    public abstract class ACollectionItemProxy<TItem> : MonoBehaviour where TItem : Component
    {
        [SerializeField] private TItem item;
        
        private ICollectionHolder<TItem> _collectionHolder = DependencyInjector.Get<ICollectionHolder<TItem>>();

        private void OnEnable() => _collectionHolder.Add(item);
        
        private void OnDisable() => _collectionHolder.Remove(item);

        private void OnValidate()
        {
            if (item != null)
            {
                return;
            }

            item = GetComponent<TItem>();
        }
    }
}