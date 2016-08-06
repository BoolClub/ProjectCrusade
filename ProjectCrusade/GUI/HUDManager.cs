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
		HealthBarGUI healthbar;

		const int height = 48;


		public HUDManager (World w) {
			world = w;
			//Create object with the player's sanity.
			healthbar = new HealthBarGUI (world.Player.Health);



		}


		public void Update(GameTime time) {
			healthbar.Update (time, world.Player.Health, world.Player.MaxHealth);


		}

		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager, FontManager fM) {
			drawHUDBackground (spriteBatch, textureManager);

			healthbar.Draw (spriteBatch, textureManager, fM);

		}


		/// <summary>
		/// Draw the background of the HUD. All of the HUD elements will be drawn on top of this.
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		private void drawHUDBackground(SpriteBatch spriteBatch, TextureManager textureManager) {
			spriteBatch.Draw (textureManager.WhitePixel, null, new Rectangle(
				0, MainGame.WindowHeight - height,MainGame.WindowWidth,height), null, null, 0, null, new Color(0,0,150,0.5f), SpriteEffects.None, 0.0f);
		}

	} //END OF HUDMANAGER CLASS

} //END OF NAMESPACE

