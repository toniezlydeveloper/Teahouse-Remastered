using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class SinkCupModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Sink;

        public bool CanModify(IItemHolder player, IItemHolder place) => player.Holds<Cup>();

        public void Modify(IItemHolder player, IItemHolder place)
        {
            player.TryGet(out Cup cup);
            
            cup.TeabagType = TeabagType.None;
            cup.WaterTemperature = 25f;
            cup.HasWater = false;
            
            player.Refresh();
        }
    }

    public class SinkKettleModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Sink;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.TryGet(out Kettle kettle))
                return false;
            
            return !kettle.HasWater;
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            player.CastTo<Kettle>().HasWater = true;
            player.Refresh();
        }
    }
}