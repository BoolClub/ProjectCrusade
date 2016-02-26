using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	//Handles all of the HUD elements
	public class HUDManager
	{
		World world;
		SanityBarGUI sanitybar;

		public HUDManager (World w) {
			world = w;
			//Create object with the player's sanity.
			sanitybar = new SanityBarGUI (world.Player.Sanity);
		}


		public void update(GameTime time) {
			sanitybar.Update (time, world.Player.Sanity);
		}

		public void draw(SpriteBatch spriteBatch, FontManager fM) {
			drawHUDBackground (spriteBatch);


			sanitybar.Draw (spriteBatch, fM);
		}


		/// <summary>
		/// Draw the background of the HUD. All of the HUD elements will be drawn on top of this.
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		private void drawHUDBackground(SpriteBatch spriteBatch) {
			Texture2D rectangleTexture = new Texture2D (MainGame.graphics.GraphicsDevice, MainGame.WindowWidth, 50);
			Color[] data = new Color[MainGame.WindowWidth * 50];
			for (int i = 0; i < data.Length; i++) { data [i] = new Color(0,0,150,0.5f); }
			rectangleTexture.SetData(data);
			spriteBatch.Draw (rectangleTexture, new Vector2(0,MainGame.WindowHeight-40), new Color(0,0,150,0.5f));
		}

	} //END OF HUDMANAGER CLASS

} //END OF NAMESPACE

