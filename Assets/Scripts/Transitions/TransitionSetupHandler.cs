using Interaction;
using Player;

namespace Transitions
{
    public class TransitionSetupHandler : AInteractionHandler
    {
        public override PlayerMode HandledModes => PlayerMode.Modification | PlayerMode.Organization;

        public override void HandleInteractionInput(InteractionElement element)
        {
            if (!TryGetInteractionComponent(element, out Transition transition))
                return;
            
            Setup(transition);
        }

        private void Setup(Transition transition) => transition.Setup();
    }
}