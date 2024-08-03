using Items.Implementations;

namespace Items.Holders
{
    public class TeabagJarItemHolder : WorldSpaceItemHolder
    {
        public override void ToggleDown()
        {
            var jar = Value as TeabagJar;

            if (jar.TeabagType == TeabagType.Lavender)
                jar.TeabagType = TeabagType.Default;
            
            this.Refresh();
        }

        public override void ToggleUp()
        {
            var jar = Value as TeabagJar;

            if (jar.TeabagType == TeabagType.Default)
                jar.TeabagType = TeabagType.Lavender;
            
            this.Refresh();
        }
    }
}