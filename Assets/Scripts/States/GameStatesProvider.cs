using Currency;
using Customers;
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
    // 1. Dekoracja pokoju v
    // 2. Wytwarzanie skladnikow ze skladnikow
    // 3. Inventory do przekladania rzeczy
    // 4. Premie / nocni klienci
    // 5. Splacanie wujka herbaciarza
    // 6. Tutorial
    // 7. Sklep z rzeczami
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
        [SerializeField] private SaleItem[] itemsForSale;
        
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
            GetReferences();
            
            AddInitialState(new ShopBootstrapState());
            AddState(new ShopOpenedAtDayState(new CustomerSpawner(_timePanel, customerPrefab, data)));
            AddState(new ShopClosedState());
            
            AddState(new GardenBootstrapState());
            AddState(new GardenState());
            
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
            DependencyInjector.InjectListRecipe<IPoolItem>();
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