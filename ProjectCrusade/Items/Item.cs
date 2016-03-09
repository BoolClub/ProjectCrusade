using Microsoft.Xna.Framework;


/* *** HOW TO IMPLEMENT ITEMS ***
 * 
 * 1. When creating a new item, make sure it inherits from Item or one of its derivatives.
 * 
 * 2. Implement all of the abstract properties, such as Type, Stackable, Depletable, etc. Most of these are constants. 
 * 
 * 		- Your code won't compile if you don't implement these properties. 
 * 
 * 3. Implement, if necessary, the PrimaryUse() and SecondaryUse() methods. 
 * 
 * 		- Many items won't need this, even if they have functionality. 
 * 
 * 		- Their abstract base classes usually handle this stuff.
 * 
 * 		- Example: Apple derives from Food, but it doesn't have to implement PrimaryUse(). 
 * 		  Instead, Food has already implemented the method, as all food items heal. 
 *		  What needs to be modified is the HealValue abstract property (a special property 
 *		  that all food items must implement). 
 *
 *  *** SEE Apple CLASS FOR A SIMPLE EXAMPLE ***
 */


namespace ProjectCrusade
{


	/// <summary>
	/// Used as an ID for each item. 
	/// </summary>
	public enum ItemType {
		Apple		 	= 0,
		Water		 	= 1,
		Coin		 	= 2,
		WoodenSword 	= 3,
		StarterArrow	= 4,
		MagicWand		= 5,
		Bread			= 6,
		StoneSword		= 7,
		IronSword		= 8,

	}


	/// <summary>
	/// Represents different items that the player can pick up and have in the inventory.
	/// </summary>
	/// <value>An Item.</value>
	public abstract class Item {

		//We are assuming square sprite sheets and square sprites.
		public const int SpriteSheetWidth = 256;
		public const int SpriteWidth = 32;


		//An enum for the type of item. Each item has a different type.
		public abstract ItemType Type { get; } 

		//This boolean determines whether or not the item can be stacked.
		public abstract bool Stackable { get; }

		//The current stack size of a stackable item.
		public int Count { get; protected set; }

		//The maximum stack that a stackable item can hold.
		public const int MaxStackSize = 64;

		//Whether an item is removed from the stack when used.
		public abstract bool Depletable { get; }


		public Item(int stackSize = 1) { Count = stackSize;}

		//Returns information about the item. This can be displayed on the screen so the player knows what each item does.
		public abstract string Tooltip { get; }

		/// <summary>
		/// Increment size of stack. 
		/// </summary>
		/// <param name="amount">Number of items to add to stack. Default=1</param>
		public void AddToStack(int amount = 1) {
			if (Count < MaxStackSize) {
				Count += amount;
			}
		}

		public Rectangle getTextureSourceRect()
		{
			int sheetWidthSprites = SpriteSheetWidth / SpriteWidth;

			int x = (int)Type % sheetWidthSprites;
			int y = (int)Type / sheetWidthSprites;

			return new Rectangle (x * SpriteWidth, y * SpriteWidth, SpriteWidth, SpriteWidth);
		}

		/// <summary>
		/// Activated when the player, e.g., left clicks
		/// </summary>
		public virtual void PrimaryUse(World world) { }
		/// <summary>
		/// Optional use for when the player, e.g., right clicks
		/// </summary>
		public virtual void SecondaryUse(World world) { }
	}

}

