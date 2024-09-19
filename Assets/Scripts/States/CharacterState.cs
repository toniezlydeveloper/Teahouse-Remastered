using Internal.Flow.States;
using Saving;
using Utilities;

namespace States
{
    public class CharacterState : AState
    {
        public override void OnExit() => SavingController.Save(PersistenceType.Persistent, FileSaveType.Character);

        protected override void AddConditions() => AddCondition<TutorialBoostrapState>(() => DevelopmentConfig.Instance.ShouldSkipCharacterCustomization);
    }
}