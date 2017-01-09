﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helios.Core;

namespace PaddleBallBlitz
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class PaddleBallBlitz : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		EntityManager _em;
		SpriteRendererSubsystem _ss;
		PhysicsSubsystem _ps;

		public PaddleBallBlitz()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			_em = new EntityManager();
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			//TODO: use this.Content to load your game content here

			var atlas = new TextureAtlas(Content);
			atlas.LoadTexture("4square");
			atlas.LoadSpriteLegend("atlas.json");

			_ss = new SpriteRendererSubsystem(_em, atlas, spriteBatch);
			_em.RegisterSubsystem(_ss);

			_ps = new PhysicsSubsystem(_em);
			_em.RegisterSubsystem(_ps);

			var e = _em.CreateEntity();

			var sprite = new Sprite("Red");
			var spatial = new Spatial(100, 150);
			_em.AddComponent(e, sprite);
			_em.AddComponent(e, spatial);
			_em.AddComponent(e, new Physics(1, 1, 1));

			//needs to be a subset of when registering entities with subsystems!

			var e1 = _em.CreateEntity();

			_em.AddComponent(e1, new Sprite("Blue"));
			_em.AddComponent(e1, new Spatial(300, 300));

		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
#endif

			// TODO: Add your update logic here

			_ps.Update();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			//TODO: Add your drawing code here
			spriteBatch.Begin();
			_ss.Draw();
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}