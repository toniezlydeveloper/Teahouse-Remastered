using Internal.Dependencies.Collections;
using UnityEngine;

namespace Internal.Collisions2D.Core
{
    public interface ICollidable
    {
        bool BelongsToPlayer { get; }
        Vector3 Position { get; }
        float Radius { get; }
    }

    public abstract class ACollidable : ACollectionItem<ACollidable>, ICollidable
    {
        [field:SerializeField] public bool BelongsToPlayer { get; set; }
        [field:SerializeField] public float Radius { get; set; }
        
        public Vector3 Position => transform.position;

        private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, Radius);
    }
}