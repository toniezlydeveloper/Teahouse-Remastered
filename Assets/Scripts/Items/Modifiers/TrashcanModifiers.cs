using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class TrashcanAddInModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Trashcan;

        public bool CanModify(IItemHolder player, IItemHolder place) => player.Holds<IAddInGenericType>();

        public void Modify(IItemHolder player, IItemHolder place) => player.Refresh(null);
    }

    public class TrashcanKettleModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Trashcan;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.TryGet(out Kettle kettle))
            {
                return false;
            }
            
            return kettle.Contains<WaterType>();
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            player.CastTo<Kettle>().ResetTemperature();
            player.CastTo<Kettle>().Clear();
            player.Refresh();
        }
    }
    
    public class TrashcanCupModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.Trashcan;

        public bool CanModify(IItemHolder player, IItemHolder place) => player.Holds<Cup>();

        public void Modify(IItemHolder player, IItemHolder place) => player.CastTo<IAddInsHolder>().HeldAddIns.Clear();
    }
}