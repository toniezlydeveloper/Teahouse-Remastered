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
            cup.HeldAddIns.Clear();
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
            
            return !kettle.Contains<WaterType>();
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            player.CastTo<Kettle>().AddWater();
            player.Refresh();
        }
    }
}