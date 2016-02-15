using System;

namespace ProjectCrusade
{
	/// <summary>
	/// Represents different items that the player can pick up and have in the inventory.
	/// </summary>
	/// <value>An Item.</value>
	public abstract class Item {
		
		//This boolean determines whether or not the item can be stacked.
		public bool Stackable;

		//This integer represents the item's ID. Each ItemType has it's own ID.
		public int ItemID { get; set; }

		//The current stack size of a stackable item.
		public int CurrentStackSize { get; set; }

		//The maximum stack that a stackable item can hold.
		protected const int MaxStackSize = 64;



		public Item() {}

		//Returns information about the item. This can be displayed on the screen so the player knows what each item does.
		public abstract String ItemInfo ();

		//Used for when the item you want to stack only has a stack of one.
		public void addToStack() {
			if (CurrentStackSize < MaxStackSize) {
				CurrentStackSize++;
			}
		}
		//Used for when the other item has a stack larger than one. In that case you insert that item's stack into the parameter.
		public void addToStack(int amount) {
			if (CurrentStackSize < MaxStackSize) {
				CurrentStackSize += amount;
			}
		}
	}

}

