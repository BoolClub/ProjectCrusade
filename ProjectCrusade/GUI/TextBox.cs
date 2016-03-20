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
		/// <summary>
		/// THe position of the text box.
		/// </summary>
		/// <value>The position.</value>
		Vector2 Position { get; set; }

		/// <summary>
		/// The width and height of the text box.
		/// </summary>
		const int Width = 1024;
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
		/// Removes all of the text from the text box.
		/// </summary>
		public void RemoveAllText() {
			spokenText.Clear ();
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
			if (spokenText.Count > speechIndex) {
				//Less than 50 characters, just draw one line.
				if (spokenText [speechIndex].Length < 50) {

					spriteBatch.DrawString (fontManager.GetFont ("Arial"), spokenText [speechIndex], Position + new Vector2 (Padding, Padding), Color.Black * Opacity);

				} else {

					//The number of lines to draw
					int numLines = spokenText [speechIndex].Length / 100;

					//The first charater on the line, up to the last character to display on one line
					int firstChar = 0, lastChar = 40;

					//The number of lines computes evenly
					if (spokenText [speechIndex].Length % 100 == 0) {
						//Draw one line of text, then add to tempY so you draw it on what looks like another line.
						float tempY = Position.Y;
						for (int i = 0; i < numLines; i++) {
							spriteBatch.DrawString (fontManager.GetFont ("Arial"), spokenText [speechIndex].Substring(firstChar,spokenText[speechIndex].Length - lastChar), new Vector2 (Position.X, tempY) + new Vector2 (Padding, Padding), Color.Black * Opacity);
							tempY += 30;
							firstChar = lastChar + 1;
							lastChar = lastChar + lastChar;
						}
					} else {
						//Draw one line of text, then add to tempY so you draw it on what looks like another line.
						float tempY = Position.Y;
						for (int i = 0; i < numLines + 1; i++) {
							spriteBatch.DrawString (fontManager.GetFont ("Arial"), spokenText [speechIndex].Substring(firstChar,spokenText[speechIndex].Length - lastChar), new Vector2 (Position.X, tempY) + new Vector2 (Padding, Padding), Color.Black * Opacity);
							tempY += 30;
							firstChar = lastChar + 1;
							lastChar = lastChar + lastChar;
						}
					}
				}
			}
		}

	} //END OF TEXTBOX CLASS


} //END OF NAMESPACE

