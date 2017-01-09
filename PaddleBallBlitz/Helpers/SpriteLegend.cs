using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PaddleBallBlitz
{
	public class SpriteLegend
	{
		public Dictionary<string, Rectangle> Indices { get; set; }

		public string Name { get; set; }

		public SpriteLegend()
		{
			Indices = new Dictionary<string, Rectangle>();
		}

		public static SpriteLegend LoadFromJson(string json)
		{
			return JsonConvert.DeserializeObject<SpriteLegend>(json);
		}
	}
}
