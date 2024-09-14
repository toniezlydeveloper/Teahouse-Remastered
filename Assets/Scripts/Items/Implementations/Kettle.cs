namespace Items.Implementations
{
    public class Kettle : IWaterItem, IItem
    {
        public float WaterTemperature { get; set; } = 25f;
        public bool HasWater { get; set; }
        
        public string Name => "Kettle";
    }
}