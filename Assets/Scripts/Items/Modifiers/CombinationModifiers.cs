using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class CombinationTeabagModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Combinator;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.Holds<Teabag>())
                return false;

            if (place.CastTo<CombinationItem>().Item1 != null)
                return place.CastTo<CombinationItem>().Item2 == null;

            return true;
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            place.TryGet(out CombinationItem item);

            if (item.Item1 == null)
                item.Item1 = player.Value;
            else
                item.Item2 = player.Value;
            
            player.Refresh(null);
            place.Refresh();
        }
    }
    
    public class CombinationAnyCombinedModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Combinator;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.IsEmpty())
                return false;

            return place.CastTo<CombinationItem>().IsCombined;
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            player.Refresh(place.CastTo<CombinationItem>().Output);
            place.Refresh(new CombinationItem());
        }
    }
}