using Customers;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using Internal.Pooling;
using Player;
using Saving;
using Transitions;

namespace States
{
    public class ShopClosedState : AState
    {
        private DependencyRecipe<DependencyList<IPoolItem>> _poolItems = DependencyInjector.GetRecipe<DependencyList<IPoolItem>>();
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        private DependencyRecipe<IDoorHinge> _doorHinge = DependencyInjector.GetRecipe<IDoorHinge>();
        
        public override void OnEnter()
        {
            InitOrganization();
            ToggleDoorHinge(false);
        }

        protected override void AddConditions()
        {
            AddCondition<ShopOpenedAtDayState>(() =>
            {
                if (!Transition.ShouldToggle(TransitionType.OpenCloseShop))
                    return false;
                
                ToggleDoorHinge(true);
                return true;
            });
            AddCondition<BedroomBoostrapState>(() =>
            {
                if (!Transition.ShouldToggle(TransitionType.Bedroom))
                    return false;
                
                SavingController.Save(PersistenceType.Volatile, FileSaveType.Shop);
                ReleasePools();
                return true;
            });
        }

        private void InitOrganization() => _playerModeToggle.Value.Toggle(PlayerMode.Organization);

        private void ToggleDoorHinge(bool state) => _doorHinge.Value?.Toggle(state);

        private void ReleasePools()
        {
            foreach (IPoolItem poolItem in _poolItems.Value)
                poolItem.TryReleasing();
        }
    }
}