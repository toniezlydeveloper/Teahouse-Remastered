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
    public interface IFurnishingListener : IDependency
    {
        void Toggle(bool state);
    }
    
    public interface IFurnishingCore : IDependency
    {
        bool IsEnabled { get; }
    }
    
    public class BedroomState : APauseAllowedState, IFurnishingCore
    {
        private DependencyRecipe<DependencyList<IFurnishingListener>> _furnishingListeners = DependencyInjector.GetRecipe<DependencyList<IFurnishingListener>>();
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private PlayerModeProxy _playerMode;
        private DayTimeProxy _dayTime;
        private bool _isEnabled;

        public bool IsEnabled => _isEnabled;

        protected override List<FileSaveType> TypesToSave => new List<FileSaveType>
        {
            FileSaveType.Bedroom
        };

        public BedroomState(PlayerModeProxy playerMode, DayTimeProxy dayTime, InputActionReference pauseInput, IPausePanel pausePanel) : base(pauseInput, pausePanel)
        {
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
            Toggle();
        }

        public override Type OnUpdate()
        {
            // todo: uncomment
            // if (Is(DayTime.Night))
            {
                Toggle();
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

        private void Toggle()
        {
            bool state = _playerMode.Value == PlayerMode.Organization;
            
            foreach (IFurnishingListener listener in _furnishingListeners.Value)
            {
                listener.Toggle(state);
            }

            _isEnabled = state;
        }

        private void AddCallbacks() => _dayTime.OnChanged += DisableOrganization;

        private void RemoveCallbacks() => _dayTime.OnChanged -= DisableOrganization;
    }
}