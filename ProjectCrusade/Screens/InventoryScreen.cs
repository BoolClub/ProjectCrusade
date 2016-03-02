using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrusade
{
	public class InventoryScreen : GameScreen
	{
		public const float Opacity = 0.5f;
		World world;
		public InventoryScreen (GameScreenManager screenManager, World _world)
		{
			world = _world;
			world.Player.Inventory.Open = true;
		}

		public override void Update (GameTime gameTime, GameScreenManager screenManager, MainGame game)
		{
			if (Keyboard.GetState ().IsKeyDown (Keys.I) && PlayerInput.PrevKeyState.IsKeyUp (Keys.I)) {
				screenManager.PopGameScreen ();
				world.Player.Inventory.Open = false;
			}

			world.Player.Inventory.Update (gameTime, world);
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, float drawOpacity)
		{
			spriteBatch.Begin ();

			spriteBatch.Draw (
				textureManager.WhitePixel, 
				new Rectangle (0, 0, MainGame.WindowWidth, MainGame.WindowHeight), 
				Color.Black * Opacity);
			
			world.Player.Inventory.DrawComplete (spriteBatch, textureManager, fontManager);

			spriteBatch.End ();

		}
	}
}

