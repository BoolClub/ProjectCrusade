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

		public const int WINDOW_WIDTH = 1280;
		public const int WINDOW_HEIGHT = 720;

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		GameScreenManager screenManager;
		TextureManager textureManager;
		FontManager fontManager;


		public MainGame ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = false;		
			graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
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

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
		
			screenManager.Draw (spriteBatch, textureManager, fontManager);
			spriteBatch.Begin ();
			spriteBatch.DrawString (fontManager.GetFont ("Arial"), "ProjectCrusade Alpha", new Vector2 (10, WINDOW_HEIGHT - 50), Color.White);
			spriteBatch.End ();


			base.Draw (gameTime);
		}
	}
}

