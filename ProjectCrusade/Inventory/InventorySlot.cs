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


		public Vector2 Position;


		public InventorySlot (int x, int y) {
			Position = new Vector2 (x,y);
			Item = null;
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
						
						this.Item.AddToStack (itm.CurrentStackSize);
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


		public Rectangle CollisionBox() {
			return new Rectangle ((int)Position.X,  (int)Position.Y, Item.SpriteWidth, Item.SpriteWidth);
		}
	
	
	} //END OF INVENTORYSLOT CLASS





} //END OF NAMESPACE



