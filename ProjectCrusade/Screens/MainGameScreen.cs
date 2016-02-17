using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrusade
{
	/// <summary>
	/// Main game screen. This is where all game logic takes place (most in the World.Update() method).
	/// </summary>
	public class MainGameScreen : GameScreen
	{
		Camera camera;
		World world;
		KeyboardState prevKeyboardState;

		public MainGameScreen ()
		{
			camera = new Camera ();
			world = new World (16, 16);
			prevKeyboardState = Keyboard.GetState ();
		}
		public override void Update (GameTime gameTime, GameScreenManager screenManager)
		{
			world.Update (gameTime);
			cameraFollow ();
			camera.Update ();

			if (Keyboard.GetState ().IsKeyDown (Keys.P) && prevKeyboardState.IsKeyUp(Keys.P))
				screenManager.PushGameScreen (new PauseMenuScreen ());
			prevKeyboardState = Keyboard.GetState ();
		}

		/// <summary>
		/// Camera smoothly follows player.
		/// </summary>
		private void cameraFollow() 
		{
			//In range (0,1]
			//0: no movement
			//1: perfect tracking
			const float speed = 0.05f;

			//Expontial following
			camera.Position+= (world.GetPlayerPosition () - new Vector2(MainGame.WINDOW_WIDTH / 2, MainGame.WINDOW_HEIGHT / 2) - camera.Position) * speed;
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager)
		{

			//Render world (do transform)
			spriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, camera.TransformMatrix);

			world.Draw (spriteBatch, textureManager);
			spriteBatch.Draw (textureManager.GetTexture ("circle"), new Rectangle(100,100,100,100), Color.White);
			spriteBatch.End ();


			//Render inventory (do not transform)
			spriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);

			world.PlayerInventory.Draw (spriteBatch, textureManager);

			spriteBatch.End ();
		}
	}
}

