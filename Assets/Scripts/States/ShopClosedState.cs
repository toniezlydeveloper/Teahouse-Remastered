using Customers;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using Internal.Pooling;
using Organization;
using Player;
using Transitions;

namespace States
{
    public class ShopClosedState : AState
    {
        private DependencyRecipe<DependencyList<IOrganizationPoint>> _organizationPoints = DependencyInjector.GetRecipe<DependencyList<IOrganizationPoint>>();
        private DependencyRecipe<DependencyList<IPoolItem>> _poolItems = DependencyInjector.GetRecipe<DependencyList<IPoolItem>>();
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private DependencyRecipe<IDoorHinge> _doorHinge = DependencyInjector.GetRecipe<IDoorHinge>();
        
        public override void OnEnter()
        {
            InitOrganization();
            ToggleDoorHinge(false);
            PopulateOrganizationPoints();
        }

        public override void OnExit()
        {
            ToggleDoorHinge(true);
        }

        protected override void AddConditions()
        {
            AddCondition<ShopOpenedAtDayState>(() => Transition.ShouldToggle(TransitionType.OpenCloseShop));
            AddCondition<GardenBootstrapState>(() =>
            {
                if (!Transition.ShouldToggle(TransitionType.Garden))
                    return false;
                
                ReleasePools();
                return true;
            });
            AddCondition<BedroomBoostrapState>(() =>
            {
                if (!Transition.ShouldToggle(TransitionType.Bedroom))
                    return false;
                
                ReleasePools();
                return true;
            });
        }

        private void InitOrganization() => _playerModeToggle.Value.Toggle(PlayerMode.Organization);

        private void ToggleDoorHinge(bool state) => _doorHinge.Value?.Toggle(state);

        private void PopulateOrganizationPoints()
        {
            foreach (IOrganizationPoint point in _organizationPoints.Value)
                point.Populate();
        }

        private void ReleasePools()
        {
            foreach (IPoolItem poolItem in _poolItems.Value)
                poolItem.TryReleasing();
        }
    }
}