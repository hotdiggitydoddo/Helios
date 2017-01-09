using System;
using Helios.Core;

namespace PaddleBallBlitz
{
	public class Moveable : Aspect
	{
		public readonly Spatial Spatial;
		public readonly Physics Physics;

		public Moveable(uint entity, Spatial spatial, Physics physics) : base(entity)
		{
			Spatial = spatial;
			Physics = physics;
		}
	}
}
