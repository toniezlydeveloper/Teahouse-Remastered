using System;
using System.Collections.Generic;
using System.Linq;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using Internal.Flow.UI;
using UnityEngine;

namespace Internal.Flow
{
    public class GameManager : MonoBehaviour
    {
        private AUIPanelsProvider _panelsProvider;
        private AStatesProvider _statesProvider;
        private AUIPanel _currentPanel;
        private AState _currentState;

        private void Awake() => GetReferences();

        private void Start()
        {
            InjectSceneDependencies();
            InjectStateDependencies();
            InjectUIDependencies();
            DontDestroyOnLoad();
            EnterInitialState();
            InitUITransitions();
        }

        private void Update()
        {
            if (!TryGetNextStateType(out Type stateType))
            {
                return;
            }

            TogglePanel(stateType);
            EnterState(stateType);
        }

        private void InjectSceneDependencies() => DependencyInjector.Inject(GetSceneDependencies());

        private void InjectStateDependencies() => DependencyInjector.Inject(GetStateDependencies());

        private void InjectUIDependencies() => DependencyInjector.Inject(GetUIDependencies());

        private void DontDestroyOnLoad() => DontDestroyOnLoad(gameObject);

        private void EnterInitialState()
        {
            TogglePanel(GetInitialState());
            EnterState(GetInitialState());
        }

        private void InitUITransitions()
        {
            foreach (AUIPanel panel in _panelsProvider.PanelsByStateType.Values)
            {
                panel.OnTransitionRequested += TogglePanel;
                panel.OnTransitionRequested += EnterState;
            }
        }

        private IDependency[] GetSceneDependencies() => GetComponentsInChildren<IDependency>();

        private IDependency[] GetStateDependencies()
        {
            IEnumerable<IDependency> stateDependencies = _statesProvider.StatesByType.Values.OfType<IDependency>();
            return stateDependencies.ToArray();
        }

        private IDependency[] GetUIDependencies()
        {
            IEnumerable<IDependency> uiDependencies = _panelsProvider.PanelsParent.GetComponents<IDependency>();
            IEnumerable<IDependency> panelDependencies = _panelsProvider.PanelsByStateType.Values.SelectMany(panel => panel.GetComponentsInChildren<IDependency>());
            return uiDependencies.Concat(panelDependencies).ToArray();
        }

        private Type GetInitialState() => _statesProvider.InitialStateType;

        private bool TryGetNextStateType(out Type stateType)
        {
            StateTransition transition = _currentState?.Transitions.FirstOrDefault(transition => transition.Conditions.All(condition => condition.Invoke()));
            stateType = transition?.TargetType;

            if (stateType != null)
                return true;
            
            stateType = _currentState?.OnUpdate();
            return stateType != null;
        }

        private void TogglePanel(Type stateType)
        {
            AUIPanel newPanel = _panelsProvider.PanelsByStateType.GetValueOrDefault(stateType);
            AUIPanel oldPanel = _currentPanel;
            _currentPanel = newPanel;

            if (oldPanel != null)
                oldPanel.Disable();
            
            if (newPanel != null)
                newPanel.Enable();
        }

        private void EnterState(Type stateType)
        {
            AState newState = _statesProvider.StatesByType[stateType];
            AState oldState = _currentState;
            _currentState = newState;
            
            oldState?.OnExit();
            newState?.OnEnter();

            Debug.Log($"[GameManager] Changing state [{oldState}] to [{newState}]");
        }

        private void GetReferences()
        {
            _panelsProvider = GetComponent<AUIPanelsProvider>();
            _statesProvider = GetComponent<AStatesProvider>();
        }
    }
}