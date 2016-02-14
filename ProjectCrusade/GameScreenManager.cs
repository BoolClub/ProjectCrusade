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

		public GameScreenManager ()
		{
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
			gameScreens.Peek ().Update (gameTime);
		}

		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager)
		{
			foreach (GameScreen screen in gameScreens)
				screen.Draw (spriteBatch, textureManager);
		}

	}
}

