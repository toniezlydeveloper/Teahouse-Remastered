namespace Items.Implementations
{
    public enum TeabagType
    {
        None,
        Default,
        Lavender
    }
    
    public class Teabag : ITeabagItem, IItem
    {
        public TeabagType TeabagType { get; set; }
        
        public string Name => "Teabag";
    }
}