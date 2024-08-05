using Customers;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using Internal.Pooling;
using Items.Holders;
using Organization;
using UI.Core;
using UnityEngine;

namespace States
{
    // 1. Dekoracja pokoju
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

        private ITimePanel _timePanel;
        
        private void Awake()
        {
            InjectListRecipes();
            GetReferences();
            
            AddState(new ShopBootstrapState());
            AddState(new ShopOpenedAtDayState(new CustomerSpawner(_timePanel, customerPrefab, data)));
            AddState(new ShopClosedState());
            
            AddState(new GardenBootstrapState());
            AddState(new GardenState());
            
            // todo: change it back to Shop after getting done with Playground
            AddInitialState(new BedroomBoostrapState());
            AddState(new BedroomState());
        }

        private void Start() => DependencyInjector.AddRecipeElement<IManageableItemHolder>(hand);

        private void OnDestroy() => DependencyInjector.RemoveRecipeElement<IManageableItemHolder>(hand);

        private void InjectListRecipes()
        {
            DependencyInjector.InjectListRecipe<IManageableItemHolder>();
            DependencyInjector.InjectListRecipe<IOrganizationPoint>();
            DependencyInjector.InjectListRecipe<IPoolItem>();
        }
        
        private void GetReferences() => _timePanel = GetFromUI<ITimePanel>();
        
        private TDependency GetFromUI<TDependency>() where TDependency : IDependency => uiParent.GetComponentInChildren<TDependency>();
    }
}