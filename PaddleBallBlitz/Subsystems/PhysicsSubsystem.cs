using System;
using System.Linq;
using System.Collections.Generic;
using Helios.Core;
using Microsoft.Xna.Framework;

namespace PaddleBallBlitz
{
	public class PhysicsSubsystem : Subsystem
	{

		private List<Moveable> _moveables;

		public PhysicsSubsystem(EntityManager em) : base(em)
		{
			_moveables = new List<Moveable>();
			_bits.SetBit(1);
			_bits.SetBit(2);
		}

		public override void CreateAspect(uint entity, List<IComponent> components)
		{
			var spatial = (Spatial)components.Single(x => x.GetType() == typeof(Spatial));
			var physics = (Physics)components.Single(x => x.GetType() == typeof(Physics));

			_moveables.Add(new Moveable(entity, spatial, physics));
		}

		public override void Update()
		{
			foreach (var m in _moveables)
			{
				m.Spatial.Position += new Vector2(m.Physics.VelX, m.Physics.VelY);
			}
		}
	}
}
