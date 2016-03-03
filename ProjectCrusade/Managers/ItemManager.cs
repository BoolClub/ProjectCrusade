using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
namespace ProjectCrusade
{

	//TODO:  
	//		-Create item handler
	//		-Create equipment system
	//		-Assign data values for items


	//Item types: equipment, consumable, misc
	//Item Values
	//	equipment
	//		0: (int) Equip Slot
	//		1: 
	//		2: 
	//		3: 
	//		4:
	//		5:
	//	consumable
	//		0:
	//		1:
	//		2:
	//		3:
	//		4:
	//		5:
	//	misc
	//		0:
	//		1:
	//		2:
	//		3:
	//		4:
	//		5:

	public class ItemManager
	{
		Dictionary<string, Item> Item_data = new Dictionary<string, Item>()
		{
			{ "gold", new Item{Identifier = "gold", Name="Gold", Tooltip="Lovely money.", Count=1, Max=1000, Type="misc", TextureResource=getTextureSourceRect("gold")} }
		};

		public Item getItem(String d){
			return Item_data[d];
		}
		public static Rectangle getTextureSourceRect(String identifier)
		{
			int sheetWidthSprites = 256 / 32;
			int num = (int)Enum.Parse (typeof(ItemSprite), identifier);
			int x = num % sheetWidthSprites;
			int y = num / sheetWidthSprites;
			return new Rectangle (x * 32, y * 32, 32, 32);
		}
	}

	public enum ItemSprite {
		Apple		 	= 0,
		Water		 	= 1,
		gold		 	= 2,
		WoodenSword 	= 3,
		StarterArrow	= 4,
		MagicWand		= 5,
		Bread			= 6,
		StoneSword		= 7,
		IronSword		= 8,

	}

	public class Item
	{
		public String Name;
		public String Identifier;
		public String Tooltip;
		public int Count;
		public int Max;
		public String Type; 
		public float[] Values = new float[10];
		public Rectangle TextureResource;

		public T ShallowCopy<T>() where T : Item
		{
			return (T)(MemberwiseClone());
		}

		public int Add(int x){
			int remain = Count + x - Max;
			Count = remain < 0 ? Count + x : Max;
			return remain > 0 ? remain : 0;
		}

		public bool Remove(int x){
			Count=Count-x<0?0:Count-x;
			return !(Count - x < 0);
		}
	}

}

