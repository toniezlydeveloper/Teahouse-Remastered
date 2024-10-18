using System;
using Items.Implementations;

namespace Items.Holders
{
    public abstract class AAddInItemHolder<TAddIn> : WorldSpaceItemHolder where TAddIn : Enum
    {
        public override string Name => $"{GetStorage()?.Name}Holder";

        private void Start() => SetupVisuals();

        protected virtual void SetupVisuals()
        {
        }

        public override void ToggleDown()
        {
        }

        public override void ToggleUp()
        {
        }

        protected override bool TryGetInitialItem(out IItem initialItem)
        {
            initialItem = new AddInStorage<TAddIn>();
            return true;
        }

        private AddInStorage<TAddIn> GetStorage() => Value as AddInStorage<TAddIn>;
    }
}