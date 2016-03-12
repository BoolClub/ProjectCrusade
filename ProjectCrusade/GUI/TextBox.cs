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

		const int Width = 256;
		const int Height = 128;

		const int Padding = 8;

		public float Opacity { get; set; }

		/// <summary>
		/// The speech that the text box will display.
		/// </summary>
		List<string> spokenText = new List<string> ();

		/// <summary>
		/// The index in the spokenText list to display. When the player clicks a button, increment this
		/// value so that the player can see the next thing that the text box has to say.
		/// </summary>
		public int speechIndex = 0;



		public TextBox (Vector2 position, float opacity = 1.0f) {
			Position = position;
			Opacity = opacity;
		}

		/// <summary>
		/// Adds text for the NPC to say to the player. The program should crash if this has not been set yet.
		/// </summary>
		/// <param name="speech">Speech.</param>
		public void AddText(string text) {
			spokenText.Add (text);
		}

		/// <summary>
		/// Removes text from the text box.
		/// </summary>
		/// <param name="text">Text.</param>
		public void RemoveText(string text) {
			if (spokenText.Contains (text))
				spokenText.Remove (text);
		}

		/// <summary>
		/// Moves on to next line of text
		/// </summary>
		public void Advance()
		{
			speechIndex++;
			if (speechIndex >= spokenText.Count)
				speechIndex = 0;
		}

		public void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager) {
			
			spriteBatch.Draw (textureManager.WhitePixel, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color.White * Opacity);

			//Draw the first item of text.
			if (spokenText.Count > speechIndex) spriteBatch.DrawString (fontManager.GetFont ("Arial"), spokenText [speechIndex], Position + new Vector2(Padding, Padding), Color.Black * Opacity);
		}

	} //END OF TEXTBOX CLASS
		

} //END OF NAMESPACE

