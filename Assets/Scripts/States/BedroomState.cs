using System;
using System.Collections.Generic;
using Bedroom;
using Internal.Dependencies.Core;
using Player;
using Saving;
using Transitions;
using UI.Shared;
using UnityEngine.InputSystem;

namespace States
{
    public interface IFurnishingCore : IDependency
    {
        bool IsEnabled { get; }
    }
    
    public class BedroomState : APauseAllowedState, IFurnishingCore
    {
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private InputActionReference _toggleInput;
        private PlayerModeProxy _playerMode;
        private DayTimeProxy _dayTime;
        private bool _isEnabled;

        public bool IsEnabled => _isEnabled;

        protected override List<FileSaveType> TypesToSave => new List<FileSaveType>
        {
            FileSaveType.Bedroom
        };

        public BedroomState(InputActionReference toggleInput, PlayerModeProxy playerMode, DayTimeProxy dayTime, InputActionReference pauseInput, IPausePanel pausePanel) : base(pauseInput, pausePanel)
        {
            _toggleInput = toggleInput;
            _playerMode = playerMode;
            _dayTime = dayTime;
        }

        public override void OnEnter()
        {
            SavingController.Load(PersistenceType.Persistent, FileSaveType.Character);
            InitModification();
            AddCallbacks();
        }

        public override void OnExit()
        {
            RemoveCallbacks();
            Disable();
        }

        public override Type OnUpdate()
        {
            // todo: uncomment
            // if (Is(DayTime.Night))
            {
                HandleModeToggling();
            }
            
            HandlePause();
            return null;
        }

        protected override void AddConditions()
        {
            AddCondition<ShopDayBootstrapState>(() =>
            {
                if (!Is(DayTime.Day))
                {
                    return false;
                }
                
                if (!Transition.ShouldToggle(TransitionType.Shop))
                {
                    return false;
                }

                SavingController.Save(PersistenceType.Volatile, FileSaveType.Bedroom);
                return true;
            });
            AddCondition<ShopNightBootstrapState>(() =>
            {
                if (!Is(DayTime.Night))
                {
                    return false;
                }
                
                if (!Transition.ShouldToggle(TransitionType.Shop))
                {
                    return false;
                }

                SavingController.Save(PersistenceType.Volatile, FileSaveType.Bedroom);
                return true;
            });
        }

        private void DisableOrganization(DayTime _) => InitModification();

        private bool Is(DayTime dayTime) => _dayTime.Value == dayTime;

        private void InitModification() => _playerModeToggle.Value.Toggle(PlayerMode.Modification);

        private void HandleModeToggling()
        {
            if (!_toggleInput.action.triggered)
            {
                return;
            }
            
            _playerModeToggle.Value.Toggle(_playerMode.Value == PlayerMode.Modification ? PlayerMode.Organization : PlayerMode.Modification);
            _isEnabled = _playerMode.Value == PlayerMode.Organization;
        }

        private void Disable() => _isEnabled = false;

        private void AddCallbacks() => _dayTime.OnChanged += DisableOrganization;

        private void RemoveCallbacks() => _dayTime.OnChanged -= DisableOrganization;
    }
}