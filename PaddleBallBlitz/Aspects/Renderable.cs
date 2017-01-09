using System;
using Helios.Core;

namespace PaddleBallBlitz
{
	public class Renderable : Aspect
	{
		public readonly Spatial Spatial;
		public readonly Sprite Sprite;

		public Renderable(uint entity, Spatial spatial, Sprite sprite) : base(entity)
		{
			Spatial = spatial;
			Sprite = sprite;
		}
	}
}
