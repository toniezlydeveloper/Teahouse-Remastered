using Internal.Flow.States;
using Saving;
using UnityEngine.SceneManagement;

namespace States
{
    public class BedroomBoostrapState : ABootstrapState<BedroomState>
    {
        protected override string LevelName => "Bedroom";

        public override void OnExit() => SavingController.Load(PersistenceType.Volatile, FileSaveType.Bedroom);
    }
    
    public class ShopBootstrapState : ABootstrapState<ShopClosedState>
    {
        protected override string LevelName => "Shop";

        public override void OnExit() => SavingController.Load(PersistenceType.Volatile, FileSaveType.Shop);
    }
    
    public class TutorialBoostrapState : ABootstrapState<TutorialState>
    {
        protected override string LevelName => "Tutorial";
    }
    
    public class CharacterBoostrapState : ABootstrapState<CharacterState>
    {
        protected override string LevelName => "Character";
    }

    public abstract class ABootstrapState<TTargetState> : AState where TTargetState : AState
    {
        protected abstract string LevelName { get; }
        
        public override void OnEnter() => SceneManager.LoadScene(LevelName);

        protected override void AddConditions() => AddCondition<TTargetState>(() => SceneManager.GetSceneByName(LevelName).isLoaded);
    }
}