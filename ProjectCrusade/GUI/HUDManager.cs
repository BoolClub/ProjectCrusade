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

		const int height = 48;

		//TESTING OUT THE NPC. IN THE REAL GAME THE NPC WILL NOT BE IN THE HUDMANAGER CLASS.
		NPC npc;


		public HUDManager (World w) {
			world = w;
			//Create object with the player's sanity.
			sanitybar = new SanityBarGUI (world.Player.Sanity);



			//Testing out NPCs and the TextBox.
			npc = new NPC ("",world);

			//Test speech
			String t = "sexdctfvyguhlijkn.uyjthregszdxfcgvhjkhgfxdzsxfbcgvhjbkn.lxgdcfvj,kn.lmkmhxgdhcfvgjbk.l/k,yjftrdhrfjyuhijok/;lhmgncfdxfhcvdsadhjdshdasbhjdasdialudaiudjasndlkajnsdjsafiudhalisufjdagildhishugdljkhgasdighdsiuagsnksdbghsdkfbj";

			npc.TextBox.AddText (t);
		}


		public void Update(GameTime time) {
			sanitybar.Update (time, world.Player.Sanity, world.Player.MaxSanity);


			npc.Update (time, world);
		}

		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager, FontManager fM) {
			drawHUDBackground (spriteBatch, textureManager);

			sanitybar.Draw (spriteBatch, textureManager, fM);


			npc.Draw (spriteBatch, textureManager, fM, Color.Green);
		}


		/// <summary>
		/// Draw the background of the HUD. All of the HUD elements will be drawn on top of this.
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		private void drawHUDBackground(SpriteBatch spriteBatch, TextureManager textureManager) {
			spriteBatch.Draw (textureManager.WhitePixel, new Rectangle(
				0, MainGame.WindowHeight - height,MainGame.WindowWidth,height), new Color(0,0,150,0.5f));
		}

	} //END OF HUDMANAGER CLASS

} //END OF NAMESPACE

