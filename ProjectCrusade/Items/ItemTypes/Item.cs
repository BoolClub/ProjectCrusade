using Microsoft.Xna.Framework;


namespace ProjectCrusade
{
	/// <summary>
	/// Represents different items that the player can pick up and have in the inventory.
	/// </summary>
	/// <value>An Item.</value>
	public abstract class Item {

		//We are assuming square sprite sheets and square sprites.
		public const int SpriteSheetWidth = 256;
		public const int SpriteWidth = 32;


		//An enum for the type of item. Each item has a different type.
		public ItemType Type { get; protected set; } 

		//This boolean determines whether or not the item can be stacked.
		public bool Stackable;

		//The current stack size of a stackable item.
		public int CurrentStackSize { get; protected set; }

		//The maximum stack that a stackable item can hold.
		protected const int MaxStackSize = 64;

		public abstract bool Depletable { get; }


		public Item(int stackSize = 1) { CurrentStackSize = stackSize;}

		//Returns information about the item. This can be displayed on the screen so the player knows what each item does.
		public abstract string ItemInfo ();

		/// <summary>
		/// Increment size of stack. 
		/// </summary>
		/// <param name="amount">Number of items to add to stack. Default=1</param>
		public void AddToStack(int amount = 1) {
			if (CurrentStackSize < MaxStackSize) {
				CurrentStackSize += amount;
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
		public virtual void PrimaryUse(Player player, World world) { }
		/// <summary>
		/// Optional use for when the player, e.g., right clicks
		/// </summary>
		public virtual void SecondaryUse(Player player, World world) { }
	}


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
	}
}

