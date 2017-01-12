using System;
using Helios.Core;

namespace PaddleBallBlitz
{
	public class Physics : IComponent
	{
		public float Speed { get; set; }
		public float VelX { get; set; }
		public float VelY { get; set; }

        public bool IsMoving { get { return VelX != 0f || VelY != 0f; } }

		public Physics() { }
		public Physics(float speed, float velX, float velY)
		{
			Speed = speed;
			VelX = velX;
			VelY = velY;
		}

		public int TypeId()
		{
			return (int)ComponentTypes.Physics;
		}
	}
}
