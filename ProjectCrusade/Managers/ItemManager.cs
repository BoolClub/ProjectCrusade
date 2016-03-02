using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
namespace ProjectCrusade
{
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
			{ "gold", new Item{identifier = "gold", name="Gold", tooltip="Lovely money.", max=1000, type="misc", TextureResource=getTextureSourceRect("gold")} }
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
		public String name;
		public String identifier;
		public String tooltip;
		public int count;
		public int max;
		public String type; 
		public float[] values = new float[10];
		public Rectangle TextureResource;

		public int add(int x){
			x-=count+x>max?max-count:0;
			count+=x;
			return x;
		}
		public bool remove(int x){
			count=count-x<0?0:count-x;
			return !(count - x < 0);
		}
		public void setCount(int x){
			count = x;
		}
	}


}

