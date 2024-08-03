using System;
using System.Collections.Generic;
using UnityEngine;

namespace Internal.Variables.Core
{
    public abstract class ACachedSet<TElement> : ScriptableObject
    {
        public event Action<List<TElement>> OnChanged;

        [field: NonSerialized] public List<TElement> Set { get; } = new List<TElement>();

        public void Add(TElement value)
        {
            AddElement(value);
            NotifyAboutChange();
        }

        public void Remove(TElement value)
        {
            RemoveElement(value);
            NotifyAboutChange();
        }

        private void AddElement(TElement value) => Set.Add(value);

        private void RemoveElement(TElement value) => Set.Remove(value);

        private void NotifyAboutChange() => OnChanged?.Invoke(Set);
    }
}