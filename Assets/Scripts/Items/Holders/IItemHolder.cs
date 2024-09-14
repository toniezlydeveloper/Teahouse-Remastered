using System;
using Items.Implementations;

namespace Items.Holders
{
    public interface IItemHolder
    {
        event Action<IItem> OnChanged; 
        
        IItem Value { get; set; }
        string Name { get; }
    }
}