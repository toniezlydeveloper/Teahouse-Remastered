using System;
using System.Collections.Generic;

namespace Items.Implementations
{
    public class Order : IAddInsHolder, IItem
    {
        public List<Enum> HeldAddIns { get; } = new();
        
        public bool WasCompleted { get; set; }
        public bool WasCollected { get; set; }
        public bool WasTaken { get; set; }

        public string Name => "Customer";
    }
}