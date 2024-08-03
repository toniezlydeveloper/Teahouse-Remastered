namespace Items.Implementations
{
    public enum TeabagType
    {
        None,
        Default,
        Lavender
    }
    
    public class Teabag : ITeabagItem
    {
        public TeabagType TeabagType { get; set; }
    }
}