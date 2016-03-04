using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace ProjectCrusade
{
	/*
	* Class that handles all of the player input. To use it, just go into the Player class and call this 
	* static method in the update method.
	*/

	public static class PlayerInput {

		public static bool Moving { get; private set; }

		public static Player player;

		public static MouseState PrevMouseState { get; set; }
		public static KeyboardState PrevKeyState { get; set; }


		//PLAYER INPUT
		public static void CheckInput(GameTime time) {
			KeyboardState keyState = Keyboard.GetState ();

			float calcDisp = (float)time.ElapsedGameTime.TotalSeconds * player.Speed;

			Vector2 disp = Vector2.Zero;


			//Move player.
			if (keyState.IsKeyDown (Keys.D) || keyState.IsKeyDown (Keys.Right)) {
				disp += new Vector2 (calcDisp, 0);
				Moving = true;	
			}
			if (keyState.IsKeyDown (Keys.A) || keyState.IsKeyDown (Keys.Left)) {
				disp += new Vector2 (-calcDisp, 0);
				Moving = true;
			}
			if (keyState.IsKeyDown (Keys.S) || keyState.IsKeyDown (Keys.Down)) {
				disp += new Vector2 (0, calcDisp);
				Moving = true;
			}
			if (keyState.IsKeyDown (Keys.W) || keyState.IsKeyDown (Keys.Up)) {
				disp += new Vector2 (0, -calcDisp);
				Moving = true;
			}


			//Primary Use Items
			if (keyState.IsKeyDown (Keys.Q) && PrevKeyState.IsKeyUp(Keys.Q)) {
				if (player.Inventory.ActiveSlot != null) {
					if (player.Inventory.ActiveSlot.HasItem) {
						player.Inventory.ActiveSlot.Item.PrimaryUse (player.world);
						//If the item is depletable, it is removed when used.
						//if (player.Inventory.ActiveSlot.Item.Depletable) {
						//	player.Inventory.ActiveSlot.RemoveItem ();
						//}
					}

				}
			}
			ItemManager ItemManager = new ItemManager();
			//Quickly add an item -- (just for testing purposes)
			if (keyState.IsKeyDown (Keys.N) && PrevKeyState.IsKeyUp (Keys.N)) {
				player.Inventory.AddItem (ItemManager.Data ["gold"]);
				player.Inventory.AddItem (ItemManager.Data ["apple"]);
			}


			//Normalize displacement so that you travel the same speed diagonally. 
			if ((keyState.IsKeyDown (Keys.D) && keyState.IsKeyDown (Keys.W)) || (keyState.IsKeyDown (Keys.D) && keyState.IsKeyDown (Keys.S)) || (keyState.IsKeyDown (Keys.A) && keyState.IsKeyDown (Keys.W)) || (keyState.IsKeyDown (Keys.A) && keyState.IsKeyDown (Keys.S))) {
				disp /= (float)Math.Sqrt (2.0);
			}
			if ((keyState.IsKeyDown (Keys.Right) && keyState.IsKeyDown (Keys.Up)) || (keyState.IsKeyDown (Keys.Right) && keyState.IsKeyDown (Keys.Down)) || (keyState.IsKeyDown (Keys.Left) && keyState.IsKeyDown (Keys.Up)) || (keyState.IsKeyDown (Keys.Left) && keyState.IsKeyDown (Keys.Down))) {
				disp /= (float)Math.Sqrt (2.0);
			}


			player.Position+=disp;

		}

	} //END OF PLAYERINPUT CLASS



} //END OF NAMESPACE

