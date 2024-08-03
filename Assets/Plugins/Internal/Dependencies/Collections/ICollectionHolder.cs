using Internal.Dependencies.Core;

namespace Internal.Dependencies.Collections
{
    public interface ICollectionHolder<in TItem> : IDependency
    {
        void Add(TItem item);
        void Remove(TItem item);
    }
}