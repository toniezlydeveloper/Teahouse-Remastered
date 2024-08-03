using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class DishwasherCupEmptyModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Dishwasher;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!place.IsEmpty())
                return false;

            if (!player.TryGet(out Cup cup))
                return false;
            
            return cup.IsDirty;
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            place.Refresh(player.Value);
            player.Refresh(null);
        }
    }

    public class DishwasherEmptyCupModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Dishwasher;

        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.IsEmpty())
                return false;
            
            return place.Holds<Cup>();
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            player.Refresh(place.Value);
            place.Refresh(null);
        }
    }
}