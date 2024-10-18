using System;
using Items.Holders;
using Items.Implementations;
using UnityEngine;

namespace Items.Processors
{
    public abstract class AAddInItemProcessor<TAddIn> : WorldSpaceItemHolder where TAddIn : Enum
    {
        [SerializeField] private float processingSpeed;
        [SerializeField] private CachedItemHolder hand;

        public override string Name => $"{GetStorage()?.Name}Processor";

        public override void Progress()
        {
            if (IsPlayerHoldingSomething())
            {
                return;
            }

            Progress(GetStorage());
        }

        protected override bool TryGetInitialItem(out IItem initialItem)
        {
            initialItem = new AddInProcessor<TAddIn>();
            return true;
        }

        private bool IsPlayerHoldingSomething() => !hand.IsEmpty();

        private AddInProcessor<TAddIn> GetStorage() => Value as AddInProcessor<TAddIn>;

        private void Progress(AddInProcessor<TAddIn> processor)
        {
            if (processor == null || processor.HeldAddIns.Count == 0)
            {
                return;
            }

            processor.ProcessingProgress += Time.deltaTime * processingSpeed;
        }
    }
}