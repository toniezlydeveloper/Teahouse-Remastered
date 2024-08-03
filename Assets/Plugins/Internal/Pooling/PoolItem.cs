using Internal.Dependencies.Core;

namespace Internal.Pooling
{
    public interface IPoolItem : IDependency
    {
        void TryReleasing();
    }
    
    public class PoolItem : ADependencyElement<IPoolItem>, IPoolItem
    {
        private IPoolsProxy _poolsProxy = DependencyInjector.Get<IPoolsProxy>();
        
        public bool ShouldRelease { get; set; }

        public void TryReleasing()
        {
            if (!ShouldRelease)
                return;
            
            _poolsProxy.Release(name.Replace("(Clone)", ""), gameObject);
        }
    }
}