using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PaddleBallBlitz
{
	public class TextureAtlas
	{
		private ContentManager _contentManager;
		private Texture2D _texture;
		private SpriteLegend _legend;

		public Texture2D Texture { get { return _texture; } }

		public TextureAtlas(ContentManager contentMgr)
		{
			_contentManager = contentMgr;
		}

		public bool LoadTexture(string assetName)
		{
			try
			{
				_texture = _contentManager.Load<Texture2D>(assetName);
			}
			catch (Exception ex)
			{
				//TODO: log error
				return false;
			}
			return true;
		}

		public bool LoadSpriteLegend(string filePath)
		{
			using (var stream = new StreamReader(filePath))
			{
				var json = stream.ReadToEnd();

				_legend = SpriteLegend.LoadFromJson(json);

				return _legend != null;
			}
		}

		public Rectangle GetSpriteRect(string name)
		{
			return _legend.Indices[name];
		}
	}
}
