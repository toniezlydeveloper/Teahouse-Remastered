namespace Internal.Collisions2D.Core
{
    public interface ICollisionHandler
    {
        bool CanHandle(ICollidable collidable1, ICollidable collidable2);
        void Handle(ICollidable collidable1, ICollidable collidable2);
    }
    
    public abstract class ACollisionHandler<TCollidable1, TCollidable2> : ICollisionHandler
    {
        public abstract void Handle(ICollidable collidable1, ICollidable collidable2);

        public bool CanHandle(ICollidable collidable1, ICollidable collidable2)
        {
            if (collidable1 is TCollidable1)
            {
                return collidable2 is TCollidable2;
            }

            if (collidable2 is TCollidable1)
            {
                return collidable1 is TCollidable2;
            }

            return false;
        }

        protected static TCollidable Get<TCollidable>(ICollidable collidable1, ICollidable collidable2) where TCollidable : ICollidable
        {
            if (collidable1 is TCollidable collidable)
            {
                return collidable;
            }

            return (TCollidable)collidable2;
        }
    }
}