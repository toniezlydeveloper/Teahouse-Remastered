using UnityEngine;

namespace Internal.Variables.Core
{
    public abstract class AVariable<TVariable> : ScriptableObject
    {
        public TVariable Value { get; set; }
    }
}