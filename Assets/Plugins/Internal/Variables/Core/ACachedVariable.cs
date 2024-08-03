using System;
using UnityEngine;

namespace Internal.Variables.Core
{
    public abstract class ACachedVariable<TVariable> : ScriptableObject
    {
        public event Action<TVariable> OnChanged;

        [NonSerialized] private TVariable _value;
        
        public TVariable Value
        {
            set => SetValue(value);
            get => GetValue();
        }

        private TVariable GetValue() => _value;

        private void SetValue(TVariable value)
        {
            OverrideValue(value);
            NotifyAboutChange();
        }

        private void OverrideValue(TVariable value) => _value = value;

        private void NotifyAboutChange() => OnChanged?.Invoke(_value);
    }
}