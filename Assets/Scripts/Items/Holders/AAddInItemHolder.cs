using System;
using System.Collections.Generic;
using System.Linq;
using Items.Implementations;
using UnityEngine;

namespace Items.Holders
{
    public abstract class AAddInItemHolder<TAddIn> : WorldSpaceItemHolder where TAddIn : Enum
    {
        [SerializeField] private bool requiresProcessing;
        [SerializeField] private float processingSpeed;
        [SerializeField] private CachedItemHolder hand;
        [SerializeField] private TAddIn initialType;

        private List<TAddIn> _allAddIns;
        private int _selectedIndex;

        public override string Name => $"{GetStorage().Name}Holder";

        private void Start()
        {
            GetReferences();
            SetupVisuals();
        }

        protected virtual void SetupVisuals()
        {
        }

        public override void ToggleDown()
        {
            GetStorage().Type = GetPrevious();
            GetStorage().Reset();
        }

        public override void ToggleUp()
        {
            GetStorage().Type = GetNext();
            GetStorage().Reset();
        }

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
            initialItem = new AddInStorage<TAddIn>
            {
                RequiresProcessing = requiresProcessing,
                Type = initialType
            };
            return true;
        }

        private bool IsPlayerHoldingSomething() => !hand.IsEmpty();

        private AddInStorage<TAddIn> GetStorage() => Value as AddInStorage<TAddIn>;

        private void Progress(AddInStorage<TAddIn> storage) => storage.ProcessingProgress += Time.deltaTime * processingSpeed;

        private TAddIn GetPrevious()
        {
            _selectedIndex = (--_selectedIndex + _allAddIns.Count) % _allAddIns.Count;
            return _allAddIns[_selectedIndex];
        }

        private TAddIn GetNext()
        {
            _selectedIndex = (++_selectedIndex + _allAddIns.Count) % _allAddIns.Count;
            return _allAddIns[_selectedIndex];
        }

        private void GetReferences()
        {
            _allAddIns = ((TAddIn[])Enum.GetValues(typeof(TAddIn))).Skip(1).ToList();
            _selectedIndex = _allAddIns.IndexOf(initialType);
        }
    }
}