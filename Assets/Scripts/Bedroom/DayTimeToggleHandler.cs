using Interaction;
using Player;
using UnityEngine;

namespace Bedroom
{
    public class DayTimeToggleHandler : AInteractionHandler
    {
        [SerializeField] private DayTimeProxy timeProxy;
        
        public override PlayerMode HandledModes => PlayerMode.Modification | PlayerMode.Organization;
        public override DayTime HandledDayTime => DayTime.Night;

        public override void HandleInteractionInput(InteractionElement element)
        {
            if (!TryGetInteractionComponent(element, out DayTimeToggle _))
            {
                return;
            }
            
            GoToDay();
        }
        
        private void GoToDay() => timeProxy.Value = DayTime.Day;
    }
}