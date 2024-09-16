using Internal.Dependencies.Core;
using Items.Holders;
using UnityEngine;

namespace UI.Items
{
    public interface IItemPanel : IDependency
    {
        void Present(IItemHolder itemHolder);
    }
    
    public abstract class AItemPanel<TItem> : MonoBehaviour, IItemPanel
    {
        private IItemHolder _itemHolder;

        public virtual void Present(IItemHolder itemHolder) => _itemHolder = itemHolder;

        protected void TryGetItem(out TItem item)
        {
            item = default;

            if (_itemHolder?.Value is not TItem value)
                return;

            item = value;
        }
    }
}