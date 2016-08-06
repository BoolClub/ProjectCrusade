using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ProjectCrusade
{
	/// <summary>
	/// Main Menu. Determines where to go.
	/// </summary>
	public class MainMenuScreen : GameScreen
	{
		/// <summary>
		/// Gets or sets the state of the previous key.
		/// </summary>
		/// <value>The state of the previous key.</value>
		public static KeyboardState PrevKeyState { get; set; }

		/// <summary>
		/// The options for the menu.
		/// </summary>
		string[] options = {"Start","How To Play","Quit"};

		/// <summary>
		/// The option that is currently selected
		/// </summary>
		int currentOption = 0;

		/// <summary>
		/// Whether or not the usr can move up or down the menu.
		/// </summary>
		bool canMove = true;

		/// <summary>
		/// The texture manager.
		/// </summary>
		TextureManager textureManager;





		/// <summary>
		/// CONSTRUCTOR.
		/// </summary>
		public MainMenuScreen(TextureManager tm)
		{
			textureManager = tm;
		}


		/// <summary>
		/// Handles the input for moving about the main menu.
		/// </summary>
		/// <returns>The menu input.</returns>
		/// <param name="time">Time.</param>
		public void CheckMenuInput(GameTime time, GameScreenManager screenManager) {
			KeyboardState keyState = Keyboard.GetState();

			#region menu options
			//You can only change options after both keys are up
			if (keyState.IsKeyUp(Keys.Up) && keyState.IsKeyUp(Keys.Down)) {
				canMove = true;
			}

			//Move between the different options
			if (keyState.IsKeyDown(Keys.Up) && PrevKeyState.IsKeyUp(Keys.Up))
			{
				if (currentOption > 0 && canMove == true)
				{
					currentOption--;
					canMove = false;
				}
			}
			if (keyState.IsKeyDown(Keys.Down) && PrevKeyState.IsKeyUp(Keys.Down))
			{
				if (currentOption < 2 && canMove == true)
				{
					currentOption++;
					canMove = false;
				}
			}
			#endregion



			#region option selection

			//Select an option
			if ((keyState.IsKeyDown(Keys.C) && PrevKeyState.IsKeyUp(Keys.C)) || (keyState.IsKeyDown(Keys.Enter) && PrevKeyState.IsKeyUp(Keys.Enter))) {

				if (currentOption == 0) {
					//start the game
					screenManager.PushGameScreen(new MainGameScreen(textureManager));
				}

				if (currentOption == 1) {
					//go to the controls screen
					//Eventually make another game screen that just displays the controls
				}

				if (currentOption == 2) {
					//quit the game
					MainGame.ShouldQuit = true;
				}

			}

			#endregion
		}


		/* ABSTRACT METHODS */

		public override void Update (GameTime gameTime, GameScreenManager screenManager, MainGame game)
		{
			CheckMenuInput(gameTime, screenManager);
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, float opacity)
		{
			spriteBatch.Begin();

			//Draw the text for each option on the screen
			foreach (string s in options)
			{
				if (s.Equals(options[0])) {
					spriteBatch.DrawString(fontManager.GetFont("MainFontLarge"), s, new Vector2(MainGame.WindowWidth / 2, MainGame.WindowHeight / 2 - 50), Color.White);
				}
			    if (s.Equals(options[1])) {
					spriteBatch.DrawString(fontManager.GetFont("MainFontLarge"), s, new Vector2(MainGame.WindowWidth / 2 - 35, MainGame.WindowHeight / 2 - 20), Color.White);
				}
				if (s.Equals(options[2])) {
					spriteBatch.DrawString(fontManager.GetFont("MainFontLarge"), s, new Vector2(MainGame.WindowWidth / 2, MainGame.WindowHeight / 2 + 10), Color.White);
				}
			}

			//Draw a little dot next to the currently selected option
			if (currentOption == 0)
				spriteBatch.Draw(textureManager.WhitePixel, new Rectangle(MainGame.WindowWidth / 2 - 20, MainGame.WindowHeight / 2 - 40, 10, 10), Color.White);
			else if(currentOption == 1)
				spriteBatch.Draw(textureManager.WhitePixel, new Rectangle(MainGame.WindowWidth / 2 - 55, MainGame.WindowHeight / 2 - 10, 10, 10), Color.White);
			else if(currentOption == 2)
				spriteBatch.Draw(textureManager.WhitePixel, new Rectangle(MainGame.WindowWidth / 2 - 20, MainGame.WindowHeight / 2 + 15, 10, 10), Color.White);


			spriteBatch.End();
		}
			
	}
}

