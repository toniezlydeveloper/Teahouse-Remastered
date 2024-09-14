namespace Items.Implementations
{
    public class TeabagJar : ITeabagItem, IItem
    {
        public TeabagType TeabagType { get; set; } = TeabagType.Default;
        
        public string Name => "TeabagJar";
    }
}