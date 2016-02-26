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

		public void draw(SpriteBatch spriteBatch) {
			sanitybar.Draw (spriteBatch);
		}


	} //END OF HUDMANAGER CLASS

} //END OF NAMESPACE

