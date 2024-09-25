using System;
using System.Collections.Generic;
using Internal.Dependencies.Core;
using Saving;
using Tutorial;
using UI.Shared;
using UnityEngine.InputSystem;
using Utilities;

namespace States
{
    public class TutorialState : APauseAllowedState
    {
        private DependencyRecipe<DependencyList<ITutorialCamera>> _tutorialCameras = DependencyInjector.GetRecipe<DependencyList<ITutorialCamera>>();
        private InputAction _progressInput;
        private IDialogPanel _dialogPanel;
        private TutorialConfig _config;
        private int _currentStepIndex;

        protected override List<FileSaveType> TypesToSave => new List<FileSaveType>();

        public TutorialState(InputAction progressInput, IDialogPanel dialogPanel, TutorialConfig config, InputActionReference pauseInput, IPausePanel pausePanel) : base(pauseInput, pausePanel)
        {
            _progressInput = progressInput;
            _dialogPanel = dialogPanel;
            _config = config;
        }

        public override void OnEnter()
        {
            SavingController.Save(PersistenceType.Persistent, FileSaveType.Shop);
            ResetSteps();
            ShowNextStep();
        }

        public override Type OnUpdate()
        {
            HandlePause();
            
            if (!ReceivedProgressInput())
            {
                return null;
            }

            if (HasRunOutOfSteps())
            {
                return typeof(BedroomNightBoostrapState);
            }

            ShowNextStep();
            return null;
        }

        protected override void AddConditions() => AddCondition<BedroomNightBoostrapState>(() => DevelopmentConfig.Instance.ShouldSkipTutorial);

        private bool ReceivedProgressInput() => _progressInput.triggered;

        private bool HasRunOutOfSteps() => _currentStepIndex >= _config.Steps.Length;

        private void ResetSteps() => _currentStepIndex = 0;

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
    }
}