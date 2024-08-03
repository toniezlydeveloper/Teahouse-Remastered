using System.Collections;
using UnityEngine;

namespace Internal.Sequences
{
    public abstract class ASequenceStep : ScriptableObject
    {
        public abstract IEnumerator Execute();
        
        public virtual void Init(GameObject executioner)
        {
        }
    }
}