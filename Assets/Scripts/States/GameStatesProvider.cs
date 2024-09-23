using Bedroom;
using Currency;
using Customers;
using Furniture;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using Internal.Pooling;
using Items.Holders;
using Player;
using Saving;
using Trading;
using Tutorial;
using UI.Core;
using UI.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace States
{
    // 1. Dekoracja pokoju V
    // 2. Wytwarzanie skladnikow ze skladnikow C
    // 3. Inventory do przekladania rzeczy V
    // 4. Premie / nocni klienci C
    // 5. Splacanie wujka herbaciarza
    // 6. Tutorial V
    // 7. Sklep z rzeczami V
    // 8. Dodanie skladnikow z ChatGPT
    // 9. Dodanie systemu zamowien na podstawie rasy zwierzecia
    // 10. Poprawienie tutorialu
    // 11. Dodanie minimalistycznej fabuly
    // 12. Dodanie buffow do prowadzenia sklepu za kupione przedmioty do sypialni
    // 13. Dodanie customizacji postaci V
    // 14. Dodanie elementow multiplayer - odwiedzania i obserwacji
    // 15. Ewentualne dodanie wspolpracy, jesli nie bedzie zbyt czasochlonne
    // 16. Poprawic model kubka, zeby pokazywal wszystkie dodatki
    // 17. Dodac auto-save
    public class GameStatesProvider : AStatesProvider
    {
        [Header("General")]
        [SerializeField] private InputActionReference pause;
        [SerializeField] private CachedItemHolder hand;
        [SerializeField] private DayTimeProxy dayTime;
        [SerializeField] private GameObject uiParent;
        
        [Header("Tutorial")]
        [SerializeField] private InputActionReference progressDialog;
        [SerializeField] private TutorialConfig tutorialConfig;

        [Header("Opened At Day")]
        [SerializeField] private Customer customerPrefab;
        [SerializeField] private SpawnData data;
        
        [Header("Bedroom")]
        [SerializeField] private InputActionReference toggle;
        [SerializeField] private PlayerModeProxy playerMode;

        private IDialogPanel _dialogPanel;
        private IPausePanel _pausePanel;
        private ITimePanel _timePanel;
        
        private void Awake()
        {
            InjectListRecipes();
            InitListRecipes();
            GetReferences();
            
            AddInitialState(new MainMenuBootstrapState());
            AddState(new MainMenuState());
            
            AddState(new CharacterBoostrapState());
            AddState(new CharacterState());
            
            AddState(new TutorialBoostrapState());
            AddState(new TutorialState(progressDialog, _dialogPanel, tutorialConfig, pause, _pausePanel));
            
            AddState(new ShopDayBootstrapState());
            AddState(new ShopNightBootstrapState());
            AddState(new ShopOpenedAtDayState(new CustomerSpawner(_timePanel, customerPrefab, data)));
            AddState(new ShopClosedAtDayState(pause, _pausePanel));
            AddState(new ShopClosedAtNightState(dayTime, pause, _pausePanel));
            
            AddState(new BedroomBoostrapState());
            AddState(new BedroomState(toggle, playerMode, dayTime, pause, _pausePanel));
            
            AddState(new CallingState(pause));
            
            AddState(new QuitState());
        }

        private void Start()
        {
            DependencyInjector.AddRecipeElement<IManageableItemHolder>(hand);
            SavingController.OverrideVolatileWithPersistent();
            SavingController.Load(PersistenceType.Volatile, FileSaveType.Inventory);
        }

        private void OnDestroy()
        {
            DependencyInjector.RemoveRecipeElement<IManageableItemHolder>(hand);
            SavingController.ClearVolatile();
        }

        private void InjectListRecipes()
        {
            DependencyInjector.InjectListRecipe<IManageableItemHolder>();
            DependencyInjector.InjectListRecipe<IFurniturePiece>();
            DependencyInjector.InjectListRecipe<ITutorialCamera>();
            DependencyInjector.InjectListRecipe<IPoolItem>();
        }

        private void InitListRecipes()
        {
            DependencyList<IFurniturePiece> pieces = DependencyInjector.GetRecipe<DependencyList<IFurniturePiece>>().Value;
            pieces.Add(null);
            pieces.Add(null);
            pieces.Add(null);
            pieces.Add(null);
            pieces.Add(null);
        }
        
        private void GetReferences()
        {
            _dialogPanel = GetFromUI<IDialogPanel>();
            _pausePanel = GetFromUI<IPausePanel>();
            _timePanel = GetFromUI<ITimePanel>();
        }
        
        private TDependency GetFromScene<TDependency>() where TDependency : IDependency => gameObject.GetComponentInChildren<TDependency>();

        private TDependency GetFromUI<TDependency>() where TDependency : IDependency => uiParent.GetComponentInChildren<TDependency>();
    }
}