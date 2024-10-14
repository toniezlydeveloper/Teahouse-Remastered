using System;

namespace Items.Selection
{
    public class AddInSelectionData
    {
        public Action<Enum> SelectionCallback { get; set; }
        public Type AddInType { get; set; }
    }
}