using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class AddInProcessorEmptyReadyAddInModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.AddInProcessor;

        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!place.TryGet(out IAddInProcessor processor))
            {
                return false;
            }

            if (!player.IsEmpty())
            {
                return false;
            }

            return processor.IsReady();
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            player.Refresh(place.CastTo<IAddInProcessor>().Get());
            place.CastTo<IAddInsHolder>().HeldAddIns.Clear();
            place.CastTo<IAddInProcessor>().Reset();
            place.Refresh();
        }
    }
    
    public class AddInProcessorAddInModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.AddInProcessor;

        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!place.TryGet(out IAddInType processor))
            {
                return false;
            }
            
            if (!player.TryGet(out IAddInType addIn))
            {
                return false;
            }

            return addIn.AddInType == processor.AddInType;
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            place.CastTo<IAddInsHolder>().HeldAddIns.Add(player.CastTo<IAddInGenericType>().GenericType);
            player.Refresh(null);
        }
    }
}