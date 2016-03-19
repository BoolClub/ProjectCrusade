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
		HUDManager hud;
		ObjectiveManager objManager;


		public MainGameScreen (TextureManager textureManager)
		{
			camera = new Camera ();
			objManager = new ObjectiveManager ();
			world = new World (textureManager, 128, 128, objManager);
			prevKeyboardState = Keyboard.GetState ();
			hud = new HUDManager (world);
		}
		public override void Update (GameTime gameTime, GameScreenManager screenManager, MainGame game)
		{
			world.Update (gameTime, camera);
			cameraFollow ();
			camera.Update ();
			hud.Update (gameTime);
			objManager.Update (gameTime, world.Player, world);


			if (Keyboard.GetState ().IsKeyDown (Keys.P) && prevKeyboardState.IsKeyUp(Keys.P))
				screenManager.PushGameScreen (new PauseMenuScreen (screenManager, game), 100);
			if (Keyboard.GetState ().IsKeyDown (Keys.I) && prevKeyboardState.IsKeyUp(Keys.I))
				screenManager.PushGameScreen (new InventoryScreen (screenManager, world));

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
			const float speed = 0.15f;

			//Expontial following
			camera.Position+= (world.Player.Position - new Vector2(MainGame.WindowWidth / 2, MainGame.WindowHeight / 2) / camera.Scale - camera.Position / camera.Scale) * speed;
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, float opacity)
		{
			//Render world (do transform)
			spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, camera.TransformMatrix);

			world.Draw (spriteBatch, textureManager, fontManager, camera);

			spriteBatch.End ();

			//Render inventory (do not transform)
			spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);

			world.Player.Inventory.DrawPartial (spriteBatch, textureManager, fontManager);

			//Draw the hud
			hud.Draw (spriteBatch, textureManager, fontManager);

			string text = String.Format ("Sanity: {0}", world.Player.Sanity);

			spriteBatch.DrawString (fontManager.GetFont ("Arial"), text, new Vector2 (MainGame.WindowWidth - 10, MainGame.WindowHeight - 50) - fontManager.GetFont ("Arial").MeasureString (text), Color.White);

			spriteBatch.End ();
		}





	} //END OF MAINGAMESCREEN CLASS
} //END OF NAMESPACE

