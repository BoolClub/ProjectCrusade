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
		MainGame game;

		GameScreenManager screenManager;



		TextureManager textureManager;

		public MainGameScreen (TextureManager _textureManager)
		{
			camera = new Camera ();
			objManager = new ObjectiveManager ();
<<<<<<< HEAD
			world = new World (_textureManager, 128, 128, objManager, levelConfigurations[currWorld]);
			prevKeyboardState = Keyboard.GetState ();
			textureManager = _textureManager;
=======
			world = new World (textureManager, 128, 128, objManager);
			prevKeyboardState = Keyboard.GetState ();
>>>>>>> parent of b42776f... World progressions, fixed tile mismatch
			hud = new HUDManager (world);
		}
		public override void Update (GameTime gameTime, GameScreenManager screenManager, MainGame game)
		{
			this.game = game;
			this.screenManager = screenManager;
			world.Update (gameTime, camera);
			cameraFollow ();
			camera.Update ();
			hud.Update (gameTime);
			objManager.Update (gameTime, world.Player, world);


			if (world.ReadyToAdvance) {
				NextWorld (textureManager);
			}


			if (Keyboard.GetState ().IsKeyDown (Keys.P) && prevKeyboardState.IsKeyUp(Keys.P))
				screenManager.PushGameScreen (new PauseMenuScreen (screenManager, game), 100);
			if (Keyboard.GetState ().IsKeyDown (Keys.I) && prevKeyboardState.IsKeyUp (Keys.I)) {
				screenManager.PushGameScreen (new InventoryScreen (screenManager, world));
			}
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
<<<<<<< HEAD
=======
			if (world.ReadyToAdvance)
				world = new World (textureManager, 128, 128, objManager);
>>>>>>> parent of b42776f... World progressions, fixed tile mismatch
			//Render world (do transform)
			spriteBatch.Begin (SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, camera.TransformMatrix);

			world.Draw (spriteBatch, textureManager, fontManager, camera);

			spriteBatch.End ();


			//Render inventory (do not transform)
			spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, null);

			world.Player.Inventory.DrawPartial (spriteBatch, textureManager, fontManager);

			//Draw the hud
			hud.Draw (spriteBatch, textureManager, fontManager);

			string text = String.Format ("Sanity: {0}", world.Player.Sanity);

			SpriteFont font = fontManager.GetFont ("MainFontLarge");

			spriteBatch.DrawString (font, text, new Vector2 (MainGame.WindowWidth - 10, MainGame.WindowHeight - 50) - font.MeasureString (text), Color.White);

			spriteBatch.End ();
		}

		public void NextWorld(TextureManager textureManager)
		{
			currWorld++;
			if (currWorld >= levelConfigurations.Count)
				currWorld -= levelConfigurations.Count;	
			world = new World (textureManager, 128, 128, objManager, levelConfigurations[currWorld]);
			screenManager.PushGameScreen (new WorldTransitionScreen(screenManager, game), 100);
		}





	} //END OF MAINGAMESCREEN CLASS
} //END OF NAMESPACE

