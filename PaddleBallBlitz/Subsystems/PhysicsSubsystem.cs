using System;
using System.Linq;
using System.Collections.Generic;
using Helios.Core;
using Microsoft.Xna.Framework;

namespace PaddleBallBlitz
{
	public class PhysicsSubsystem : Subsystem, ILateUpdateable
	{
		private readonly List<Moveable> _moveables;

		public PhysicsSubsystem(EntityManager em) : base(em)
		{
			_moveables = new List<Moveable>();
			_bits.SetBit((int)ComponentTypes.Spatial);
			_bits.SetBit((int)ComponentTypes.Physics);
		}

		public override void CreateAspect(uint entity, List<IComponent> components)
		{
			var spatial = (Spatial)components.Single(x => x.GetType() == typeof(Spatial));
			var physics = (Physics)components.Single(x => x.GetType() == typeof(Physics));

			_moveables.Add(new Moveable(entity, spatial, physics));
		}

	    public override bool HasAspect(uint entity)
	    {
            return _moveables.Exists(x => x.Owner == entity);
        }

        public void LateUpdate(float dt)
        {
            foreach (var m in _moveables)
            {
                if (!m.Spatial.IsDirty) continue;

                m.Spatial.Position = m.Spatial.FuturePosition;
                m.Spatial.IsDirty = false;
            }
        }

        public override void Update(float dt)
		{
			foreach (var m in _moveables)
			{
                if (!m.Physics.IsMoving)
                    continue;
			    m.Spatial.FuturePosition = m.Spatial.Position +
			                               new Vector2(m.Physics.VelX*m.Physics.Speed*dt, m.Physics.VelY*m.Physics.Speed*dt);
			    m.Spatial.IsDirty = true;
			}
		}
	}
}
