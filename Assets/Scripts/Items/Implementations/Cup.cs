using System;
using System.Collections.Generic;

namespace Items.Implementations
{
    public class Cup : IWaterItem, IAddInsHolder, IItem
    {
        public List<Enum> HeldAddIns { get; } = new();
        
        public float WaterTemperature { get; set; }
        public bool HasWater { get; set; }
        public bool IsDirty { get; set; }
        
        public string Name => "Cup";
    }
}