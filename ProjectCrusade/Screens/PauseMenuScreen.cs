using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrusade
{
	public class PauseMenuScreen : GameScreen
	{
		KeyboardState prevKeyboardState;

		public PauseMenuScreen ()
		{
			prevKeyboardState = Keyboard.GetState ();
		}
		public override void Update (GameTime gameTime, GameScreenManager screenManager)
		{
			//unpause
			if (Keyboard.GetState().IsKeyDown(Keys.P) && prevKeyboardState.IsKeyUp(Keys.P)) screenManager.PopGameScreen ();

			prevKeyboardState = Keyboard.GetState ();
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager)
		{
			spriteBatch.Begin ();

			spriteBatch.DrawString (fontManager.GetFont ("Arial"), "PAUSED", (new Vector2(MainGame.WINDOW_WIDTH*0.5f, MainGame.WINDOW_HEIGHT*0.5f)), Color.Red);

			spriteBatch.End ();
		}
	}
}

