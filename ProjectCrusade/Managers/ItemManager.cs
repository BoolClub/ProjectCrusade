using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using System.Xml;
using System.IO;


namespace ProjectCrusade
{


	//Item types: equipment, tool, consumable, misc
	//Item values: 
	//	consumable:
	//		HEALTH, change amount, effect time, effect length (-1 is permanent), EMPTY

	public static class ItemManager
	{
		public static Item GetItem(string d){
			return Data[d];
		}

		const string ItemFilePath = "Content/items.xml";

		static ItemManager()
		{
			//Initialize items from XML file. 
			XmlDocument document = new XmlDocument ();
			if (!File.Exists(ItemFilePath)) throw new Exception("Item file does not exist!");
			document.Load (new FileStream (ItemFilePath, FileMode.Open));
			foreach (XmlElement element in document.SelectNodes("items/item")) {
				Item item = new Item ();
				foreach (XmlElement property in element) {
					switch (property.Name) {
					case "name":
						item.Name = property.InnerText;
						break;
					case "identifier":
						item.Identifier = property.InnerText;
						break;
					case "tooltip":
						item.Tooltip = property.InnerText;
						break;
					case "max":
						item.Max = Convert.ToInt32(property.InnerText);
						break;
					case "type":
						item.Type = property.InnerText;
						break;
					case "spriteId":
						item.TextureResource = getTextureSourceRect(Convert.ToInt32 (element.GetElementsByTagName ("spriteId") [0].InnerText));
						break;
					}
				}
				Items [item.Identifier] = item;
			}

		}


		private static Rectangle getTextureSourceRect(int spriteSheetId)
		{
			int sheetWidthSprites = Item.SpriteSheetWidth / Item.SpriteWidth;
			int x = spriteSheetId % sheetWidthSprites;
			int y = spriteSheetId / sheetWidthSprites;
			return new Rectangle (x * Item.SpriteWidth, y * Item.SpriteWidth, Item.SpriteWidth, Item.SpriteWidth);
		}

		public static Item Create(string identifier, int count = 1)
		{
			
			if (!Items.ContainsKey (identifier))
				throw new Exception ("Item with that identifier does not exist!");
			Item item = (Item)Items [identifier].ShallowCopy ();
			item.Count = count;
			return item;
		}

		private static Dictionary<string, Item> Items = new Dictionary<string, Item>();

		public static Dictionary<string, Item> Data = new Dictionary<string, Item>()
		{
//			{ "gold", new Item{Identifier = "gold", Name="Gold", Tooltip="Lovely money.", Max=1000, Type="misc", TextureResource=GetTextureSourceRect("GoldCoin"),} },
//			{ "apple", new Item{Identifier = "apple", Name="Apple", Tooltip="Tasty and red.", Max=12, Type="consumable", TextureResource=GetTextureSourceRect("Apple"),
//				Behavior=new TupleList<string, float, int, int, int>{
//						{"HEALTH", 10, 0, -1, 0}
//			}}},
//			{ "bread", new Item{Identifier = "bread", Name="Bread", Tooltip="A big loaf of bread", Max=12, Type="consumable", TextureResource=GetTextureSourceRect("Bread"),
//					Behavior=new TupleList<string, float, int, int, int>{
//						{"HEALTH", 5, 0, -1, 0}
//					}}},
//			{ "water", new Item{Identifier = "water", Name="Water", Tooltip="Nice, refreshing water", Max=20, Type="consumable", TextureResource=GetTextureSourceRect("Water"),
//					Behavior=new TupleList<string, float, int, int, int>{
//						{"HEALTH", 2, 0, -1, 0}
//					}}}
//			
		};
	}

	public class TupleList<T1, T2, T3, T4, T5> : List<Tuple<T1, T2, T3, T4, T5>>
	{
		public void Add( T1 i1, T2 i2, T3 i3, T4 i4, T5 i5 )
		{
			Add( new Tuple<T1, T2, T3, T4, T5>( i1, i2, i3, i4, i5 ) );
		}
	}
}

