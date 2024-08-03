using Internal.Collisions2D.Core;
using Internal.Dependencies.Core;
using Internal.Pooling;
using UnityEngine;

namespace Internal.Collisions2D.Projectiles
{
    public class Projectile : ACollidable
    {
        [SerializeField] private string poolName;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float lifetime;
        [SerializeField] private int damage;

        private IPoolsProxy _poolsProxy = DependencyInjector.Get<IPoolsProxy>();

        public string PoolName => poolName;
        public int Damage => damage;

        protected override void OnEnable()
        {
            Invoke(nameof(DestroySelf), lifetime);
            base.OnEnable();
        }

        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(poolName))
            {
                return;
            }

            poolName = name;
        }

        public void Move() => Move(GetMoveDelta());

        public void DestroySelf()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            
            _poolsProxy.Release(poolName, gameObject);
        }

        private void Move(Vector3 delta) => transform.position += delta;

        private Vector3 GetMoveDelta() => Time.deltaTime * moveSpeed * transform.up;
    }
}