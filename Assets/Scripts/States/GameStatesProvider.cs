using Bedroom;
using Customers;
using Furniture;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using Internal.Pooling;
using Items.Holders;
using Player;
using Saving;
using Tutorial;
using UI.Core;
using UI.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace States
{
    public class GameStatesProvider : AStatesProvider
    {
        [Header("General")]
        [SerializeField] private InputActionReference pause;
        [SerializeField] private CachedItemHolder hand;
        [SerializeField] private DayTimeProxy dayTime;
        [SerializeField] private GameObject uiParent;
        [SerializeField] private TextAsset[] initialSaves;
        
        [Header("Tutorial")]
        [SerializeField] private InputActionReference progressDialog;
        [SerializeField] private TutorialConfig tutorialConfig;

        [Header("Opened At Day")]
        [SerializeField] private Customer customerPrefab;
        [SerializeField] private SpawnData data;

        [Header("Opened At Day")]
        [SerializeField] private InputActionReference[] inputsToDisable;
        [SerializeField] private InputActionReference controls;
        [SerializeField] private InputActionReference back;
        
        [Header("Bedroom")]
        [SerializeField] private PurchasableItemsConfig purchasableItems;
        [SerializeField] private InputActionReference toggle;
        [SerializeField] private PlayerModeProxy playerMode;

        private ISelectableFurniturePanel _selectableFurniturePanel;
        private IMainMenuPanel _mainMenuPanel;
        private IDialogPanel _dialogPanel;
        private IPausePanel _pausePanel;
        private INotesPanel _notesPanel;
        private ITimePanel _timePanel;
        
        private void Awake()
        {
            InjectListRecipes();
            GetReferences();
            
            AddInitialState(new MainMenuBootstrapState());
            AddState(new MainMenuState(_mainMenuPanel, initialSaves));
            
            AddState(new CharacterBoostrapState());
            AddState(new CharacterState());
            
            AddState(new TutorialBoostrapState());
            AddState(new TutorialState(progressDialog, _dialogPanel, tutorialConfig, pause, _pausePanel));
            
            AddState(new ShopDayBootstrapState());
            AddState(new ShopNightBootstrapState());
            AddState(new ShopOpenedState(new CustomerSpawner(_timePanel, customerPrefab, data), inputsToDisable, toggle, _notesPanel, pause, _pausePanel));
            AddState(new ShopClosedAtDayState(pause, _pausePanel));
            AddState(new ShopClosedAtNightState(dayTime, pause, _pausePanel));
            
            AddState(new BedroomDayBoostrapState());
            AddState(new BedroomDayState(pause, _pausePanel));
            AddState(new BedroomNightBoostrapState());
            AddState(new BedroomNightState(toggle, back, _selectableFurniturePanel, purchasableItems, playerMode, dayTime, pause, _pausePanel));
            
            AddState(new CallingState(controls, back));
            
            AddState(new QuitState());
        }

        private void Start()
        {
            SavingController.OverrideSafe(PersistenceType.Volatile, PersistenceType.Persistent);
            DependencyInjector.AddRecipeElement<IManageableItemHolder>(hand);
        }

        private void OnDestroy()
        {
            DependencyInjector.RemoveRecipeElement<IManageableItemHolder>(hand);
            SavingController.ClearVolatile();
        }

        private void InjectListRecipes()
        {
            DependencyInjector.InjectListRecipe<IManageableItemHolder>();
            DependencyInjector.InjectListRecipe<IFurnishingListener>();
            DependencyInjector.InjectListRecipe<ITutorialCamera>();
            DependencyInjector.InjectListRecipe<IPoolItem>();
            DependencyInjector.InjectListRecipe<ICustomer>();
        }
        
        private void GetReferences()
        {
            _selectableFurniturePanel = GetFromUI<ISelectableFurniturePanel>();
            _mainMenuPanel = GetFromUI<IMainMenuPanel>();
            _dialogPanel = GetFromUI<IDialogPanel>();
            _dialogPanel = GetFromUI<IDialogPanel>();
            _notesPanel = GetFromUI<INotesPanel>();
            _pausePanel = GetFromUI<IPausePanel>();
            _timePanel = GetFromUI<ITimePanel>();
        }

        private TDependency GetFromUI<TDependency>() where TDependency : IDependency => uiParent.GetComponentInChildren<TDependency>();
    }
}