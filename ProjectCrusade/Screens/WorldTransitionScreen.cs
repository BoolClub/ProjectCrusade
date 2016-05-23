using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	public class WorldTransitionScreen : GameScreen
	{
		KeyboardState prevKeyboardState;


		GameScreenManager screenManager;

		MainGame game; //reference to game so that we can quit
		TimeSpan duration;
		TimeSpan passed = TimeSpan.Zero;

		public WorldTransitionScreen (GameScreenManager _screenManager, MainGame game)
		{
			screenManager = _screenManager;
			prevKeyboardState = Keyboard.GetState ();
			this.game = game;
			duration = new TimeSpan (0, 0, 3);
		}


		//Unpause game
		private void Close()
		{
			screenManager.PopGameScreen (100);
		}

		public override void Update (GameTime gameTime, GameScreenManager screenManager, MainGame game)
		{
			prevKeyboardState = Keyboard.GetState ();
			passed += gameTime.ElapsedGameTime;
			if (passed > duration)
				Close ();
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, float opacity)
		{
			spriteBatch.Begin ();

			spriteBatch.Draw (textureManager.WhitePixel, new Rectangle (0, 0, MainGame.WindowWidth, MainGame.WindowHeight), Color.Black * 0.5f*opacity);

			string text = "Transition";
			const string fontname = "MainFontLarge";

			spriteBatch.DrawString (
				fontManager.GetFont (fontname), 
				text, 
				new Vector2(200,200) - new Vector2 (
					(float)fontManager.GetFont(fontname).MeasureString(text).X/2, 
					50), 
				Color.White*opacity);
			

			spriteBatch.End ();
		}
	}
}

