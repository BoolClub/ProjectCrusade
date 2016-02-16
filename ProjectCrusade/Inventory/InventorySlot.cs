using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;

namespace ProjectCrusade
{
	public class InventorySlot
	{
		public Item Item { get; set; }
		public bool HasItem { get { return Item == null; } }


		public InventorySlot () {
			
		}

		public void Initialize() {

		}
	
		public void Update(GameTime time) {
		}

		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager) {

		}


		//Sets the item on this inventory slot.
		public void setItem(Item itm) {
			
			//If there is no current item, then just add the item to this inventory slot.
			if (Item == null) {
				Item = itm;

			//However, if there is an item...
			} else {
				//If they are the same item...
				if (Item.Type == itm.Type) {
					
					//If the item is stackable, then add to the stack.
					if (this.Item.Stackable == true) {
						
						this.Item.addToStack (itm.CurrentStackSize);

					//If it is not stackable, write this message.
					} else {
						Console.WriteLine ("You cannot stack that item.");
					}

				//If they are not the same item, write this message.
				} else {

					Console.WriteLine ("These are not the same item, you cannot put the item in this slot.");

				}
			}
		}



	
	
	} //END OF INVENTORYSLOT CLASS





} //END OF NAMESPACE



