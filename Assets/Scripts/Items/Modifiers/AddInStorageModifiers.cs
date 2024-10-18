using System;
using Internal.Dependencies.Core;
using Items.Holders;
using Items.Implementations;
using Items.Selection;

namespace Items.Modifiers
{
    public class AddInStorageAddInModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.AddInStorage;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!place.TryGet(out IAddInType storage))
            {
                return false;
            }
            
            if (!player.TryGet(out IAddInType addIn))
            {
                return false;
            }

            return storage.AddInType == addIn.AddInType;
        }

        public void Modify(IItemHolder player, IItemHolder place) => player.Refresh(null);
    }
    
    public class AddInStorageAnyEmptyModifier : IItemModifier
    {
        private IAddInSelector _addInSelector = DependencyInjector.Get<IAddInSelector>();
        
        public ModifierType Type => ModifierType.AddInStorage;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!place.Holds<IAddInType>())
            {
                return false;
            }
                
            return player.IsEmpty();
        }

        public void Modify(IItemHolder player, IItemHolder place) => _addInSelector.Init(new AddInSelectionData
        {
            SelectionCallback = value => Select(player, value),
            AddInType = place.CastTo<IAddInType>().AddInType
        });

        private void Select(IItemHolder player, Enum value)
        {
            _addInSelector.Deinit();
            
            if ((int)(object)value == 0)
            {
                return;
            }
            
            player.Refresh(GetAddIn(value));
        }
        
        private IItem GetAddIn (Enum value) => value switch
        {
            HerbType type => new AddIn<HerbType> { Type = type },
            FlowerType type => new AddIn<FlowerType> { Type = type },
            TeabagType type => new AddIn<TeabagType> { Type = type },
            WaterType type => new AddIn<WaterType> { Type = type },
            _ => null
        };
    }
}