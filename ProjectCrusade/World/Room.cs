using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.IO;

namespace ProjectCrusade
{
	public struct Room
	{
		Rectangle Rect;

		public Room(Rectangle rect, ref Tile[,] world, string filename)
		{
			Rect = rect;
			generateRoom (ref world, filename);
		}

		void generateRoom(ref Tile[,] world, string filename)
		{

			XmlReader reader = XmlReader.Create (filename);

			XmlDocument doc = new XmlDocument ();
			doc.Load (reader);
			reader.Close ();
			int width = Convert.ToInt32(((XmlElement)doc.SelectSingleNode ("map")).GetAttribute ("width"));
			int height=Convert.ToInt32(((XmlElement)doc.SelectSingleNode("map")).GetAttribute("height"));
			Tile[,] roomTiles = new Tile[width, height];

			foreach (XmlElement layer in doc.SelectNodes("map/layer"))
			{
				string layerName = layer.GetAttribute ("name");
				//CSV data
				string layerData = layer.SelectSingleNode ("data").InnerText;
				StringReader sread = new StringReader (layerData);
				for (int j = 0; j < height; j++) {
					string line = sread.ReadLine ();
					if (line == "")
						continue;
					var vals = line.Split (',');

					if (vals.Length < width)
						throw new Exception ("Floor file format does not match width!");
					for (int i = 0; i < width; i++) {
						int v = Convert.ToInt32 (vals [i]);

						
						//Only include a wall file if not air
						//Overwrites floor tiles
						if (v!= 0) 
							roomTiles [i, j] = new Tile((TileType)(v-1), layerName!="Floor", new Vector3(1,1,1));
						else if (layerName=="Floor")
							roomTiles [i, j] = new Tile(TileType.Air, false, new Vector3(1,1,1));

					}
				}
			}
			//TODO: Add error handling
			for (int i = 0; i < Rect.Width; i++)
				for (int j = 0; j < Rect.Height; j++) {
					world[i+Rect.Left,j+Rect.Right] = roomTiles[i,j];
				}
		}
	}
}

