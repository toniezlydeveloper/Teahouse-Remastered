using System;

namespace Items.Holders
{
    public abstract class AddInItemHolder<TAddIn> : WorldSpaceItemHolder where TAddIn : Enum
    {
        private void Start() => Setup();

        protected abstract void Setup();
    }
}