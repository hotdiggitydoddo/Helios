using System;
using Helios.Core;
using Microsoft.Xna.Framework;

namespace PaddleBallBlitz
{
	public class Spatial : IComponent
	{
		public Vector2 Position	{ get; set; }

		public Spatial() { }
		public Spatial(float x, float y)
		{
			Position = new Vector2(x, y);
		}

		public int TypeId()
		{
			return 1;
		}
	}
}
