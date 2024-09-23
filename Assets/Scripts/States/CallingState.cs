using Internal.Flow.States;
using UnityEngine.InputSystem;

namespace States
{
    public class CallingState : AState
    {
        private InputActionReference _back;

        public CallingState(InputActionReference back) => _back = back;

        protected override void AddConditions() => AddCondition<ShopClosedAtDayState>(ReceivedBackInput);

        private bool ReceivedBackInput() => _back.action.triggered;
    }
}