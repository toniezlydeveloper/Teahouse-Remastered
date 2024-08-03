using Internal.Dependencies.Core;
using Internal.Flow.States;
using Player;
using Transitions;

namespace States
{
    public class GardenState : AState
    {
        private DependencyRecipe<IPlayerModeToggle> _playerModeToggle = DependencyInjector.GetRecipe<IPlayerModeToggle>();
        
        public override void OnEnter() => InitOrganization();

        protected override void AddConditions() => AddCondition<ShopBootstrapState>(() => Transition.ShouldToggle(TransitionType.Shop));

        private void InitOrganization() => _playerModeToggle.Value.Toggle(PlayerMode.Modification);
    }
}