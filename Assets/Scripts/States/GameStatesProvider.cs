using Currency;
using Customers;
using Furniture;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using Internal.Pooling;
using Items.Holders;
using Organization;
using Player;
using UI.Core;
using UI.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace States
{
    // 1. Dekoracja pokoju V
    // 2. Wytwarzanie skladnikow ze skladnikow
    // 3. Inventory do przekladania rzeczy V
    // 4. Premie / nocni klienci
    // 5. Splacanie wujka herbaciarza
    // 6. Tutorial
    // 7. Sklep z rzeczami V
    public class GameStatesProvider : AStatesProvider
    {
        [Header("General")]
        [SerializeField] private CachedItemHolder hand;
        [SerializeField] private GameObject uiParent;

        [Header("Opened At Day")]
        [SerializeField] private Customer customerPrefab;
        [SerializeField] private SpawnData data;

        [Header("Item Shop")]
        [SerializeField] private InputActionReference controls;
        [SerializeField] private InputActionReference back;
        [SerializeField] private TradeItemSet itemsForSale;
        
        [Header("Bedroom")]
        [SerializeField] private InputActionReference toggle;
        [SerializeField] private PlayerModeProxy playerMode;

        private IFurnishingPanel _furnishingPanel;
        private ICurrencyHolder _currencyHolder;
        private IItemShopPanel _itemShopPanel;
        private ITimePanel _timePanel;
        
        private void Awake()
        {
            InjectListRecipes();
            InitListRecipes();
            GetReferences();
            
            AddInitialState(new ShopBootstrapState());
            AddState(new ShopOpenedAtDayState(new CustomerSpawner(_timePanel, customerPrefab, data)));
            AddState(new ShopClosedState());
            
            AddState(new BedroomBoostrapState());
            AddState(new BedroomState(toggle, playerMode, _furnishingPanel));
            
            AddState(new ItemShopState(itemsForSale, _itemShopPanel, controls, back, _furnishingPanel, _currencyHolder));
        }

        private void Start() => DependencyInjector.AddRecipeElement<IManageableItemHolder>(hand);

        private void OnDestroy() => DependencyInjector.RemoveRecipeElement<IManageableItemHolder>(hand);

        private void InjectListRecipes()
        {
            DependencyInjector.InjectListRecipe<IManageableItemHolder>();
            DependencyInjector.InjectListRecipe<IOrganizationPoint>();
            DependencyInjector.InjectListRecipe<IFurniturePiece>();
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
            _currencyHolder = GetFromScene<ICurrencyHolder>();
            _furnishingPanel = GetFromUI<IFurnishingPanel>();
            _itemShopPanel = GetFromUI<IItemShopPanel>();
            _timePanel = GetFromUI<ITimePanel>();
        }
        
        private TDependency GetFromScene<TDependency>() where TDependency : IDependency => gameObject.GetComponentInChildren<TDependency>();

        private TDependency GetFromUI<TDependency>() where TDependency : IDependency => uiParent.GetComponentInChildren<TDependency>();
    }
}