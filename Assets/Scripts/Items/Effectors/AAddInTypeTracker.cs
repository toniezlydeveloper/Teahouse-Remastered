using System;
using Items.Implementations;

namespace Items.Effectors
{
    public abstract class AAddInTypeTracker<TAddIn> : AEffector<IAddInType<TAddIn>> where TAddIn : Enum
    {
        private TAddIn _type;
        
        protected override bool TryEffecting(IAddInType<TAddIn> addInType)
        {
            if (!TryChanging(addInType, out TAddIn type))
            {
                return false;
            }

            ChangeType(type);
            return true;
        }

        private bool TryChanging(IAddInType<TAddIn> addInType, out TAddIn type)
        {
            type = default;
            
            if (addInType == null)
            {
                return false;
            }
            
            int comparisonResult = _type.CompareTo(addInType.Value);
            _type = type = addInType.Value;
            return comparisonResult != 0;
        }

        protected abstract void ChangeType(TAddIn type);
    }
}