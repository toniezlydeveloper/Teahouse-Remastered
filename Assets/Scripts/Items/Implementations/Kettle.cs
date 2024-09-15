using System;
using System.Collections.Generic;

namespace Items.Implementations
{
    public class Kettle : IAddInsHolder, IItem
    {
        public List<Enum> HeldAddIns { get; } = new();
        
        public float WaterTemperature { get; set; }
        
        public string Name => "Kettle";

        public void AddWater() => HeldAddIns.Add(WaterTemperature switch
        {
            > 75f => WaterType.Hot,
            > 50f => WaterType.Medium,
            _ => WaterType.Low
        });

        public void Refresh() => HeldAddIns[0] = WaterTemperature switch
        {
            > 75f => WaterType.Hot,
            > 50f => WaterType.Medium,
            _ => WaterType.Low
        };

        public void Clear() => HeldAddIns.Clear();
    }
}