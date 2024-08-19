using Internal.Dependencies.Core;
using Internal.Flow.States;
using Items.Holders;
using Player;
using Transitions;

namespace States
{
    public class GardenState : AState
    {
        private DependencyRecipe<DependencyList<IManageableItemHolder>> _itemHolders = DependencyInjector.GetRecipe<DependencyList<IManageableItemHolder>>();
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        
        public override void OnEnter()
        {
            InitOrganization();
            InitItemHolders();
        }

        public override void OnExit() => DeinitItemHolders();

        protected override void AddConditions() => AddCondition<ShopBootstrapState>(() => Transition.ShouldToggle(TransitionType.Shop));

        private void InitOrganization() => _playerModeToggle.Value.Toggle(PlayerMode.Modification);

        private void InitItemHolders()
        {
            foreach (IManageableItemHolder itemHolder in _itemHolders.Value)
                itemHolder.SetInitial();
        }

        private void DeinitItemHolders()
        {
            foreach (IManageableItemHolder itemHolder in _itemHolders.Value)
                itemHolder.Destroy();
        }
    }
}