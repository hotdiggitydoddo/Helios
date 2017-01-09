using System.Collections.Generic;
using System.Linq;
using Helios.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PaddleBallBlitz
{
	public class SpriteRendererSubsystem : Subsystem
	{
		private SpriteBatch _sb;
		private TextureAtlas _atlas;

		private List<Renderable> _renderables;

		public SpriteRendererSubsystem(EntityManager em, TextureAtlas atlas, SpriteBatch sb) : base(em)
		{
			_atlas = atlas;
			_sb = sb;
			_renderables = new List<Renderable>();
			_bits.SetBit(1);
			_bits.SetBit(3);
		}


		public override void CreateAspect(uint entity, List<IComponent> components)
		{
			var spatial = (Spatial)components.Single(x => x.GetType() == typeof(Spatial));
			var sprite = (Sprite)components.Single(x => x.GetType() == typeof(Sprite));

			_renderables.Add(new Renderable(entity, spatial, sprite));
		}

		public void Draw()
		{
			foreach (var r in _renderables)
			{
				_sb.Draw(
					_atlas.Texture, 
					new Vector2(r.Spatial.Position.X, r.Spatial.Position.Y), 
					_atlas.GetSpriteRect(r.Sprite.Name), 
					r.Sprite.Color, 
					r.Sprite.Rotation, 
					r.Sprite.Origin, 
					Vector2.One, 
					SpriteEffects.None, 
					0);
			}
		}
	}
}
