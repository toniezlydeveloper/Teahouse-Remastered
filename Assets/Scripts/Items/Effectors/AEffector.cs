using Items.Holders;
using UnityEngine;

namespace Items.Effectors
{
    public abstract class AEffector<TItem> : MonoBehaviour
    {
        private IItemHolder _itemHolder;
        private TItem _item;

        private void Awake() => _itemHolder = GetComponent<IItemHolder>();

        private void OnEnable() => _itemHolder.OnChanged += Cache;

        private void OnDisable() => _itemHolder.OnChanged -= Cache;

        private void Update()
        {
            if (!TryEffecting(_item))
                return;
            
            Refresh();
        }

        protected abstract bool TryEffecting(TItem item);

        protected virtual void Cache(object item)
        {
            if (item is TItem typedItem)
                _item = typedItem;
            else
                _item = default;
        }

        private void Refresh() => _itemHolder.Refresh();
    }
}