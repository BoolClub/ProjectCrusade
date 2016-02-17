using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	/// <summary>
	/// Stores, updates, and draws a stack of game screens. Only the top, active game screen is updated, but all screens are drawn. This allows for, e.g., transparent pause menus.
	/// </summary>
	public class GameScreenManager
	{
		Stack<GameScreen> gameScreens;

		public GameScreenManager (GameScreen initialGameScreen)
		{
			gameScreens = new Stack<GameScreen> ();
			PushGameScreen (initialGameScreen);
		}

		public void PushGameScreen(GameScreen screen) {
			gameScreens.Push (screen);
		}
		public void PopGameScreen() {
			gameScreens.Pop ();
		}

		public void Update(GameTime gameTime)
		{
			//Only update top screen.
			gameScreens.Peek ().Update (gameTime, this);

		}

		/// <summary>
		/// Draw all screens. Note that each screen needs to call SpriteBatch.Begin separately, because different matrix transformations can be applied to each screen.
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		/// <param name="textureManager">Texture manager.</param>
		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager)
		{
			//We need to draw the last screen last, so we reverse the stack.
			var reverseStack = new Stack<GameScreen> (gameScreens.ToArray ());

			foreach (GameScreen screen in reverseStack)
				screen.Draw (spriteBatch, textureManager, fontManager);
		}

	}
}

