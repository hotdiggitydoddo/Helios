using System;
using Helios.Core;
using Microsoft.Xna.Framework;

namespace PaddleBallBlitz
{
	public class Sprite : IComponent
	{
		public string Name { get; set; }
		public Rectangle SrcRect { get; set; }
		public Color Color { get; set; }
		public Vector2 Origin { get { return new Vector2(SrcRect.Width * .5f, SrcRect.Height * .5f); } }
		public Vector2 Scale { get; set; }
		public float Rotation = 0f;

		public Sprite()
		{
			Color = Color.White;
			Scale = Vector2.One;
		}

		public Sprite(string name)
		{
			Color = Color.White;
			Scale = Vector2.One;
			Name = name;
		}

		public Sprite(string name, Color color) : this(name)
		{
			Color = color;
		}

		public int TypeId()
		{
			return 3;
		}
	}
}
