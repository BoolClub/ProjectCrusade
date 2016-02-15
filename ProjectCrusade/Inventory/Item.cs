using System;

namespace ProjectCrusade
{
	/// <summary>
	/// Represents different items that the player can pick up and have in the inventory.
	/// </summary>
	/// <value>An Item.</value>
	public class Item : IUD {

		ItemType Type { get; set; }
		public bool Stackable;
		public int itemID;
		public int CurrentStackSize 


		public Item() {
			Type = null;
		}
		public Item(ItemType type) {
			Type = type;
		}


		private void writeItemInfo() {
			if (ItemType == ItemType.Coin) {
				Stackable = true;
				itemID = 1;
			}
			if (ItemType == ItemType.Apple) {
				Stackable = false;
				itemID = 2;
			}
			if (ItemType == ItemType.Sword) {
				Stackable = false;
				itemID = 3;
			}
		}

		void Initialize() {
			writeItemInfo ();
		}
		void Update(GameTime gameTime) {
			
		}
		void Draw(SpriteBatch spriteBatch, TextureManager textureManager) {

		}

	}


	/* Enum that just lists the different types of items. */
	public enum ItemType {
		Coin,
		Apple,
		Sword
	}
}

