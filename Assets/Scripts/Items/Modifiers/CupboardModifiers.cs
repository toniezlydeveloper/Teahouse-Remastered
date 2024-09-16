using System.Linq;
using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class CupboardAnyAnyModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Cupboard;
        
        public bool CanModify(IItemHolder player, IItemHolder place) => player.IsEmpty() != place.IsEmpty();

        public void Modify(IItemHolder player, IItemHolder place) => (player.Value, place.Value) = (place.Value, player.Value);
    }
    
    public class CupboardKettleCupModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Cupboard;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.TryGet(out Kettle kettle))
                return false;
            
            if (!place.TryGet(out Cup cup))
                return false;

            return !cup.Contains<WaterType>() && kettle.Contains<WaterType>();
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            place.CastTo<Cup>().HeldAddIns.AddRange(player.CastTo<Kettle>().HeldAddIns);
            player.CastTo<Kettle>().ResetTemperature();
            player.CastTo<Kettle>().Clear();
            player.Refresh();
            place.Refresh();
        }
        
        public class CupboardAddInCupModifier : IItemModifier
        {
            public ModifierType Type => ModifierType.Cupboard;

            public bool CanModify(IItemHolder player, IItemHolder place)
            {
                if (!player.TryGet(out IAddInGenericType addIn))
                    return false;

                if (!place.TryGet(out Cup cup))
                    return false;

                return cup.HeldAddIns.All(heldAddIn => heldAddIn.GetType() != addIn.GenericType.GetType());
            }

            public void Modify(IItemHolder player, IItemHolder place)
            {
                place.CastTo<Cup>().HeldAddIns.Add(player.CastTo<IAddInGenericType>().GenericType);
                player.Refresh(null);
                place.Refresh();
            }
        }
    }
}