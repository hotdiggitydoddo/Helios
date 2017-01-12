using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helios.Core;
using PaddleBallBlitz.Components;
using PaddleBallBlitz.Subsystems;

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
	    private CollisionSubsystem _cs;
	    private BallAISubsystem _bs;
	    private InputSubsystem _is;

        private FrameCounter _frameCounter = new FrameCounter();
	    private SpriteFont _spriteFont;

        public static int SCREEN_WIDTH;
	    public static int SCREEN_HEIGHT;

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

            SCREEN_WIDTH = graphics.GraphicsDevice.Viewport.Width;
            SCREEN_HEIGHT = graphics.GraphicsDevice.Viewport.Height;
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

		    _spriteFont = Content.Load<SpriteFont>("Arial");

            var atlas = new TextureAtlas(Content);
            atlas.LoadTexture("spritesheet");
            atlas.LoadSpriteLegend("atlas.json");
            //atlas.LoadTexture("4square");
            //atlas.LoadSpriteLegend("atlas.json");

            _ss = new SpriteRendererSubsystem(_em, atlas, spriteBatch);
			_em.RegisterSubsystem(_ss);

			_ps = new PhysicsSubsystem(_em);
			_em.RegisterSubsystem(_ps);

            _cs = new CollisionSubsystem(_em);
            _em.RegisterSubsystem(_cs);

            _bs = new BallAISubsystem(_em);
            _em.RegisterSubsystem(_bs);

            _is = new InputSubsystem(_em);
            _em.RegisterSubsystem(_is);

			var ball = _em.CreateEntity();
			var ballSprite = new Sprite("Ball");
			var ballSpatial = new Spatial(100, 150);
			_em.AddComponent(ball, ballSpatial);
            _em.AddComponent(ball, ballSprite);
			_em.AddComponent(ball, new Physics(300, 1, 1));
		    _em.AddComponent(ball,
		        new CircleCollider(ballSpatial.Position, ballSprite.SrcRect.Height/2));
            _em.AddComponent(ball, new BallAI());

			var paddle = _em.CreateEntity();
            var paddleSprite = new Sprite("Paddle");
            var paddleSpatial = new Spatial(300, 300);
            _em.AddComponent(paddle, paddleSpatial);
            _em.AddComponent(paddle, paddleSprite);
            _em.AddComponent(paddle, new Physics(350, 0, 0));
            _em.AddComponent(paddle, new Input());
		    _em.AddComponent(paddle,
		        new BoxCollider(paddleSpatial.Position, paddleSprite.SrcRect.Width, paddleSprite.SrcRect.Height));
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
		    var dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
#endif
			// TODO: Add your update logic here
            _is.Update(dt);
			_ps.Update(dt);
            _cs.Update(dt);
            _bs.Update(dt);

            _ps.LateUpdate(dt);
            _cs.LateUpdate(dt);
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.Black);

            //TODO: Add your drawing code here
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameCounter.Update(deltaTime);

            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);



            spriteBatch.Begin();
			_ss.Draw();
            spriteBatch.DrawString(_spriteFont, fps, new Vector2(1, 1), Color.White);

            spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
