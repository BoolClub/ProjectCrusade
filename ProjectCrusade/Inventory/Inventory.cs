using System;
using System.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ProjectCrusade
{
	public class Inventory : IUD {

		//The number of rows and columns (inventory slots) to have in the Inventory.
		public int Rows { get; private set; }
		public int Columns { get; private set; }

		//The slots
		InventorySlot[] slots;

		//The number of items in the inventory (see 'checkInventoryFull' method below).
		private int numItems = 0;

		//Boolean for whether or not the inventory is full.
		public bool IsFull = false;



		public Inventory (int rows, int columns) {
			Rows = rows;
			Columns = columns;
			slots = new InventorySlot[Rows * Columns];
			Initialize ();
		}




		public void Initialize() {
			for (int x = 0; x < Rows; x++) {
				for (int y = 0; y < Columns; y++) {
					slots [x + y * Rows] = new InventorySlot ();
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
			numItems = 0;
			foreach (InventorySlot slot in slots) {
				if (slot.HasItem == true) {
					numItems++;
				}
			}

			if (numItems == Rows * Columns) {
				IsFull = true;
			} else
				IsFull = false;
		}


	}//END OF INVENTORY CLASS


} //END OF NAMESPACE


