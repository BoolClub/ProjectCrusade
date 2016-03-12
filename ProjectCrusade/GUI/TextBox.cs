using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace ProjectCrusade
{
	/// <summary>
	/// This class represents a text box that will be used to display conversations between the player
	/// and NPCs. It contains a list of spoken text, which is what the NPC will say. Everytime a button is clicked,
	/// the text will change to the next one in the list, and will display that text. By doing this we can have 
	/// different NPCs saying different things, and by pressing "spacebar," for example, we can go to the next
	/// line of text.
	/// </summary>
	public class TextBox
	{
		Vector2 Position { get; set; }

		const int Height = 90;

		/// <summary>
		/// The speech that the text box will display.
		/// </summary>
		List<String> spokenText = new List<String> ();

		/// <summary>
		/// The index in the spokenText list to display. When the player clicks a button, increment this
		/// value so that the player can see the next thing that the text box has to say.
		/// </summary>
		public int speechIndex = 0;



		public TextBox () {
			Position = new Vector2(0,500);
		}

		/// <summary>
		/// Adds text for the NPC to say to the player. The program should crash if this has not been set yet.
		/// </summary>
		/// <param name="speech">Speech.</param>
		public void addSpokenText(String text) {
			spokenText.Add (text);
		}

		/// <summary>
		/// Removes text from the text box.
		/// </summary>
		/// <param name="text">Text.</param>
		public void removeSpokenText(String text) {
			if (spokenText.Contains (text))
				spokenText.Remove (text);
		}


		public void Initialize () {

		}

		public void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager) {
			Texture2D box = new Texture2D (MainGame.graphics.GraphicsDevice, MainGame.WindowWidth, Height);
			Color[] data = new Color[MainGame.WindowWidth * Height];
			for (int i = 0; i < data.Length; i++) { data [i] = Color.White; }
			box.SetData (data);

			spriteBatch.Draw (box, Position, Color.White);


			//Draw the first item of text.
			spriteBatch.DrawString (fontManager.GetFont ("Arial"), spokenText [speechIndex], Position, Color.Black);
		}

		public void Update(GameTime gameTime, World world) {

		}



	} //END OF TEXTBOX CLASS
		

} //END OF NAMESPACE

