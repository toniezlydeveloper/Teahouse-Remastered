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

            return !cup.HasWater && kettle.HasWater;
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            place.CastTo<Cup>().WaterTemperature = player.CastTo<Kettle>().WaterTemperature;
            player.CastTo<Kettle>().WaterTemperature = 25f;
            player.CastTo<Kettle>().HasWater = false;
            place.CastTo<Cup>().HasWater = true;
            player.Refresh();
            place.Refresh();
        }
        
        public class CupboardTeabagCupModifier : IItemModifier
        {
            public ModifierType Type => ModifierType.Cupboard;

            public bool CanModify(IItemHolder player, IItemHolder place)
            {
                if (!player.Holds<Teabag>())
                    return false;

                if (!place.TryGet(out Cup cup))
                    return false;
                
                return cup.TeabagType == TeabagType.None;
            }

            public void Modify(IItemHolder player, IItemHolder place)
            {
                place.CastTo<Cup>().TeabagType = player.CastTo<Teabag>().TeabagType;
                player.Refresh(null);
                place.Refresh();
            }
        }
    }
}