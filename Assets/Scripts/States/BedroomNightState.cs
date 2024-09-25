using System;
using System.Collections.Generic;
using System.Linq;
using Bedroom;
using Furniture;
using Grids;
using Internal.Dependencies.Core;
using Player;
using Saving;
using Transitions;
using UI.Core;
using UI.Shared;
using UnityEngine.InputSystem;
using Utilities;

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

    public interface IFurnitureSelector : IDependency
    {
        GridDimensions Dimensions { get; }
        
        void HandleSelection(out IFurniturePiece selectedPiece);
    }

    public class BedroomNightState : APauseAllowedState, IFurnishingCore, IFurnitureSelector
    {
        private DependencyRecipe<DependencyList<IFurnishingListener>> _furnishingListeners = DependencyInjector.GetRecipe<DependencyList<IFurnishingListener>>();
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private ISelectableFurniturePanel _selectablePanel;
        private GridDimensions _selectedPieceDimensions;
        private IFurniturePiece _selectedPiece;
        private InputActionReference _toggle;
        private PlayerModeProxy _playerMode;
        private InputActionReference _back;
        private DayTimeProxy _dayTime;
        private bool _isEnabled;

        public GridDimensions Dimensions => _selectedPieceDimensions;
        public bool IsEnabled => _isEnabled;

        protected override List<FileSaveType> TypesToSave => new List<FileSaveType>
        {
            FileSaveType.Inventory,
            FileSaveType.Bedroom
        };
        
        private static readonly GridDimensions DefaultDimensions = new GridDimensions { Width = 1, Height = 1 };

        public BedroomNightState(InputActionReference toggle, InputActionReference back, ISelectableFurniturePanel selectablePanel, PurchasableItemsConfig config, PlayerModeProxy playerMode, DayTimeProxy dayTime, InputActionReference pauseInput, IPausePanel pausePanel) : base(pauseInput, pausePanel)
        {
            GetReferences(playerMode, dayTime, toggle, back);
            Init(selectablePanel, config);
        }

        public override void OnEnter()
        {
            SavingController.Load(PersistenceType.Persistent, FileSaveType.Character);
            DisableFurnishing();
            AddCallbacks();
        }

        public override void OnExit()
        {
            DisableFurnishing();
            RemoveCallbacks();
        }

        public override Type OnUpdate()
        {
            if (ReceivedToggleInput())
            {
                ToggleFurnishing();
            }
            
            if (ReceivedDisableInput())
            {
                DisableFurnishing();
            }
            
            HandlePause();
            return null;
        }

        public void HandleSelection(out IFurniturePiece selectedPiece) => ReadOutput(out selectedPiece);

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

        private void ToggleFurnishing()
        {
            ToggleMode();
            RefreshState();
        }

        private void DisableFurnishing()
        {
            InitModification();
            HandleSelection(null);
            RefreshState();
        }

        private bool ReceivedDisableInput() => _back.action.triggered;

        private bool ReceivedToggleInput() => _toggle.action.triggered;

        private bool Is(DayTime dayTime) => _dayTime.Value == dayTime;

        private void InitModification() => _playerModeToggle.Value.Toggle(PlayerMode.Modification);

        private void ToggleMode() => _playerModeToggle.Value.Toggle(_playerMode.Value == PlayerMode.Modification ? PlayerMode.Organization : PlayerMode.Modification);

        private void RefreshState()
        {
            bool state = _playerMode.Value == PlayerMode.Organization;
            
            foreach (IFurnishingListener listener in _furnishingListeners.Value)
            {
                listener.Toggle(state);
            }

            _isEnabled = state;
        }

        private void HandleSelection(IFurniturePiece piece)
        {
            _selectedPiece = _selectedPiece == piece ? null : piece;
            _selectedPieceDimensions = _selectedPiece != null ? _selectedPiece.Dimensions : DefaultDimensions;
        }

        private bool IsSelected(IFurniturePiece piece) => _selectedPiece == piece;

        private void ReadOutput(out IFurniturePiece selectedPiece) => selectedPiece = _selectedPiece;

        private void GetReferences(PlayerModeProxy playerMode, DayTimeProxy dayTime, InputActionReference toggle, InputActionReference back)
        {
            _playerMode = playerMode;
            _dayTime = dayTime;
            _toggle = toggle;
            _back = back;
        }

        private void Init(ISelectableFurniturePanel selectablePanel, PurchasableItemsConfig config) => selectablePanel.Present(new FurnishingData
        {
            PiecesData = config.Set.Select(item => new SelectableFurniturePieceData
            {
                SelectionCallback = () => HandleSelection(item),
                IsSelectedCallback = () => IsSelected(item),
                Text = "No bonuses for this item yet.",
                Category = item.Category,
                Name = item.Name,
                Icon = item.Icon,
                Cost = item.Cost
            }).ToArray(),
            ToggleCallback = ToggleFurnishing
        });

        private void AddCallbacks() => _dayTime.OnChanged += DisableOrganization;

        private void RemoveCallbacks() => _dayTime.OnChanged -= DisableOrganization;
    }
}