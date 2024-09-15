using System;
using Items.Implementations;

namespace Items.Effectors
{
    public abstract class AAddInTypeTracker<TAddIn> : AEffector<IAddInType<TAddIn>> where TAddIn : Enum
    {
        private TAddIn _type;
        
        protected override bool TryEffecting(IAddInType<TAddIn> addIn)
        {
            if (!TryChanging(addIn, out TAddIn type))
            {
                return false;
            }

            ChangeType(type);
            return true;
        }

        private bool TryChanging(IAddInType<TAddIn> addIn, out TAddIn type)
        {
            type = default;
            
            if (addIn == null)
            {
                return false;
            }
            
            int comparisonResult = _type.CompareTo(addIn.Type);
            _type = type = addIn.Type;
            return comparisonResult != 0;
        }

        protected abstract void ChangeType(TAddIn type);
    }
}