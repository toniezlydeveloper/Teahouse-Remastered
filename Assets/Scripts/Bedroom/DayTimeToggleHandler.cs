using Interaction;
using Player;
using UnityEngine;

namespace Bedroom
{
    public class DayTimeToggleHandler : AInteractionHandler
    {
        [SerializeField] private DayTimeProxy timeProxy;
        
        public override PlayerMode HandledModes => PlayerMode.Modification | PlayerMode.Organization;

        public override void HandleInteractionInput(InteractionElement element)
        {
            if (!TryGetInteractionComponent(element, out DayTimeToggle _))
                return;
            
            Toggle();
        }
        
        private void Toggle() => timeProxy.Value = timeProxy.Value == DayTime.Day ? DayTime.Night : DayTime.Day;
    }
}