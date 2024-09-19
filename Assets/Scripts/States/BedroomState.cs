using System;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using Player;
using Saving;
using Transitions;
using UI.Shared;
using UnityEngine.InputSystem;

namespace States
{
    public class BedroomState : AState
    {
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private InputActionReference _toggleInput;
        private IFurnishingPanel _furnishingPanel;
        private PlayerModeProxy _playerMode;

        public BedroomState(InputActionReference toggleInput, PlayerModeProxy playerMode, IFurnishingPanel furnishingPanel)
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