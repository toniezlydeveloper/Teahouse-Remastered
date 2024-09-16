using System;
using System.Collections.Generic;

namespace Items.Implementations
{
    public class Kettle : IAddInsHolder, IItem
    {
        public List<Enum> HeldAddIns { get; } = new();
        
        public float WaterTemperature { get; set; }
        
        public string Name => "Kettle";

        public void AddWater() => HeldAddIns.Add(WaterType.Low);

        public void ResetTemperature() => WaterTemperature = 0f;
        
        public void Clear() => HeldAddIns.Clear();

    }
}