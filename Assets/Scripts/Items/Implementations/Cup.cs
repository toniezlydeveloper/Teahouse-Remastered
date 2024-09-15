using System;
using System.Collections.Generic;

namespace Items.Implementations
{
    public class Cup : IAddInsHolder, IItem
    {
        public List<Enum> HeldAddIns { get; } = new();
        
        public bool IsDirty { get; set; }
        
        public string Name => "Cup";
    }
}