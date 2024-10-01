using System;
using System.Collections.Generic;
using System.Linq;
using Customers;
using Internal.Dependencies.Core;
using Internal.Pooling;
using Internal.Utilities;
using Items.Holders;
using Items.Implementations;
using Player;
using Saving;
using UI.Core;
using UI.Shared;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace States
{
    public class ShopOpenedState : APauseAllowedState
    {
        private DependencyRecipe<DependencyList<IManageableItemHolder>> _itemHolders = DependencyInjector.GetRecipe<DependencyList<IManageableItemHolder>>();
        private DependencyRecipe<DependencyList<IPoolItem>> _poolItems = DependencyInjector.GetRecipe<DependencyList<IPoolItem>>();
        private DependencyRecipe<DependencyList<ICustomer>> _customers = DependencyInjector.GetRecipe<DependencyList<ICustomer>>();
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private GameObjectProxy _gameObjectProxy = GameObjectProxy.Get<ShopOpenedState>();
        private InputActionReference[] _inputsToDisable;
        private InputActionReference _toggleInput;
        private CustomerSpawner _customerSpawner;
        private Coroutine _spawnCoroutine;
        private INotesPanel _notesPanel;
        private bool _isInNotesMode;

        protected override List<FileSaveType> TypesToSave => new();

        public ShopOpenedState(CustomerSpawner customerSpawner, InputActionReference[] inputsToDisable, InputActionReference toggleInput, INotesPanel notesPanel, InputActionReference pauseInput, IPausePanel pausePanel) : base(pauseInput, pausePanel)
        {
            _customerSpawner = customerSpawner;
            _inputsToDisable = inputsToDisable;
            _toggleInput = toggleInput;
            _notesPanel = notesPanel;
        }

        public override void OnEnter()
        {
            InitModification();
            InitItemHolders();
            RefreshNotes();

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
            if (ReceivedToggleInput())
            {
                ToggleNotesMode();
                RefreshNotes();
                ToggleInputs();
                return null;
            }
            
            HandlePause();
            return null;
        }

        protected override void AddConditions() => AddCondition<ShopClosedAtNightState>(HasFinishedLevel);

        private void InitModification() => _playerModeToggle.Value.Toggle(PlayerMode.Modification);

        private void InitItemHolders()
        {
            foreach (IManageableItemHolder itemHolder in _itemHolders.Value)
            {
                itemHolder.SetInitial();
            }
        }

        private void InitCustomerQueue() => _spawnCoroutine = _gameObjectProxy.StartCoroutine(_customerSpawner.CSpawnCustomers());

        private void DeinitItemHolders()
        {
            foreach (IManageableItemHolder itemHolder in _itemHolders.Value)
            {
                itemHolder.Destroy();
            }
        }

        private void DeinitCustomerQueue()
        {
            if (_spawnCoroutine != null)
            {
                _gameObjectProxy.StopCoroutine(_spawnCoroutine);
            }
            
            _customerSpawner.Deinit();
        }

        private void DeinitPooledItems()
        {
            foreach (IPoolItem poolItem in _poolItems.Value.Where(poolItem => poolItem.Type == PoolItemType.Game))
            {
                poolItem.TryReleasing();
            }
        }

        private bool HasFinishedLevel() => _customerSpawner.IsDone || DevelopmentConfig.Instance.ShouldSkipCustomers;

        private bool ReceivedToggleInput()
        {
            if (_toggleInput.action.WasReleasedThisFrame())
            {
                return true;
            }

            return _toggleInput.action.WasPressedThisFrame();
        }

        private void ToggleNotesMode() => _isInNotesMode = !_isInNotesMode;

        private void RefreshNotes()
        {
            if (_isInNotesMode)
            {
                OrderData[] ordersData = _customers.Value.Select(customer => new OrderData
                {
                    AddIns = ((Order)customer.GameObject?.GetComponentInChildren<IItemHolder>().Value)?.HeldAddIns ?? new List<Enum>(),
                    Species = customer.Species
                }).ToArray();
                    
                _notesPanel.Init(ordersData);
            }

            _notesPanel.Toggle(_isInNotesMode);
        }

        private void ToggleInputs()
        {
            foreach (InputActionReference input in _inputsToDisable)
            {
                if (_isInNotesMode)
                {
                    input.action.Disable();
                }
                else
                {
                    input.action.Enable();
                }
            }
        }
    }
}