using Internal.Dependencies.Core;
using Internal.Flow.States;
using Organization;
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
        private DependencyRecipe<DependencyList<IOrganizationPoint>> _organizationPoints = DependencyInjector.GetRecipe<DependencyList<IOrganizationPoint>>();
        
        protected override string LevelName => "Shop";

        public override void OnExit()
        {
            PopulateOrganizationPoints();
            SavingController.Load(PersistenceType.Volatile, FileSaveType.Shop);
        }

        private void PopulateOrganizationPoints()
        {
            foreach (IOrganizationPoint point in _organizationPoints.Value)
                point.Populate();
        }
    }

    public abstract class ABootstrapState<TTargetState> : AState where TTargetState : AState
    {
        protected abstract string LevelName { get; }
        
        public override void OnEnter() => SceneManager.LoadScene(LevelName);

        protected override void AddConditions() => AddCondition<TTargetState>(() => SceneManager.GetSceneByName(LevelName).isLoaded);
    }
}