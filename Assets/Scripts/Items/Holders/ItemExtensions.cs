using System;
using System.Linq;
using Items.Implementations;

namespace Items.Holders
{
    // ReSharper disable once ConvertIfStatementToReturnStatement
    public static class ItemExtensions
    {
        public static bool Contains<TAddIn>(this IAddInsHolder addInsHolder) where TAddIn : Enum => addInsHolder.HeldAddIns.Any(addIn => addIn.GetType() == typeof(TAddIn));
        
        public static bool TryGet<TItem>(this IItemHolder holder, out TItem value)
        {
            if (holder.Value is TItem typedItem)
                value = typedItem;
            else
                value = default;

            return value != null;
        }

        public static bool Holds<TItem>(this IItemHolder holder) => holder.Value is TItem;

        public static void Refresh(this IItemHolder holder, IItem item) => holder.Value = item;

        public static void Refresh(this IItemHolder holder) => holder.Value = holder.Value;

        public static bool IsEmpty(this IItemHolder holder) => holder.Value == null;

        public static TItem CastTo<TItem>(this IItemHolder holder) => (TItem)holder.Value;
    }
}