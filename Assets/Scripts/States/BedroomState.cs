using System;
using System.Collections.Generic;
using Internal.Dependencies.Core;
using Player;
using Saving;
using Transitions;
using UI.Shared;
using UnityEngine.InputSystem;

namespace States
{
    public class BedroomState : APauseAllowedState
    {
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private InputActionReference _toggleInput;
        private IFurnishingPanel _furnishingPanel;
        private PlayerModeProxy _playerMode;

        protected override List<FileSaveType> TypesToSave => new List<FileSaveType>
        {
            FileSaveType.Inventory,
            FileSaveType.Bedroom
        };

        public BedroomState(InputActionReference toggleInput, PlayerModeProxy playerMode, IFurnishingPanel furnishingPanel, InputActionReference pauseInput, IPausePanel pausePanel) : base(pauseInput, pausePanel)
        {
            _furnishingPanel = furnishingPanel;
            _toggleInput = toggleInput;
            _playerMode = playerMode;
        }

        public override void OnEnter()
        {
            SavingController.Load(PersistenceType.Persistent, FileSaveType.Character);
            InitModification();
        }

        public override Type OnUpdate()
        {
            HandleModeToggling();
            HandlePause();
            return null;
        }

        protected override void AddConditions()
        {
            AddCondition<ItemShopState>(() => Transition.ShouldToggle(TransitionType.ItemShop));
            AddCondition<ShopBootstrapState>(() =>
            {
                if (!Transition.ShouldToggle(TransitionType.Shop))
                    return false;

                SavingController.Save(PersistenceType.Volatile, FileSaveType.Bedroom);
                return true;
            });
        }

        private void InitModification()
        {
            _playerModeToggle.Value.Toggle(PlayerMode.Modification);
            _furnishingPanel.Present(false);
        }

        private void HandleModeToggling()
        {
            if (!_toggleInput.action.triggered)
                return;
            
            _playerModeToggle.Value.Toggle(_playerMode.Value == PlayerMode.Modification ? PlayerMode.Organization : PlayerMode.Modification);
            _furnishingPanel.Present(_playerMode.Value == PlayerMode.Organization);
        }
    }
}