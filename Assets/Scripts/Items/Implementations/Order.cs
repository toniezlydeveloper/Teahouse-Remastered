using System;
using UnityEngine;

namespace Items.Implementations
{
    [Serializable]
    public class Order : IItem
    {
        [field:SerializeField] public float MinWaterTemperature { get; set; }
        [field:SerializeField] public float MaxWaterTemperature { get; set; }
        
        public bool WasCompleted { get; set; }
        public bool WasCollected { get; set; }
        public bool WasTaken { get; set; }

        public string Name => "Order";
    }
}