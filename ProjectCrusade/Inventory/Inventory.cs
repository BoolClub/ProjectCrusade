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
		InventorySlot[,] slots;

		//The slot that is currently being selected. Used for performing actions on inventory items.
		public static InventorySlot selectedSlot;


		//The number of items in the inventory (see 'checkInventoryFull' method below).
		private int numItems = 0;

		//Boolean for whether or not the inventory is full.
		public bool IsFull = false;

		/// <summary>
		/// Slot spacing in pixels.
		/// </summary>
		const int SlotSpacing = 8;

		Vector2 screenPosition = new Vector2 (10, 10);


		public Inventory (int rows, int columns) {
			Rows = rows;
			Columns = columns;
			slots = new InventorySlot[Columns, Rows];
			Initialize ();
		}




		public void Initialize() {
			for (int x = 0; x < Columns; x++) {
				for (int y = 0; y < Rows; y++) {
					slots [x,y] = new InventorySlot (x,y);

				}
			}
		}

		public void Update(GameTime time) {
			checkInventoryFull ();
			checkInventoryItemSelected ();

		}

		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager) {

			for (int i = 0; i < Columns; i++)
				for (int j = 0; j < Rows; j++) {
					
					if (slots [i, j] != selectedSlot) {
						int disp = SlotSpacing + Item.SpriteWidth;

						int x = (int)screenPosition.X + disp * i;
						int y = (int)screenPosition.Y + disp * j;

						Rectangle r = new Rectangle (x, y, Item.SpriteWidth, Item.SpriteWidth);

						spriteBatch.Draw (textureManager.WhitePixel, r, Color.White * 0.5f);
						if (slots [i, j].HasItem)
							spriteBatch.Draw (textureManager.GetTexture ("items"), null, r, slots [i, j].Item.getTextureSourceRect (), null, 0, null, null, SpriteEffects.None, 0);
					} else {
						int disp = SlotSpacing + Item.SpriteWidth;

						int x = (int)screenPosition.X + disp * i;
						int y = (int)screenPosition.Y + disp * j;

						Rectangle r = new Rectangle (x, y, Item.SpriteWidth, Item.SpriteWidth);

						spriteBatch.Draw (textureManager.WhitePixel, r, Color.Red * 0.5f);
						if (slots[i,j].HasItem)
							spriteBatch.Draw (textureManager.GetTexture ("items"), null, r, slots [i, j].Item.getTextureSourceRect (), null, 0, null, null, SpriteEffects.None, 0);
					}
				}

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
			} else {
				IsFull = false;
			}
		}

		/// <summary>
		/// Adds an item to the first empty/similar slot in the inventory.
		/// </summary>
		/// <returns><c>true</c>, if item was added, <c>false</c> otherwise.</returns>
		public bool AddItem(Item item) 
		{
			//TODO: check if similar item exists in inventory.
			if (IsFull)
				return false;

			for (int j = 0; j < Rows; j++)
				for (int i = 0; i < Columns; i++) {
					if (!slots [i,j].HasItem) {
						slots [i,j].AddItem (item);
						return true;
					}
				}
			return true;
		}


		/// <summary>
		/// Checks if an item in the inventory is going to be selected.
		/// </summary>
		/// <value> SelectedSlot </value>
		private void checkInventoryItemSelected() {
			for (int j = 0; j < Rows; j++) {
				for (int i = 0; i < Columns; i++) {
					if (slots [i, j].CollisionBox ().Contains (Mouse.GetState ().Position) && Mouse.GetState().LeftButton == ButtonState.Pressed) {
						selectedSlot = slots [i, j];
					}
				}
			}
		}


	}//END OF INVENTORY CLASS


} //END OF NAMESPACE


