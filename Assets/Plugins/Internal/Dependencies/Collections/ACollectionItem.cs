using Internal.Dependencies.Core;
using UnityEngine;

namespace Internal.Dependencies.Collections
{
    public abstract class ACollectionItem<TItem> : MonoBehaviour
    {
        [SerializeField] [HideInInspector] private TItem item;
        
        private ICollectionHolder<TItem> _collectionHolder = DependencyInjector.Get<ICollectionHolder<TItem>>();

        protected virtual void OnEnable() => _collectionHolder.Add(item);
        
        protected virtual void OnDisable() => _collectionHolder.Remove(item);

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