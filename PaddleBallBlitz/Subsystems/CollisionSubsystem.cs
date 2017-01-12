using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Helios.Core;
using Microsoft.Xna.Framework;
using PaddleBallBlitz.Components;
using PaddleBallBlitz.Helpers;

namespace PaddleBallBlitz
{
    public class CollisionSubsystem : Subsystem, ILateUpdateable
    {
        private readonly List<Collidable> _collidables;

        public CollisionSubsystem(EntityManager em) : base(em)
        {
            _collidables = new List<Collidable>();
            _bits.SetBit((int)ComponentTypes.Spatial);
            _bits.SetBit((int)ComponentTypes.Collider);
        }

        public override void CreateAspect(uint entity, List<IComponent> components)
        {
            var spatial = (Spatial)components.Single(x => x.GetType() == typeof(Spatial));
            var collider = (Collider)components.Single(x => (x.GetType() == typeof(BoxCollider)) || x.GetType() ==  typeof(CircleCollider));

            _collidables.Add(new Collidable(entity, spatial, collider));
        }

        public override bool HasAspect(uint entity)
        {
            return _collidables.Exists(x => x.Owner == entity);
        }

        public void LateUpdate(float dt)
        {
            foreach (var c in _collidables.Where(x => x.Collider.Collisions.Any()))
                c.Collider.Collisions.Clear();

            foreach (var c in _collidables.Where(x => x.Collider.Intersections.Any()))
                c.Collider.Intersections.Clear();
        }

        public override void Update(float dt)
        {
            foreach (var c in _collidables)
            {
                //check collider type and cast
                switch (c.Collider.Type)
                {
                    case ColliderType.Circle:
                        var circleCollider = c.Collider as CircleCollider;
                        CheckForCollisions(c, circleCollider);
                        break;

                    case ColliderType.Box:
                        var boxCollider = c.Collider as BoxCollider;
                        CheckForCollisions(c, boxCollider);
                        break;
                }
            }
        }

        private void CheckForCollisions(Collidable c, BoxCollider cCollider)
        {
            if (c.Spatial.IsDirty)
            {
                var oldBounds = cCollider.Bounds;
                cCollider.Bounds = new Rectangle((int)c.Spatial.FuturePosition.X, (int)c.Spatial.FuturePosition.Y, oldBounds.Width, oldBounds.Height);
            }

            var cRect = cCollider.Bounds;

            foreach (var otherC in _collidables)
            {
                if (otherC == c)
                    continue;

                switch (otherC.Collider.Type)
                {
                    case ColliderType.Circle:
                        var otherCircle = ((CircleCollider) otherC.Collider).Bounds;
                        var collisionDir = CollisionHelper.Intersects(otherCircle, cRect);

                        if (collisionDir.HasValue)
                        {
                            c.Collider.Collisions.Add(new Collision
                            {
                                CollidedWith = otherC.Owner,
                                CollisionDirection = collisionDir.Value
                            });
                        }
                        break;

                    case ColliderType.Box:
                        var otherRect = ((BoxCollider)otherC.Collider).Bounds;
                        if (cRect.Intersects(otherRect))
                        {
                            //got an intersection, now determine from what direction!
                            var w = 0.5f * (cRect.Width + otherRect.Width);
                            var h = 0.5f * (cRect.Height + otherRect.Height);
                            var dx = (c.Spatial.Position.X + (cRect.Width / 2)) -
                                     (otherC.Spatial.Position.X + (otherRect.Width / 2));
                            var dy = (c.Spatial.Position.Y + (cRect.Height / 2)) -
                                     (otherC.Spatial.Position.Y + (otherRect.Height / 2));

                            ColDir colDir;
                          
                            var wy = w * dy;
                            var hx = h * dx;

                            if (wy > hx)
                            {
                                if (wy > -hx)
                                {
                                    // top
                                    colDir = ColDir.Top;
                                }
                                else
                                {
                                    /* on the left */
                                    colDir = ColDir.Left;
                                }
                            }
                            else if (wy > -hx)
                            {
                                /* on the right */
                                colDir = ColDir.Right;
                            }
                            else
                            {
                                /* at the bottom */
                                colDir = ColDir.Bottom;
                            }
                            c.Collider.Collisions.Add(new Collision
                            {
                                CollidedWith = otherC.Owner,
                                CollisionDirection = colDir
                            });
                        }
                        break;
                }
            }
        }



        private void CheckForCollisions(Collidable c, CircleCollider cCollider)
        {
            if (c.Spatial.IsDirty)
                cCollider.Bounds =
                    new Circle(c.Spatial.FuturePosition, cCollider.Radius);

            var cCirc = cCollider.Bounds;

            foreach (var otherC in _collidables)
            {
                if (otherC == c)
                    continue;

                switch (otherC.Collider.Type)
                {
                    case ColliderType.Circle:
                        //var otherCircle = ((CircleCollider)otherC.Collider).Bounds;
                        //var collisionDir = CollisionHelper.Intersects(otherCircle, cRect);

                        //if (collisionDir.HasValue)
                        //{
                        //    c.Collider.Collisions.Add(new Collision
                        //    {
                        //        CollidedWith = otherC.Owner,
                        //        CollisionDirection = collisionDir.Value
                        //    });
                        //}
                        break;

                    case ColliderType.Box:
                        var otherRect = ((BoxCollider)otherC.Collider).Bounds;

                        var posChange = c.Spatial.FuturePosition - c.Spatial.Position;
                        var oldPos = cCirc.Center - posChange;

                        var inter = CollisionHelper.CircleIntersectsRect(oldPos, cCirc.Center, 
                            cCirc.Radius, otherRect);

                        if (inter != null)
                        {
                            c.Collider.Intersections.Add(inter);
                            //float remainingTime = 1.0f - inter.Time;
                            //float dot = posChange.X * inter.Nx + posChange.Y * inter.Ny;
                            //float ndx = posChange.X - 2 * dot * inter.Nx;
                            //float ndy = posChange.Y - 2 * dot * inter.Ny;
                            //float newx = inter.Cx + ndx * remainingTime;
                            //float newy = inter.Cy + ndy * remainingTime;
                        }

                        //var collisionDir = CollisionHelper.Intersects(cCirc, otherRect);
                        //if (collisionDir.HasValue)
                        //{
                        //    c.Collider.Collisions.Add(new Collision
                        //    {
                        //        CollidedWith = otherC.Owner,
                        //        CollisionDirection = collisionDir.Value
                        //    });
                        //}
                        break;
                }
            }

        }
    }
}
