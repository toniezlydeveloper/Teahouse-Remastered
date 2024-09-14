namespace Items.Implementations
{
    public class Cup : IWaterItem, ITeabagItem, IItem
    {
        public float WaterTemperature { get; set; }
        public TeabagType TeabagType { get; set; }
        public bool HasWater { get; set; }
        public bool IsDirty { get; set; }
        
        public string Name => "Cup";
    }
}