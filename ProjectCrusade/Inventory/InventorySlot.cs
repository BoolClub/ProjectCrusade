using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrusade
{
	public class InventorySlot
	{
		public Item Item { get; set; }
		public bool HasItem { get { return Item != null; } }

		public Rectangle CollisionBox { get; set; }



		public InventorySlot (Rectangle collisionRectangle) {
			Item = null;
			CollisionBox = collisionRectangle;
		}

		public void Initialize() {

		}
	
		public void Update(GameTime time) {
		}

		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager) {
			
		}


		/// <summary>
		/// Moves an item into the inventory slot.
		/// </summary>
		/// <returns><c>true</c>, if item was successfully added, <c>false</c> otherwise.</returns>
		/// <param name="itm">Item to move into slot.</param>
		public bool AddItem(Item itm) {


			//If there is no current item, then just add the item to this inventory slot.
			if (Item == null) {
				Item = itm;
				return true;
			//However, if there is an item...
			} else {
				//If they are the same item...
				if (Item.Type == itm.Type) {
					
					//If the item is stackable, then add to the stack.
					if (this.Item.Stackable) {
						
						this.Item.AddToStack (itm.Count);
						return true;
					//If it is not stackable, write this message.
					} else {
						Console.WriteLine ("You cannot stack that item.");
						return false;
					}

				//If they are not the same item, write this message.
				} else {

					Console.WriteLine ("These are not the same item, you cannot put the item in this slot.");
					return false;
				}
			}
		}

		/// <summary>
		/// Removes some number of items from the slot (one by default)
		/// </summary>
		/// <returns><c>true</c>, if item was removed, <c>false</c> otherwise.</returns>
		public bool RemoveItem(int count = 1) {
			if (Item == null || Item.Count==0)
				return false;
			int prevSize = Item.Count;

			Item.AddToStack (-count);
			if (Item.Count <= 0)
				Item = null;
			return true;
		}


	} //END OF INVENTORYSLOT CLASS





} //END OF NAMESPACE



