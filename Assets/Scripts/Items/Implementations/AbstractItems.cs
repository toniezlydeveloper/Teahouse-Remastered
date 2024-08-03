namespace Items.Implementations
{
    public interface ITeabagItem
    {
        public TeabagType TeabagType { get; }
    }
    
    public interface IWaterItem
    {
        float WaterTemperature { get; }
        bool HasWater { get; }
    }
}