using Internal.Dependencies.Core;
using UnityEngine;

namespace Internal.Pooling
{
    public enum PoolItemType
    {
        Setup,
        Game
    }
    
    public interface IPoolItem : IDependency
    {
        PoolItemType Type { get; }
        
        void TryReleasing();
    }
    
    public class PoolItem : ADependencyElement<IPoolItem>, IPoolItem
    {
        [field:SerializeField] public PoolItemType Type { get; set; }
        
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