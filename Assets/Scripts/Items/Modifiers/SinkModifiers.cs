using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class SinkEmptyKettleModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Sink;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.TryGet(out Kettle kettle))
                return false;
            
            return !kettle.Contains<WaterType>();
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            player.CastTo<Kettle>().AddWater();
            player.Refresh();
        }
    }
    
    public class SinkWaterKettleModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Sink;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.TryGet(out Kettle kettle))
                return false;
            
            return kettle.Contains<WaterType>();
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            player.CastTo<Kettle>().ResetTemperature();
            player.CastTo<Kettle>().Clear();
            player.Refresh();
        }
    }
}