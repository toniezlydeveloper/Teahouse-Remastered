using System;

namespace Items.Holders
{
    public interface IItemHolder
    {
        event Action<object> OnChanged; 
        
        object Value { get; set; }
    }
}