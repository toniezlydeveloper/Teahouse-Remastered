using System;
using System.Collections.Generic;
using Customers;
using Internal.Dependencies.Core;
using Internal.Pooling;
using Player;
using Saving;
using Transitions;
using UI.Shared;
using UnityEngine.InputSystem;

namespace States
{
    public class ShopClosedAtDayState : APauseAllowedState
    {
        private DependencyRecipe<DependencyList<IPoolItem>> _poolItems = DependencyInjector.GetRecipe<DependencyList<IPoolItem>>();
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private DependencyRecipe<IDoorHinge> _doorHinge = DependencyInjector.GetRecipe<IDoorHinge>();

        protected override List<FileSaveType> TypesToSave => new List<FileSaveType>
        {
            FileSaveType.Inventory,
            FileSaveType.Shop
        };

        public ShopClosedAtDayState(InputActionReference pauseInput, IPausePanel pausePanel) : base(pauseInput, pausePanel)
        {
        }
        
        public override void OnEnter()
        {
            SavingController.Load(PersistenceType.Persistent, FileSaveType.Character);
            InitOrganization();
            ToggleDoorHinge(false);
        }

        public override Type OnUpdate()
        {
            HandlePause();
            return null;
        }

        protected override void AddConditions()
        {
            AddCondition<CallingState>(() => Transition.ShouldToggle(TransitionType.Calling));
            AddCondition<ShopOpenedState>(() =>
            {
                if (!Transition.ShouldToggle(TransitionType.OpenShop))
                    return false;
                
                ToggleDoorHinge(true);
                return true;
            });
            AddCondition<BedroomDayBoostrapState>(() =>
            {
                if (!Transition.ShouldToggle(TransitionType.Bedroom))
                    return false;
                
                SavingController.Save(PersistenceType.Volatile, FileSaveType.Shop);
                ReleasePools();
                return true;
            });
        }

        private void InitOrganization() => _playerModeToggle.Value.Toggle(PlayerMode.Organization);

        private void ToggleDoorHinge(bool state) => _doorHinge.Value?.Toggle(state);

        private void ReleasePools()
        {
            foreach (IPoolItem poolItem in _poolItems.Value)
                poolItem.TryReleasing();
        }
    }
}