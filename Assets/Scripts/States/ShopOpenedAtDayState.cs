using System;
using System.Collections.Generic;
using System.Linq;
using Customers;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using Internal.Pooling;
using Internal.Utilities;
using Items.Holders;
using Player;
using Saving;
using UI.Shared;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace States
{
    public class ShopOpenedAtDayState : APauseAllowedState
    {
        private DependencyRecipe<DependencyList<IManageableItemHolder>> _itemHolders = DependencyInjector.GetRecipe<DependencyList<IManageableItemHolder>>();
        private DependencyRecipe<DependencyList<IPoolItem>> _poolItems = DependencyInjector.GetRecipe<DependencyList<IPoolItem>>();
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private GameObjectProxy _gameObjectProxy = GameObjectProxy.Get<ShopOpenedAtDayState>();
        private CustomerSpawner _customerSpawner;
        private Coroutine _spawnCoroutine;

        protected override List<FileSaveType> TypesToSave => new();

        public ShopOpenedAtDayState(CustomerSpawner customerSpawner, InputActionReference pauseInput, IPausePanel pausePanel) : base(pauseInput, pausePanel)
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

        public override Type OnUpdate()
        {
            HandlePause();
            return null;
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