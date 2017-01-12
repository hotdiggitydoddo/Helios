using Helios.Core;
using Microsoft.Xna.Framework;
using PaddleBallBlitz.Components;
using PaddleBallBlitz.Helpers;

namespace PaddleBallBlitz
{
    public class BoxCollider : Collider, IComponent 
    {
        private int _w;
        private int _h;

        public Rectangle Bounds { get; set; }
        public Vector2 Center { get { return new Vector2(Bounds.Center.X, Bounds.Center.Y); } }
        //public int Width
        //{
        //    get { return _w; }
        //    set
        //    {
        //        _w = value;
        //        var r = Bounds;
        //        Bounds = new Rectangle(r.X, r.Y, _w, r.Height);
        //    }
        //}

        //public int Height
        //{
        //    get { return _h; }
        //    set
        //    {
        //        _h = value;
        //        var r = Bounds;
        //        Bounds = new Rectangle(r.X, r.Y, r.Width, _h);
        //    }
        //}
        public int TypeId()
        {
            return (int)ComponentTypes.Collider;
        }

        public BoxCollider(Vector2 position, int width, int height) : base(ColliderType.Box)
        {
            Bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
        }
    }

    public class Intersection
    {
        public float Cx, Cy, Time, Nx, Ny, Ix, Iy;
        public Intersection(float x, float y, float time, float nx, float ny, float ix, float iy)
        {
            Cx = x;
            Cy = y;
            Time = time;
            Nx = nx;
            Ny = ny;
            Ix = ix;
            Iy = iy;
        }
    }

    public class Collision
    {
        public uint Entity { get; set; }
        public uint? CollidedWith { get; set; }
        public ColDir CollisionDirection { get; set; }
        public CollisionTypes CollisionType { get; set; }

        public Collision() { }

        public Collision(uint entityA, uint? entityB, ColDir direction, CollisionTypes type)
        {
            Entity = entityA;
            CollidedWith = entityB;
            CollisionDirection = direction;
            CollisionType = type;
        }
    }

    public enum CollisionTypes
    {
        ScreenLeft,
        ScreenRight,
        ScreenTop,
        ScreenBottom,
        Paddle,
        Ball,
        PowerUp
    }
}
