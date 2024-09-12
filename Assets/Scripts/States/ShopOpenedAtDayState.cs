using System.Linq;
using Customers;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using Internal.Pooling;
using Internal.Utilities;
using Items.Holders;
using Player;
using Transitions;
using UnityEngine;

namespace States
{
    public class ShopOpenedAtDayState : AState
    {
        private DependencyRecipe<DependencyList<IManageableItemHolder>> _itemHolders = DependencyInjector.GetRecipe<DependencyList<IManageableItemHolder>>();
        private DependencyRecipe<DependencyList<IPoolItem>> _poolItems = DependencyInjector.GetRecipe<DependencyList<IPoolItem>>();
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private GameObjectProxy _gameObjectProxy = GameObjectProxy.Get<ShopOpenedAtDayState>();
        private CustomerSpawner _customerSpawner;
        private Coroutine _spawnCoroutine;

        public ShopOpenedAtDayState(CustomerSpawner customerSpawner) => _customerSpawner = customerSpawner;

        public override void OnEnter()
        {
            InitModification();
            InitItemHolders();
            InitCustomerQueue();
        }

        public override void OnExit()
        {
            DeinitItemHolders();
            DeinitCustomerQueue();
            DeinitPooledItems();
        }

        protected override void AddConditions() => AddCondition<ShopClosedState>(() =>
        {
            if (Transition.ShouldToggle(TransitionType.OpenCloseShop))
                return true;

            return HasFinishedLevel();
        });

        private void InitModification() => _playerModeToggle.Value.Toggle(PlayerMode.Modification);

        private void InitItemHolders()
        {
            foreach (IManageableItemHolder itemHolder in _itemHolders.Value)
                itemHolder.SetInitial();
        }

        private void InitCustomerQueue() => _spawnCoroutine = _gameObjectProxy.StartCoroutine(_customerSpawner.CSpawnCustomers());

        private void DeinitItemHolders()
        {
            foreach (IManageableItemHolder itemHolder in _itemHolders.Value)
                itemHolder.Destroy();
        }

        private void DeinitCustomerQueue()
        {
            if (_spawnCoroutine != null)
                _gameObjectProxy.StopCoroutine(_spawnCoroutine);
            
            _customerSpawner.Deinit();
        }

        private void DeinitPooledItems()
        {
            foreach (IPoolItem poolItem in _poolItems.Value.Where(poolItem => poolItem.Type == PoolItemType.Game))
                poolItem.TryReleasing();
        }

        private bool HasFinishedLevel() => _customerSpawner.IsDone;
    }
}