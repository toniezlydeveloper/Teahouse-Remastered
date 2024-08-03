using System.Collections.Generic;
using Internal.Dependencies.Collections;
using UnityEngine;

namespace Internal.Collisions2D.Projectiles
{
    public class ProjectilesMover : MonoBehaviour, ICollectionHolder<Projectile>
    {
        private List<Projectile> _projectiles = new();

        private void Update() => HandleMovement();

        public void Add(Projectile projectile) => _projectiles.Add(projectile);

        public void Remove(Projectile projectile) => _projectiles.Remove(projectile);

        private void HandleMovement()
        {
            foreach (Projectile projectile in _projectiles)
            {
                projectile.Move();
            }
        }
    }
}