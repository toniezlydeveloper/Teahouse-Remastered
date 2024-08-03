using System;
using UnityEngine;

namespace Internal.Variables.Core
{
    public abstract class AChannel<TVariable> : ScriptableObject
    {
        public event Action<TVariable> OnChanged;

        public TVariable Value
        {
            set => OnChanged?.Invoke(value);
        }
    }
}