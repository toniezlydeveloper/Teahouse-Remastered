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
    public class BedroomDayState : APauseAllowedState
    {
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private DayTimeProxy _dayTime;

        protected override List<FileSaveType> TypesToSave => new List<FileSaveType>
        {
            FileSaveType.Inventory,
            FileSaveType.Bedroom
        };
        
        public BedroomDayState(InputActionReference pauseInput, IPausePanel pausePanel) : base(pauseInput, pausePanel)
        {
        }

        public override void OnEnter()
        {
            SavingController.Load(PersistenceType.Persistent, FileSaveType.Character);
            InitModification();
        }

        public override Type OnUpdate()
        {
            HandlePause();
            return null;
        }

        protected override void AddConditions()
        {
            AddCondition<ShopDayBootstrapState>(() =>
            {
                if (!Transition.ShouldToggle(TransitionType.Shop))
                {
                    return false;
                }

                SavingController.Save(PersistenceType.Volatile, FileSaveType.Bedroom);
                return true;
            });
        }
        
        private void InitModification() => _playerModeToggle.Value.Toggle(PlayerMode.Modification);
    }
}