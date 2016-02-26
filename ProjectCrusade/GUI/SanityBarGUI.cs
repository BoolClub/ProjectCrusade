using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	//<summary> This class represents the GUI that displays the player's sanity. </summary>
	public class SanityBarGUI
	{
		Vector2 position;
		float sanity;
		const int HEIGHT = 20;
		Color barColor = new Color ();

		public SanityBarGUI (float sanityAmount) {
			sanity = sanityAmount;
			position = new Vector2 (MainGame.WindowWidth - 250, MainGame.WindowHeight - 35);
		}

		public void Update(GameTime time, float sanityAmount) {
			sanity = sanityAmount;

			if (sanity <= 100 && sanity > 60) {
				barColor = new Color(0,255,0);
			} else if (sanity <= 60 && sanity > 30) {
				barColor = Color.Yellow;
			} else {
				barColor = Color.Red;
			}
		}

		public void Draw(SpriteBatch spriteBatch) {
			Texture2D rectangleTexture = new Texture2D (MainGame.graphics.GraphicsDevice, (int)sanity*2, HEIGHT);
			Color[] data = new Color[(int)sanity*2 * HEIGHT];

			for (int i = 0; i < data.Length; i++) {
				data [i] = barColor;
			}

			rectangleTexture.SetData(data);

			spriteBatch.Draw (rectangleTexture, position, barColor);
		}

	} //END OF SANITYBARGUI CLASS



} //END OF NAMESPACE

