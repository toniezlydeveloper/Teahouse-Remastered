using Internal.Flow.States;
using UnityEngine.InputSystem;

namespace States
{
    public class CallingState : AState
    {
        private InputActionReference _controls;
        private InputActionReference _back;

        public CallingState(InputActionReference controls, InputActionReference back)
        {
            _controls = controls;
            _back = back;
        }

        public override void OnEnter() => ToggleControls(false);

        public override void OnExit() => ToggleControls(true);

        protected override void AddConditions() => AddCondition<ShopClosedAtDayState>(ReceivedBackInput);

        private bool ReceivedBackInput() => _back.action.triggered;

        private void ToggleControls(bool state)
        {
            if (state)
                _controls.action.Enable();
            else
                _controls.action.Disable();
        }
    }
}