using Bedroom;
using Interaction;
using Internal.Dependencies.Core;
using Items.Holders;
using Player;
using UI.Items;
using UnityEngine;

namespace Items.Modification
{
    public class ModificationHandler : AInteractionHandler
    {
        [SerializeField] private CachedItemHolder hand;

        private IItemPanel _itemPanel = DependencyInjector.Get<IItemPanel>();
        private ModificationPreviewer _itemPreviewer;

        public override PlayerMode HandledModes => PlayerMode.Modification;
        public override DayTime HandledDayTime => DayTime.Day;

        public override void HandleInteractionDownInput(InteractionElement element)
        {
            if (!TryGetInteractionComponent(element, out WorldSpaceItemHolder itemHolder))
                return;
            
            ToggleDown(itemHolder);
        }

        public override void HandleInteractionUpInput(InteractionElement element)
        {
            if (!TryGetInteractionComponent(element, out WorldSpaceItemHolder itemHolder))
                return;
            
            ToggleUp(itemHolder);
        }

        public override void HandleInteractionInput(InteractionElement element)
        {
            if (!TryGetInteractionComponent(element, out WorldSpaceItemHolder itemHolder))
                return;
            
            ModifiersFactory.Interact(hand, itemHolder, itemHolder.ModifierType);
        }

        public override void HandleProgressInput(InteractionElement element)
        {
            if (!TryGetInteractionComponent(element, out WorldSpaceItemHolder itemHolder))
                return;
            
            Progress(itemHolder);
        }

        private void ToggleDown(WorldSpaceItemHolder itemHolder) => itemHolder.ToggleDown();

        private void ToggleUp(WorldSpaceItemHolder itemHolder) => itemHolder.ToggleUp();

        private void Progress(WorldSpaceItemHolder itemHolder) => itemHolder.Progress();
        
        public override void Present(InteractionElement element)
        {
            WorldSpaceItemHolder itemHolder = null;
            bool hasHolder = element != null && element.TryGetComponent(out itemHolder);

            if (hasHolder)
                _itemPanel.Present(itemHolder);
            else
                _itemPanel.Present(hand);
            
            _itemPreviewer?.Toggle(true);

            if (!hasHolder)
                return;

            itemHolder.TryGetComponent(out _itemPreviewer);
            _itemPreviewer?.Toggle(false);
        }
    }
}