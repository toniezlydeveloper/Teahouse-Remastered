using Player;
using UnityEngine;

namespace Interaction
{
    public abstract class AInteractionHandler : MonoBehaviour
    {
        public abstract PlayerMode HandledModes { get; }

        public virtual void HandleInteractionDownInput(InteractionElement element)
        {
        }

        public virtual void HandleInteractionUpInput(InteractionElement element)
        {
        }

        public virtual void HandleInteractionInput(InteractionElement element)
        {
        }

        public virtual void Present(InteractionElement element)
        {
        }

        protected bool TryGetInteractionComponent<TComponent>(InteractionElement element, out TComponent component)
        {
            component = default;

            if (element == null)
                return false;
            
            return element.TryGetComponent(out component);
        }
    }
}