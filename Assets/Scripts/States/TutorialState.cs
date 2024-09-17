using System;
using Internal.Dependencies.Core;
using Internal.Flow.States;
using Tutorial;
using UI.Shared;
using UnityEngine.InputSystem;

namespace States
{
    public class TutorialState : AState
    {
        private DependencyRecipe<DependencyList<ITutorialCamera>> _tutorialCameras = DependencyInjector.GetRecipe<DependencyList<ITutorialCamera>>();
        private InputAction _progressInput;
        private IDialogPanel _dialogPanel;
        private TutorialConfig _config;
        private int _currentStepIndex;

        public TutorialState(InputAction progressInput, IDialogPanel dialogPanel, TutorialConfig config)
        {
            _progressInput = progressInput;
            _dialogPanel = dialogPanel;
            _config = config;
        }

        public override void OnEnter() => ShowNextStep();

        public override Type OnUpdate()
        {
            if (!ReceivedProgressInput())
            {
                return null;
            }

            if (HasRunOutOfSteps())
            {
                return typeof(ShopBootstrapState);
            }

            ShowNextStep();
            return null;
        }

        protected override void AddConditions() => AddCondition<ShopBootstrapState>(ShouldSkipTutorial);

        private bool ReceivedProgressInput() => _progressInput.triggered;

        private bool HasRunOutOfSteps() => _currentStepIndex >= _config.Steps.Length;

        private void ShowNextStep()
        {
            TutorialStep step = _config.Steps[_currentStepIndex++];
            
            foreach (ITutorialCamera camera in _tutorialCameras.Value)
            {
                camera.Toggle(step.CameraType);
            }
            
            _dialogPanel.Present(new DialogData
            {
                Icon = step.Icon,
                Text = step.Text
            });
        }

        private bool ShouldSkipTutorial() => _config.ShouldSkipSteps;
    }
}