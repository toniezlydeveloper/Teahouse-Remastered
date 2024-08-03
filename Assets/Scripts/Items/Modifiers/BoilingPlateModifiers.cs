using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class BoilingPlateKettleEmptyModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.BoilingPlate;
        
        public bool CanModify(IItemHolder player, IItemHolder place) => place.IsEmpty() && player.Holds<Kettle>();

        public void Modify(IItemHolder player, IItemHolder place)
        {
            place.Refresh(player.Value);
            player.Refresh(null);
        }
    }
    
    public class BoilingPlateEmptyKettleModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.BoilingPlate;
        
        public bool CanModify(IItemHolder player, IItemHolder place) => player.IsEmpty() && place.Holds<Kettle>();

        public void Modify(IItemHolder player, IItemHolder place)
        {
            player.Refresh(place.Value);
            place.Refresh(null);
        }
    }
}