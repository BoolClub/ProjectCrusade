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
using System;


namespace ProjectCrusade
{


	/// <summary>
	/// Used as an ID for each item. 
	/// </summary>
	public enum ItemSprite {
		Apple		 	= 0,
		Coin		 	= 1,
		WoodenSword		= 2,
		StoneSword		= 18,
		FireySword		= 34,
		GreenSword		= 50,
		GoldSword		= 66,
		StoneSword2		= 82,
		IronSword		= 98,
		BowArrow		= 3,
		MagicWand		= 4,
		Mace			= 5,
		Dagger			= 6,
		Arrow			= 7,
		BlueFireball	= 8,
		RedFireball		= 9,
		Bread			= 16,
		Water		 	= 32,
	}


	/// <summary>
	/// Represents different items that the player can pick up and have in the inventory.
	/// </summary>
	/// <value>An Item.</value>
	public abstract class Item {

		//We are assuming square sprite sheets and square sprites.
		public const int SpriteSheetWidth = 512;
		public const int SpriteWidth = 32;


		/// <summary>
		/// An enum for the type of item. Each item has a different type.
		/// </summary>
		public abstract ItemSprite Type { get; } 

		/// <summary>
		/// This boolean determines whether or not the item can be stacked.
		/// </summary>
		public abstract bool Stackable { get; }

		int count;
		/// <summary>
		/// The current stack size of a stackable item.
		/// </summary>
		public int Count { get { return count;} set { 
			
				if (!Stackable && value > 1)
					throw new Exception ("A non-stackable item cannot have a count of more than 1.");
				else
					count = value;
			}}

		/// <summary>
		/// The maximum stack that a stackable item can hold.
		/// </summary>
		public const int MaxCount = 64;

		/// <summary>
		/// Whether an item is removed from the stack when used.
		/// </summary>
		public abstract bool Depletable { get; }


		public Item(int stackSize = 1) { count = stackSize;}

		/// <summary>
		/// Returns information about the item. This can be displayed on the screen so the player knows what each item does.
		/// </summary>
		public abstract string Tooltip { get; }

		/// <summary>
		/// Gets the name of the item.
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Increment size of stack. 
		/// </summary>
		/// <param name="amount">Number of items to add to stack. Default=1</param>
		public void AddToStack(int amount = 1) {
			if (count < MaxCount) {
				count += amount;
			}
		}

		public Rectangle GetTextureSourceRect()
		{
			return Item.GetTextureSourceRect ((int)Type);
		}

		public static Rectangle GetTextureSourceRect(int id) {
			int sheetWidthSprites = SpriteSheetWidth / SpriteWidth;

			int x = id % sheetWidthSprites;
			int y = id / sheetWidthSprites;

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

		public virtual void Update(GameTime gameTime) { } 
	}

}

