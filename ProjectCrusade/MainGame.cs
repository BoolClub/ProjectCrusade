#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

#endregion

namespace ProjectCrusade
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class MainGame : Game
	{

		public const int WindowWidth = 1024;
		public const int WindowHeight = 640;

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		GameScreenManager screenManager;
		TextureManager textureManager;
		FontManager fontManager;
		FrameRateCounter frameCounter;

		public MainGame ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = false;		
			graphics.PreferredBackBufferWidth = WindowWidth;
			graphics.PreferredBackBufferHeight = WindowHeight;
			IsMouseVisible = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			screenManager = new GameScreenManager (new MainGameScreen());

			frameCounter = new FrameRateCounter ();

			base.Initialize ();

		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);


			Texture2D whitePix = new Texture2D (graphics.GraphicsDevice, 1, 1);
			whitePix.SetData (new Color[] { Color.White });
			//Load all textures
			textureManager = new TextureManager (Content, whitePix);
			//Load all fonts
			fontManager = new FontManager (Content);

		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
				Keyboard.GetState ().IsKeyDown (Keys.Escape) || (Keyboard.GetState().IsKeyDown(Keys.Q) &&
					Keyboard.GetState().IsKeyDown(Keys.LeftWindows))) {
				Exit ();
			}
			#endif

			screenManager.Update (gameTime, this);

			frameCounter.Update (gameTime);



			//update previous mouse state
			PlayerInput.PrevMouseState = Mouse.GetState ();
			//update previous keyboard state
			PlayerInput.PrevKeyState = Keyboard.GetState ();

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.Black);
		
			screenManager.Draw (spriteBatch, textureManager, fontManager);
			spriteBatch.Begin ();
			spriteBatch.DrawString (
				fontManager.GetFont ("Arial"), 
				String.Format("ProjectCrusade {1}\nframetime {0} ms", 
					frameCounter.AverageElapsedMilliseconds, 
					System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()),
					new Vector2 (10, WindowHeight - 50), Color.White);
			spriteBatch.End ();


			base.Draw (gameTime);
		}
	}
}

