namespace Items.Implementations
{
    public class Cup : IWaterItem, ITeabagItem
    {
        public float WaterTemperature { get; set; }
        public TeabagType TeabagType { get; set; }
        public bool HasWater { get; set; }
        public bool IsDirty { get; set; }
    }
}