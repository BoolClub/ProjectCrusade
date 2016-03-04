using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
namespace ProjectCrusade
{


	//Item types: equipment, tool, consumable, misc
	//Item values: 
	//	consumable:
	//		HEALTH, change amount, effect time, effect length (-1 is permanent), EMPTY

	public class ItemManager
	{
		public Item getItemm(String d){
			return Data[d];
		}
		public static Rectangle getTextureSourceRect(String identifier)
		{
			int sheetWidthSprites = 256 / 32;
			int num = (int)Enum.Parse (typeof(Sprite), identifier);
			int x = num % sheetWidthSprites;
			int y = num / sheetWidthSprites;
			return new Rectangle (x * 32, y * 32, 32, 32);
		}

		public Dictionary<string, Item> Data = new Dictionary<string, Item>()
		{
			{ "gold", new Item{Identifier = "gold", Name="Gold", Tooltip="Lovely money.", Max=1000, Type="misc", TextureResource=getTextureSourceRect("GoldCoin"),} },
			{ "apple", new Item{Identifier = "apple", Name="Apple", Tooltip="Tasty and red.", Max=12, Type="consumable", TextureResource=getTextureSourceRect("Apple"),
				Behavior=new TupleList<string, float, int, int, int>{
						{"HEALTH", 10, 0, -1, 0}
			}}},
			{ "bread", new Item{Identifier = "bread", Name="Bread", Tooltip="A big loaf of bread", Max=12, Type="consumable", TextureResource=getTextureSourceRect("Bread"),
					Behavior=new TupleList<string, float, int, int, int>{
						{"HEALTH", 5, 0, -1, 0}
					}}},
			{ "water", new Item{Identifier = "water", Name="Water", Tooltip="Nice, refreshing water", Max=20, Type="consumable", TextureResource=getTextureSourceRect("Water"),
					Behavior=new TupleList<string, float, int, int, int>{
						{"HEALTH", 2, 0, -1, 0}
					}}}
			
		};

		private enum Sprite {
			Apple		 	= 0,
			Water		 	= 1,
			GoldCoin	 	= 2,
			WoodenSword 	= 3,
			StarterArrow	= 4,
			MagicWand		= 5,
			Bread			= 6,
			StoneSword		= 7,
			IronSword		= 8,
		}
	}

	public class TupleList<T1, T2, T3, T4, T5> : List<Tuple<T1, T2, T3, T4, T5>>
	{
		public void Add( T1 i1, T2 i2, T3 i3, T4 i4, T5 i5 )
		{
			Add( new Tuple<T1, T2, T3, T4, T5>( i1, i2, i3, i4, i5 ) );
		}
	}
}

