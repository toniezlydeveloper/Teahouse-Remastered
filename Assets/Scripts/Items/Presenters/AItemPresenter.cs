using Internal.Dependencies.Core;
using Internal.Pooling;
using Items.Holders;
using Items.Implementations;
using Items.Models;
using UnityEngine;

namespace Items.Presenters
{
    public abstract class AItemPresenter : MonoBehaviour
    {
        [SerializeField] private Transform point;
        
        private IPoolsProxy _poolsProxy = DependencyInjector.Get<IPoolsProxy>();
        private string _itemTypeName;
        private GameObject _model;
        private bool _hasItem;

        protected void AddCallback(IItemHolder itemHolder) => itemHolder.OnChanged += ModifyModel;

        protected void RemoveCallback(IItemHolder itemHolder) => itemHolder.OnChanged -= ModifyModel;

        private void ModifyModel(IItem item)
        {
            if (TryGetTypeName(item, out string itemTypeName))
                ChangeModel(itemTypeName);
            
            TryRefreshingModel(item);
        }

        private void ChangeModel(string itemTypeName)
        {
            TryReleasingModel();
            TryGettingModel(itemTypeName);
        }
        
        private bool TryGetTypeName(IItem item, out string itemTypeName)
        {
            itemTypeName = item != null ? $"P_{item.Name}" : null;
            _hasItem = item != null;
            return _itemTypeName != itemTypeName;
        }
        
        private void TryReleasingModel()
        {
            if (_itemTypeName == null)
                return;
            
            _poolsProxy.Release(_itemTypeName, _model);
            _itemTypeName = null;
            _model = null;
        }

        private void TryGettingModel(string itemTypeName)
        {
            if (itemTypeName == null)
                return;
            
            _model = _poolsProxy.Get(itemTypeName, point.position, point.rotation);
            _model.transform.SetParent(point);
            _itemTypeName = itemTypeName;
        }

        private void TryRefreshingModel(object item)
        {
            if (!_hasItem)
                return;
            
            if (!_model.TryGetComponent(out AItemModel model))
                return;
            
            model.Refresh(item);
        }
    }
}