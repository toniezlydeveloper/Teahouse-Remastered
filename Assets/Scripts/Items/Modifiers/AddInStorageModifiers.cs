using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class TeabagStorageTeabagModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.AddInStorage;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.TryGet(out AddIn<TeabagType> addIn))
            {
                return false;
            }
            
            if (!place.TryGet(out IAddInStorage storage))
            {
                return false;
            }

            return storage is IAddInType<TeabagType>;
        }

        public void Modify(IItemHolder player, IItemHolder place) => player.Refresh(null);
    }
    
    public class AddInStorageAnyEmptyModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.AddInStorage;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!place.TryGet(out IAddInStorage storage))
            {
                return false;
            }

            if (!storage.CanGet())
            {
                return false;
            }
                
            return player.IsEmpty();
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            player.Refresh(place.CastTo<IAddInStorage>().Get());
            place.CastTo<IAddInStorage>().Reset();
        }
    }
}