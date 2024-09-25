using System;
using System.Collections.Generic;
using Bedroom;
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
    public class ShopClosedAtNightState : APauseAllowedState
    {
        private DependencyRecipe<DependencyList<IPoolItem>> _poolItems = DependencyInjector.GetRecipe<DependencyList<IPoolItem>>();
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private DependencyRecipe<IDoorHinge> _doorHinge = DependencyInjector.GetRecipe<IDoorHinge>();

        private DayTimeProxy _timeProxy;

        protected override List<FileSaveType> TypesToSave => new List<FileSaveType>
        {
            FileSaveType.Inventory
        };
        
        public ShopClosedAtNightState(DayTimeProxy timeProxy, InputActionReference pauseInput, IPausePanel pausePanel) : base(pauseInput, pausePanel)
        {
            _timeProxy = timeProxy;
        }
        
        public override void OnEnter()
        {
            SavingController.Load(PersistenceType.Persistent, FileSaveType.Character);
            InitOrganization();
            ToggleDoorHinge(false);
            ChangeToNight();
        }

        public override Type OnUpdate()
        {
            HandlePause();
            return null;
        }

        protected override void AddConditions()
        {
            AddCondition<BedroomNightBoostrapState>(() =>
            {
                if (!Transition.ShouldToggle(TransitionType.Bedroom))
                    return false;
                
                SavingController.Save(PersistenceType.Volatile, FileSaveType.Shop);
                ReleasePools();
                return true;
            });
        }

        private void InitOrganization() => _playerModeToggle.Value.Toggle(PlayerMode.None);

        private void ToggleDoorHinge(bool state) => _doorHinge.Value?.Toggle(state);

        private void ChangeToNight() => _timeProxy.Value = DayTime.Night;

        private void ReleasePools()
        {
            foreach (IPoolItem poolItem in _poolItems.Value)
                poolItem.TryReleasing();
        }
    }
}