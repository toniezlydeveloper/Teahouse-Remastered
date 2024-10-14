using System;
using System.Collections.Generic;
using Bedroom;
using Internal.Dependencies.Core;
using Player;
using Saving;
using Transitions;
using UI.Shared;
using UnityEngine.InputSystem;
using Utilities;

namespace States
{
    public class BedroomNightState : APauseAllowedState
    {
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private DayTimeProxy _dayTime;

        protected override List<FileSaveType> TypesToSave => new List<FileSaveType>
        {
            FileSaveType.Inventory,
            FileSaveType.Bedroom
        };

        public BedroomNightState(DayTimeProxy dayTime, InputActionReference pauseInput, IPausePanel pausePanel) : base(pauseInput, pausePanel)
        {
            _dayTime = dayTime;
        }

        public override void OnEnter()
        {
            SavingController.Load(PersistenceType.Persistent, FileSaveType.Character);
            AddCallbacks();
        }

        public override void OnExit()
        {
            RemoveCallbacks();
        }

        public override Type OnUpdate()
        {
            HandlePause();
            return null;
        }

        protected override void AddConditions()
        {
            AddCondition<ShopNightBootstrapState>(() =>
            {
                if (!Transition.ShouldToggle(TransitionType.Shop))
                {
                    return false;
                }

                SavingController.Save(PersistenceType.Volatile, FileSaveType.Bedroom);
                return true;
            });
            AddCondition<BedroomDayState>(() =>
            {
                if (DevelopmentConfig.Instance.ShouldStartInBuildMode)
                {
                    return false;
                }
                
                return Is(DayTime.Day);
            });
        }

        private void DisableOrganization(DayTime _) => InitModification();

        private bool Is(DayTime dayTime) => _dayTime.Value == dayTime;

        private void InitModification() => _playerModeToggle.Value.Toggle(PlayerMode.Modification);

        private void AddCallbacks() => _dayTime.OnChanged += DisableOrganization;

        private void RemoveCallbacks() => _dayTime.OnChanged -= DisableOrganization;
    }
}