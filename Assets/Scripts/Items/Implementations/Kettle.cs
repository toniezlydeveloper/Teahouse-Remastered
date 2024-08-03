namespace Items.Implementations
{
    public class Kettle : IWaterItem
    {
        public float WaterTemperature { get; set; } = 25f;
        public bool HasWater { get; set; }
    }
}