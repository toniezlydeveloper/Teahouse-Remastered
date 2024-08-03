using System;
using System.Collections.Generic;
using System.Linq;
using Internal.Dependencies.Collections;
using UnityEngine;

namespace Internal.Collisions2D.Core
{
    public class CollisionProcessor : MonoBehaviour, ICollectionHolder<ACollidable>
    {
        private List<ICollisionHandler> _collisionHandlers = new();
        private List<ICollidable> _collidables = new();

        private void Start() => InitCollisionHandlers();

        private void Update() => HandleCollision();

        public void Add(ACollidable collidable) => _collidables.Add(collidable);

        public void Remove(ACollidable collidable) => _collidables.Remove(collidable);

        private void HandleCollision()
        {
            List<(ICollidable, ICollidable)> collidablesThatCollided = new();
            
            foreach (ICollidable collidable in _collidables)
            {
                foreach (ICollidable otherCollidable in _collidables)
                {
                    if (collidable == otherCollidable)
                    {
                        continue;
                    }
                    
                    if (collidable.BelongsToPlayer == otherCollidable.BelongsToPlayer)
                    {
                        continue;
                    }
                    
                    Vector3 reposition = collidable.Position - otherCollidable.Position;

                    if (reposition.sqrMagnitude > collidable.Radius * collidable.Radius + otherCollidable.Radius * otherCollidable.Radius)
                    {
                        continue;
                    }
                    
                    collidablesThatCollided.Add((collidable, otherCollidable));
                }
            }
            
            foreach ((ICollidable collidable1, ICollidable collidable2) in collidablesThatCollided)
            {
                ICollisionHandler collisionHandler = _collisionHandlers.FirstOrDefault(handler => handler.CanHandle(collidable1, collidable2));
                collisionHandler?.Handle(collidable1, collidable2);
            }
        }

        private void InitCollisionHandlers()
        {
            IEnumerable<Type> collisionHandlerTypes = typeof(ICollisionHandler).Assembly.GetTypes().Where(IsCollisionHandler);
            
            foreach (Type collisionHandlerType in collisionHandlerTypes)
            {
                _collisionHandlers.Add((ICollisionHandler)Activator.CreateInstance(collisionHandlerType));
            }
        }

        private static bool IsCollisionHandler(Type type)
        {
            if (type.IsAbstract)
            {
                return false;
            }

            if (type.IsInterface)
            {
                return false;
            }

            return type.GetInterfaces().Contains(typeof(ICollisionHandler));
        }
    }
}