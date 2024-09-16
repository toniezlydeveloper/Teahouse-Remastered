using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class CupShelfEmptyModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.CupShelf;

        public bool CanModify(IItemHolder player, IItemHolder place) => player.IsEmpty();

        public void Modify(IItemHolder player, IItemHolder place) => player.Refresh(new Cup());
    }
}