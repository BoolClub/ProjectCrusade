using System;
using System.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;

namespace ProjectCrusade
{
	public class Inventory : IUD {

		//The number of rows and columns (inventory slots) to have in the Inventory.
		public int Rows { get; set; }
		public int Columns { get; set; }

		//The slots
		ArrayList slots;

		//The number of items in the inventory (see 'checkInventoryFull' method below).
		private static int numItems = 0;

		//Boolean for whether or not the inventory is full.
		public bool IsFull = false;



		public Inventory () {
			slots = new ArrayList();
			Initialize ();
		}




		public void Initialize() {
			for (int x = 0; x < Rows; x++) {
				for (int y = 0; y < Columns; y++) {
					slots.Add (new InventorySlot());
				}
			}
		}

		public void Update(GameTime time) {
			checkInventoryFull ();
		}

		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager) {

		}



		//Checks if the inventory is full. If it is, then you cannot add another item.
		private void checkInventoryFull() {
			foreach (InventorySlot slot in slots) {
				if (slot.HasItem == true) {
					numItems++;
				}
			}

			if (numItems == Rows * Columns) {
				IsFull = true;
			}
		}


	}//END OF INVENTORY CLASS


} //END OF NAMESPACE


