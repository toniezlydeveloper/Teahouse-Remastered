using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class AddInStorageModifiers
    {
        public class AddInStorageEmptyModifier : IItemModifier
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
}