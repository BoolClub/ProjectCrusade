using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrusade
{
	public class InventoryScreen : GameScreen
	{

		World world;
		public InventoryScreen (GameScreenManager screenManager, World _world)
		{
			world = _world;
		}

		public override void Update (GameTime gameTime, GameScreenManager screenManager, MainGame game)
		{
			if (Keyboard.GetState ().IsKeyDown (Keys.I) && PlayerInput.PrevKeyState.IsKeyUp (Keys.I))
				screenManager.PopGameScreen ();

			world.Player.Inventory.Update (gameTime, world);
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager)
		{
			spriteBatch.Begin ();


			world.Player.Inventory.DrawComplete (spriteBatch, textureManager, fontManager);

			spriteBatch.End ();

		}
	}
}

