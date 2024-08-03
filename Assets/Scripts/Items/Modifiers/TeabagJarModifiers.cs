using Items.Holders;
using Items.Implementations;

namespace Items.Modifiers
{
    public class TeabagJarTeabagModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.TeabagJar;

        public bool CanModify(IItemHolder player, IItemHolder place) => player.Holds<Teabag>();

        public void Modify(IItemHolder player, IItemHolder place) => player.Refresh(null);
    }

    public class TeabagJarEmptyModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.TeabagJar;

        public bool CanModify(IItemHolder player, IItemHolder place) => player.IsEmpty();

        public void Modify(IItemHolder player, IItemHolder place) => player.Refresh(new Teabag
        {
            TeabagType = place.CastTo<TeabagJar>().TeabagType
        });
    }

    public class TeabagJarCupModifier : IItemModifier
    {
        public ModifierType Type => ModifierType.TeabagJar;
        
        public bool CanModify(IItemHolder player, IItemHolder place)
        {
            if (!player.TryGet(out Cup cup))
                return false;
            
            return cup.TeabagType != TeabagType.None;
        }

        public void Modify(IItemHolder player, IItemHolder place)
        {
            player.CastTo<Cup>().TeabagType = place.CastTo<TeabagJar>().TeabagType;
            player.Refresh();
        }
    }
}