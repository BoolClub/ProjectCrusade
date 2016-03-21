using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text;


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
		public Vector2 Position { get; set; }

		/// <summary>
		/// The width and height of the text box.
		/// </summary>
		int Width = 512;
		int Height = 256;

		const int Padding = 16;

		public float Opacity { get; set; }

		public Color TextColor { get; set; }
		public Color BackgroundColor { get; set; }

		/// <summary>
		/// The speech that the text box will display.
		/// </summary>
		List<string> spokenText = new List<string> ();

		/// <summary>
		/// The index in the spokenText list to display. When the player clicks a button, increment this
		/// value so that the player can see the next thing that the text box has to say.
		/// </summary>
		public int speechIndex = 0;

		int currLen = 0;
		const float charAnimTime = 0.05e3f;
		float lastCharAnim = 0f;

		public TextBox (Vector2 position, Color textColor, Color backgroundColor, float opacity = 1.0f) {
			Position = position;
			Opacity = opacity;
			TextColor = textColor;
			BackgroundColor = backgroundColor;
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
			//restart animation
			currLen = 0;
		}

		public void Update(GameTime gameTime)
		{
			if (lastCharAnim > charAnimTime && currLen < spokenText[speechIndex].Length) {
				currLen++;
				lastCharAnim = 0f;
			}
			lastCharAnim += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
		}

		public void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager) {


			spriteBatch.Draw (textureManager.WhitePixel, null, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), null, null, 0, null, BackgroundColor * Opacity, SpriteEffects.None, 1.0f);

			SpriteFont font = fontManager.GetFont ("MainFontSmall");

			//Draw the first item of text.
			if (spokenText.Count > speechIndex) {


				string text = spokenText [speechIndex];
				int currChar = 0;

				//the main string builder for the entire text box
				StringBuilder main = new StringBuilder ();


				//loop through every character in string
				while (currChar < text.Length) {
					//current width of line
					float currWidth = 0;
					//string builder for line
					StringBuilder builder = new StringBuilder ();

					//only continue if line is smaller than box
					while (currWidth + Padding < Width-Padding) {
						
						if (currChar >= text.Length)
							break;
						
						if (text [currChar] == '\n') {
							currChar++;
							break;
						}

						builder.Append (text [currChar]);
						currWidth = font.MeasureString (builder).X;
						currChar++;
					}

					//append line to main string
					builder.Append ('\n');
					main.Append (builder);
				}

				//draw string
				spriteBatch.DrawString (font, main.ToString().Substring(0,currLen), Position + new Vector2 (Padding, Padding), TextColor, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.1f);
					
			}
		}

	} //END OF TEXTBOX CLASS


} //END OF NAMESPACE

