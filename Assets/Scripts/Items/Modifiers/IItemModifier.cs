using System;
using Items.Holders;

namespace Items.Modifiers
{
    [Flags]
    public enum ModifierType
    {
        // Next: | 7 | X | X |
        None = 0,
        
        // Level
        Cupboard = 1 << 0,
        Dishwasher = 1 << 1,
        Sink = 1 << 2,
        BoilingPlate = 1 << 3,
        TeabagJar = 1 << 4,
        Customer = 1 << 5,
        AddInStorage = 1 << 6,
        CupShelf = 1 << 7,
        Trashcan = 1 << 8
    }
    
    public interface IItemModifier
    {
        ModifierType Type { get; }

        bool CanModify(IItemHolder player, IItemHolder place);
        void Modify(IItemHolder player, IItemHolder place);
    }
}