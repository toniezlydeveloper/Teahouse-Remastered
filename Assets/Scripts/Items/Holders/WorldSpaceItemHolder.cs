using System;
using Internal.Dependencies.Core;
using Items.Implementations;
using Items.Modifiers;
using UnityEngine;

namespace Items.Holders
{
    public enum InitialItemType
    {
        None,
        Kettle,
        Cup,
        TeabagJar
    }

    public interface IManageableItemHolder : IDependency
    {
        void SetInitial();
        void Destroy();
    }
    
    public class WorldSpaceItemHolder : ADependencyElement<IManageableItemHolder>, IItemHolder, IManageableItemHolder
    {
        public event Action<object> OnChanged;
        
        [field:SerializeField] public ModifierType ModifierType { get; set; }
        [field:SerializeField] public InitialItemType InitialItemType { get; set; }

        private object _value;
        
        public object Value
        {
            get => GetItem();
            set => SetItem(value);
        }
        
        public void SetInitial()
        {
            if (!TryGetInitialItem(out object initialItem))
                return;
            
            SetItem(initialItem);
        }

        public void Destroy() => Value = null;

        public virtual void ToggleDown()
        {
        }

        public virtual void ToggleUp()
        {
        }

        public virtual void Progress()
        {
        }
        
        protected virtual bool TryGetInitialItem(out object initialItem)
        {
            initialItem = InitialItemType switch
            {
                InitialItemType.None => null,
                InitialItemType.Kettle => new Kettle(),
                InitialItemType.Cup => new Cup(),
                InitialItemType.TeabagJar => new TeabagJar(),
                _ => throw new ArgumentOutOfRangeException()
            };

            return initialItem != null;
        }

        private object GetItem() => _value;

        private void SetItem(object value)
        {
            OverrideItem(value);
            NotifyAboutChange();
        }

        private void OverrideItem(object value) => _value = value;

        private void NotifyAboutChange() => OnChanged?.Invoke(_value);
    }
}