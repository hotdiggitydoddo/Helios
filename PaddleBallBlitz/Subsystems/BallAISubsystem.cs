using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.Core;
using Microsoft.Xna.Framework;
using PaddleBallBlitz.Components;
using PaddleBallBlitz.Helpers;
using BallAI = PaddleBallBlitz.Aspects.BallAI;

namespace PaddleBallBlitz.Subsystems
{
    public class BallAISubsystem : Subsystem
    {
        private readonly List<BallAI> _ballAIs;

        public BallAISubsystem(EntityManager em) : base(em)
        {
            _ballAIs = new List<BallAI>();
            _bits.SetBit((int)ComponentTypes.Spatial);
            _bits.SetBit((int)ComponentTypes.Physics);
            _bits.SetBit((int)ComponentTypes.BallAI);
            _bits.SetBit((int)ComponentTypes.Collider);
        }

        public override void CreateAspect(uint entity, List<IComponent> components)
        {
            var spatial = (Spatial)components.Single(x => x.GetType() == typeof(Spatial));
            var collider = (CircleCollider)components.Single(x => x.GetType() == typeof(CircleCollider));
            var physics = (Physics)components.Single(x => x.GetType() == typeof(Physics));
            var sprite = (Sprite)components.Single(x => x.GetType() == typeof(Sprite));
            _ballAIs.Add(new BallAI(entity, spatial, physics, collider, sprite));
        }

        public override bool HasAspect(uint entity)
        {
            return _ballAIs.Exists(x => x.Owner == entity);
        }

        public override void Update(float dt)
        {
            foreach (var b in _ballAIs)
            {
                //Keep ball on screen -- for now
                if (b.Collider.Bounds.Center.X - b.Collider.Bounds.Radius <= 0 || b.Collider.Bounds.Center.X + b.Collider.Bounds.Radius >= PaddleBallBlitz.SCREEN_WIDTH)
                    b.Physics.VelX *= -1;
                if (b.Collider.Bounds.Center.Y - b.Collider.Bounds.Radius <= 0 || b.Collider.Bounds.Center.Y + b.Collider.Bounds.Radius >= PaddleBallBlitz.SCREEN_HEIGHT)
                    b.Physics.VelY *= -1;

                if (b.Collider.Intersections.Any())
                {
                    var inter = b.Collider.Intersections[0];
                    var posChange = b.Spatial.FuturePosition - b.Spatial.Position;

                    var remainingTime = 1.0f - inter.Time;
                    var dot = posChange.X * inter.Nx + posChange.Y * inter.Ny;
                    var ndx = posChange.X - 2 * dot * inter.Nx;
                    var ndy = posChange.Y - 2 * dot * inter.Ny;
                    var newx = inter.Cx + ndx * remainingTime;
                    var newy = inter.Cy + ndy * remainingTime;

                    b.Spatial.FuturePosition = new Vector2(newx, newy);

                    if (inter.Nx != 0)
                        b.Physics.VelX = inter.Nx;
                    if (inter.Ny != 0)
                        b.Physics.VelY = inter.Ny;
                }
            }
            base.Update(dt);
        }
    }
}
