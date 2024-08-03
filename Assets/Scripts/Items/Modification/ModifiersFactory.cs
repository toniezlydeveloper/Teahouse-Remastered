using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Items.Holders;
using Items.Modifiers;

namespace Items.Modification
{
    public static class ModifiersFactory
    {
        private static Dictionary<ModifierType, List<IItemModifier>> _itemModifiersByModificationType = new();
        private static bool _isInitialized;
        
        public static void Interact(IItemHolder player, IItemHolder place, ModifierType modifierType)
        {
            Initialize();
            TryInteracting(player, place, modifierType);
        }

        private static void Initialize()
        {
            if (IsInitialized())
                return;

            foreach (Type modifierType in GetModifierTypes())
                AddModifier(modifierType);
        }

        private static void TryInteracting(IItemHolder player, IItemHolder place, ModifierType modifierType)
        {
            if (!_itemModifiersByModificationType.TryGetValue(modifierType, out List<IItemModifier> modifiers))
                return;
            
            IItemModifier modifier = modifiers.FirstOrDefault(modifier => modifier.CanModify(player, place));
            modifier?.Modify(player, place);
        }

        private static bool IsInitialized() => _isInitialized;

        private static void AddModifier(Type modifierType)
        {
            IItemModifier modifier = Activator.CreateInstance(modifierType) as IItemModifier;

            if (_itemModifiersByModificationType.TryGetValue(modifier!.Type, out List<IItemModifier> modifiers))
                modifiers.Add(modifier);
            else
                _itemModifiersByModificationType.Add(modifier.Type, new List<IItemModifier> { modifier });
        }

        private static IEnumerable<Type> GetModifierTypes()
        {
            IEnumerable<Type> allAssemblyTypes = Assembly.GetAssembly(typeof(IItemModifier)).GetTypes();
            IEnumerable<Type> modifierTypes = allAssemblyTypes.Where(type =>
            {
                if (!type.IsClass)
                    return false;

                if (type.IsAbstract)
                    return false;

                return type.GetInterfaces().Contains(typeof(IItemModifier));
            });
            return modifierTypes;
        }
    }
}