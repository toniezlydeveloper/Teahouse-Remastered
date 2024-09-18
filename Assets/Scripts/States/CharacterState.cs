using Internal.Flow.States;
using Utilities;

namespace States
{
    public class CharacterState : AState
    {
        protected override void AddConditions() => AddCondition<TutorialBoostrapState>(() => DevelopmentConfig.Instance.ShouldSkipCharacterCustomization);
    }
}