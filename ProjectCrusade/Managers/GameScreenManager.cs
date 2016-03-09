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
		float timeRemaining;
		float maxTimeRemaining = -1;
		bool transitionFadeIn = false;

		public GameScreenManager (GameScreen initialGameScreen)
		{
			gameScreens = new Stack<GameScreen> ();
			PushGameScreen (initialGameScreen);
		}

		public void PushGameScreen(GameScreen screen, float transitionTime = 0) {
			gameScreens.Push (screen);
			maxTimeRemaining = transitionTime;
			timeRemaining = maxTimeRemaining;
			transitionFadeIn = true;
		}

		/// <summary>
		/// Pops the game screen.
		/// </summary>
		/// <param name="transitionTime">Transition time in milliseconds.</param>
		public void PopGameScreen(float transitionTime = 0) {
			
			maxTimeRemaining = transitionTime;
			timeRemaining = maxTimeRemaining;
			transitionFadeIn = false;
		}

		public void Update(GameTime gameTime, MainGame game)
		{
			//Only update top screen.
			gameScreens.Peek ().Update (gameTime, this, game);
			if (timeRemaining > 0)
				timeRemaining -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			if (timeRemaining <= 0) {
				if (maxTimeRemaining >= 0) {
					if (!transitionFadeIn) gameScreens.Pop ();
					maxTimeRemaining = -1;
				}
			
				timeRemaining = 0;
			}
		}

		/// <summary>
		/// Draw all screens. Note that each screen needs to call SpriteBatch.Begin separately, because different matrix transformations can be applied to each screen.
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		/// <param name="textureManager">Texture manager.</param>
		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager)
		{
			//We need to draw the top screen last, so we reverse the stack.
			var reverseStack = new Stack<GameScreen> (gameScreens.ToArray ());
			int i = 0; 
			float frac = timeRemaining / maxTimeRemaining;
			foreach (GameScreen screen in reverseStack) {
				screen.Draw (spriteBatch, textureManager, fontManager, ((i==gameScreens.Count-1 && maxTimeRemaining > 0) ? (transitionFadeIn ? (1-frac) : frac) : 1.0f) );
				i++;
			}
		}
	}
}

