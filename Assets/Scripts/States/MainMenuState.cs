using Internal.Flow.States;
using Utilities;

namespace States
{
    public class MainMenuState : AState
    {
        protected override void AddConditions() => AddCondition<BedroomBoostrapState>(() => DevelopmentConfig.Instance.ShouldSkipMainMenu);
    }
}