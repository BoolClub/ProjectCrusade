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
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Texture2D testSprite;

		Camera camera;

		List<Vector2> things = new List<Vector2>(); //temp

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = false;		
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			IsMouseVisible = true;
			Window.Title = "Better than MS Paint";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
			base.Initialize ();
			testSprite = new Texture2D (graphics.GraphicsDevice, 1, 1);
			testSprite.SetData (new Color[]{ Color.White });

			camera = new Camera ();

		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			//TODO: use this.Content to load your game content here 
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
			    Keyboard.GetState ().IsKeyDown (Keys.Escape)) {
				Exit ();
			}
			#endif

			camera.Rotation = 1.0f*(float)Math.Sin((float)gameTime.TotalGameTime.TotalMilliseconds/1000);
			camera.Scale += 0.001f;

			if (Mouse.GetState ().LeftButton == ButtonState.Pressed)
				things.Add (Vector2.Transform(new Vector2 (Mouse.GetState ().X, Mouse.GetState ().Y),camera.InverseMatrix));

			camera.Update ();



			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
		
			spriteBatch.Begin (SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix);
			spriteBatch.Draw (testSprite, new Rectangle (100, 100, 50, 50), Color.White);

			foreach (Vector2 thing in things)
				spriteBatch.Draw (testSprite, thing, Color.White);

			spriteBatch.End ();

			base.Draw (gameTime);
		}
	}
}

