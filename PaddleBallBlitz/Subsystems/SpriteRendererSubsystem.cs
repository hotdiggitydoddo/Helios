using System.Collections.Generic;
using System.Linq;
using Helios.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PaddleBallBlitz
{
	public class SpriteRendererSubsystem : Subsystem
	{
		private readonly SpriteBatch _sb;
		private readonly TextureAtlas _atlas;

		private readonly List<Renderable> _renderables;

		public SpriteRendererSubsystem(EntityManager em, TextureAtlas atlas, SpriteBatch sb) : base(em)
		{
			_atlas = atlas;
			_sb = sb;
			_renderables = new List<Renderable>();
			_bits.SetBit((int)ComponentTypes.Spatial);
			_bits.SetBit((int)ComponentTypes.Sprite);
		}

		public override void CreateAspect(uint entity, List<IComponent> components)
		{
			var spatial = (Spatial)components.Single(x => x.GetType() == typeof(Spatial));
			var sprite = (Sprite)components.Single(x => x.GetType() == typeof(Sprite));
		    sprite.SrcRect = _atlas.GetSpriteRect(sprite.Name);
			_renderables.Add(new Renderable(entity, spatial, sprite));
		}

	    public override bool HasAspect(uint entity)
	    {
	        return _renderables.Exists(x => x.Owner == entity);
        }

	    public void Draw()
		{
			foreach (var r in _renderables)
			{
			    _sb.Draw(
			        _atlas.Texture,
			        new Rectangle((int) r.Spatial.Position.X, (int) r.Spatial.Position.Y, r.Sprite.SrcRect.Width,
			            r.Sprite.SrcRect.Height),
			        _atlas.GetSpriteRect(r.Sprite.Name),
			        r.Sprite.Color,
			        r.Sprite.Rotation,
			        r.Sprite.Origin,
			        SpriteEffects.None,
			        0);
			}
		}
	}
}
