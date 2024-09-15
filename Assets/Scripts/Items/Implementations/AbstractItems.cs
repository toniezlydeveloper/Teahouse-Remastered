using System;
using System.Collections.Generic;

namespace Items.Implementations
{
    public interface IWaterItem
    {
        float WaterTemperature { get; }
        bool HasWater { get; }
    }

    public interface IAddInsHolder
    {
        List<Enum> HeldAddIns { get; }
    }
}