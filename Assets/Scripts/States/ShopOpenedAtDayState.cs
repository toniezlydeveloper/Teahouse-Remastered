using System.Linq;
using Customers;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using Internal.Pooling;
using Internal.Utilities;
using Items.Holders;
using Player;
using UI.Shared;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

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

        public ShopOpenedAtDayState(CustomerSpawner customerSpawner)
        {
            _customerSpawner = customerSpawner;
        }

        public override void OnEnter()
        {
            InitModification();
            InitItemHolders();

            if (DevelopmentConfig.Instance.ShouldSkipCustomers)
            {
                return;
            }
            
            InitCustomerQueue();
        }

        public override void OnExit()
        {
            DeinitItemHolders();
            DeinitPooledItems();

            if (DevelopmentConfig.Instance.ShouldSkipCustomers)
            {
                return;
            }
            
            DeinitCustomerQueue();
        }

        protected override void AddConditions() => AddCondition<ShopClosedAtNightState>(HasFinishedLevel);

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

        private bool HasFinishedLevel() => _customerSpawner.IsDone || DevelopmentConfig.Instance.ShouldSkipCustomers;
    }
}