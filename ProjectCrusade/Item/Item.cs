using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ProjectCrusade
{
	public class Item
	{
		public const int SpriteWidth = 32;
		public const int SpriteSheetWidth = 256;

		public string Name;
		public string Identifier;
		public string Tooltip;
		public int Count;
		public int Max;
		public string Type; 
		public List<Tuple<string,float,int,int,int>> Behavior;
		public Rectangle TextureResource;

		public Item ShallowCopy()
		{
			return (Item)MemberwiseClone ();
		}

		/// <summary>
		/// Adds a certain number of items to the stack. Returns number of items not added to the stack.
		/// </summary>
		public int Add(int x){
			int remain = Count + x - Max;
			Count = remain < 0 ? Count + x : Max;
			return remain > 0 ? remain : 0;
		}

		/// <summary>
		/// Attempts to remove a certain number of items from the stack. Returns false if all items could not be removed. 
		/// </summary>
		public bool Remove(int x){
			Count=Count-x<0?0:Count-x;
			return !(Count - x < 0);
		}

		public void PrimaryUse(World world){
			if (Type=="consumable")
				Count--;
			ItemBehavior.RunBehavior(Type, Behavior, world);
		}
	}
}

