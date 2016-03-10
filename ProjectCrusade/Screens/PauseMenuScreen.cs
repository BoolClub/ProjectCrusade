using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrusade
{
	public class PauseMenuScreen : GameScreen
	{
		KeyboardState prevKeyboardState;

		Menu menu;

		GameScreenManager screenManager;

		MainGame game; //reference to game so that we can quit


		public PauseMenuScreen (GameScreenManager _screenManager, MainGame game)
		{
			screenManager = _screenManager;
			prevKeyboardState = Keyboard.GetState ();
			menu = new Menu ((new Vector2(MainGame.WindowWidth*0.5f, MainGame.WindowHeight*0.5f)), "Arial", 20);
			MenuItem menuItem1 = new MenuItem ("Resume");
			menuItem1.Activated += new MenuItem.ActivatedHandler (Option1);
			MenuItem menuItem2 = new MenuItem ("Quit");
			menuItem2  .Activated += new MenuItem.ActivatedHandler (Option2);
			menu.AddItem (menuItem1);
			menu.AddItem (menuItem2);
			this.game = game;
		}


		//Unpause game
		private void Option1(MenuItem m, EventArgs e)
		{
			screenManager.PopGameScreen (100);
		}


		//Quit
		private void Option2(MenuItem m, EventArgs e)
		{
			//TODO: make more elegant. Currently there is an issue with objects being disposed, etc. 
			try 
			{
			game.Exit ();
			}
			catch(NullReferenceException ex) {
				Console.WriteLine(ex.StackTrace);
			}
		}

		public override void Update (GameTime gameTime, GameScreenManager screenManager, MainGame game)
		{
			//unpause

			menu.Update (prevKeyboardState);

			prevKeyboardState = Keyboard.GetState ();
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, float opacity)
		{
			spriteBatch.Begin ();

			spriteBatch.Draw (textureManager.WhitePixel, new Rectangle (0, 0, MainGame.WindowWidth, MainGame.WindowHeight), Color.Black * 0.5f*opacity);

			string text = "Paused";

			spriteBatch.DrawString (
				fontManager.GetFont ("Arial"), 
				text, 
				menu.Position - new Vector2 (
					(float)fontManager.GetFont("Arial").MeasureString(text).X/2, 
					50), 
				Color.White*opacity);

			menu.Draw (spriteBatch, fontManager, opacity);


			spriteBatch.End ();
		}
			
	}
}

