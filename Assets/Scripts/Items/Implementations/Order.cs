using System;
using UnityEngine;

namespace Items.Implementations
{
    [Serializable]
    public class Order : IItem
    {
        public bool WasCompleted { get; set; }
        public bool WasCollected { get; set; }
        public bool WasTaken { get; set; }

        public string Name => "Order";
    }
}