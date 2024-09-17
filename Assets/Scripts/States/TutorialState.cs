using System;
using Internal.Flow.States;
using UI.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace States
{
    [Serializable]
    public class DialogStep
    {
        [field:SerializeField] public Sprite Icon { get; set; }
        [field:SerializeField] public string Text { get; set; }
    }
    
    public class TutorialState : AState
    {
        private InputAction _progressInput;
        private IDialogPanel _dialogPanel;
        private int _currentStepIndex;
        private DialogStep[] _steps;

        public TutorialState(InputAction progressInput, IDialogPanel dialogPanel, DialogStep[] steps)
        {
            _progressInput = progressInput;
            _dialogPanel = dialogPanel;
            _steps = steps;
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

        private bool ReceivedProgressInput() => _progressInput.triggered;

        private bool HasRunOutOfSteps() => _currentStepIndex >= _steps.Length;

        private void ShowNextStep()
        {
            DialogStep step = _steps[_currentStepIndex++];
            
            _dialogPanel.Present(new DialogData
            {
                Icon = step.Icon,
                Text = step.Text
            });
        }
    }
}