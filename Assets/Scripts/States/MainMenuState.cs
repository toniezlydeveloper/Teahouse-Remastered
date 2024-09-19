using Internal.Flow.States;
using Utilities;

namespace States
{
    public class MainMenuState : AState
    {
        protected override void AddConditions() => AddCondition<ShopBootstrapState>(() => DevelopmentConfig.Instance.ShouldSkipMainMenu);
    }
}