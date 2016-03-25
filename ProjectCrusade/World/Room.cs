using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace ProjectCrusade
{
	public class Room
	{
		public Rectangle Rect;

		public Vector2 Center { get { return World.TileWidth * Rect.Center.ToVector2 (); } }

		const int PaddingSpace = 2;
		public Rectangle ExpandedRect { get { return new Rectangle (Rect.Left - PaddingSpace, Rect.Top - PaddingSpace, Rect.Width + 2 * PaddingSpace, Rect.Height + 2 * PaddingSpace); } }

		string file;

		public List<Point> Entrances;

		/// <summary>
		/// Key value pairs of NPC locations, where positions are in absolute world coordinates.
		/// </summary>
		public List<Entity> NPCs;

		/// <summary>
		/// Key value pairs of objective locations, where rectangle locations are absolute (world coordinates)
		/// </summary>
		public List<Tuple<string, Rectangle>> Objectives;

		public Room(Point position, string filename)
		{
			Entrances = new List<Point> ();
			Objectives = new List<Tuple<string, Rectangle>> ();
			NPCs = new List<Entity> ();
			file = filename;
			XmlReader reader = XmlReader.Create (file);

			XmlDocument doc = new XmlDocument ();
			doc.Load (reader);
			int width = Convert.ToInt32(((XmlElement)doc.SelectSingleNode ("map")).GetAttribute ("width"));
			int height=Convert.ToInt32(((XmlElement)doc.SelectSingleNode("map")).GetAttribute("height"));
			reader.Close ();

			Rect = new Rectangle (position.X, position.Y, width, height);
		}



		public void GenerateRoom(List<WorldLayer> layers, ref List<Light> lights, TieredPropertySelector propertySelector, Random rand)
		{
			int layerCount = layers.Count;
			XmlReader reader = XmlReader.Create (file);

			XmlDocument doc = new XmlDocument ();
			doc.Load (reader);
			reader.Close ();
			int width = Convert.ToInt32(((XmlElement)doc.SelectSingleNode ("map")).GetAttribute ("width"));
			int height=Convert.ToInt32(((XmlElement)doc.SelectSingleNode("map")).GetAttribute("height"));

			if (width != Rect.Width || height != Rect.Height)
				throw new Exception ("Dimensions of rectangle do not match dimensions of file!");

			Tile[,,] roomTiles = new Tile[width, height, layerCount];

			foreach (XmlElement layer in doc.SelectNodes("map/layer"))
			{
				string layerName = layer.GetAttribute ("name");
				//CSV data
				string layerData = layer.SelectSingleNode ("data").InnerText;
				StringReader sread = new StringReader (layerData);
				//Because of newline character at beginning of string
				sread.ReadLine ();
				for (int j = 0; j < height; j++) {
					string line = sread.ReadLine ();
					var vals = line.Split (',');

					if (vals.Length < width)
						throw new Exception ("Floor file format does not match width!");
					for (int i = 0; i < width; i++) {
						int v = Convert.ToInt32 (vals [i]);

						
						//Only include a wall file if not air
						//Overwrites floor tiles
						if (v != 0) 
							roomTiles [i, j, layerName=="Floor" ? 0 : 1] = new Tile((TileType)(v-1), layerName!="Floor", new Vector3(1,1,1));
						else
							roomTiles [i, j, layerName=="Floor" ? 0 : 1] = new Tile(TileType.Air, false, new Vector3(1,1,1));
						
					}
				}
			}
			makeObjectives (doc);
			makeLights (doc, ref lights);
			makeNPCs (doc, propertySelector, rand);

			//Get entrances
			//An entrance is defined as any non-solid tile on the border of the room.
			for (int i = 0; i < width; i++) {
				if (!roomTiles [i, 0, 1].Solid)
					Entrances.Add (new Point (i, 0));
				if (!roomTiles [i, height-1, 1].Solid)
					Entrances.Add (new Point (i, height-1));
			}
			for (int j = 1; j < height - 1; j++) {
				if (!roomTiles [0, j, 1].Solid)
					Entrances.Add (new Point (0, j));
				if (!roomTiles [width-1, j, 1].Solid)
					Entrances.Add (new Point (width-1, j));
			}
			//TODO: Add error handling
			for (int l = 0; l<layerCount;l++)
				for (int i = 0; i < Rect.Width; i++)
					for (int j = 0; j < Rect.Height; j++) {
						layers[l].Tiles[i+Rect.Left,j+Rect.Top] = roomTiles[i,j,l];
					}
		}

		/// <summary>
		/// Looks for and places any objectives indicated in the TMX file
		/// </summary>
		void makeObjectives(XmlDocument doc)
		{
			foreach (XmlElement objective in doc.SelectNodes("map/objectgroup[@name='Objectives']/object")) {

				Rectangle objRect = new Rectangle (
					int.Parse (objective.GetAttribute ("x")),
					int.Parse (objective.GetAttribute ("y")), 
					int.Parse (objective.GetAttribute ("width")),
					int.Parse (objective.GetAttribute ("height")));
				//shift objective position relative to room's world coordinates
				objRect.Offset (new Point(Rect.X * World.TileWidth, Rect.Y * World.TileWidth));
				Objectives.Add (new Tuple<string, Rectangle> (objective.GetAttribute ("name"), objRect));
			}
		}

		/// <summary>
		/// Looks for and places any lights indicated in the TMX file
		/// </summary>
		void makeLights(XmlDocument doc, ref List<Light> lights)
		{
			foreach (XmlElement light in doc.SelectNodes("map/objectgroup[@name='Lights']/object")) {

				Point lightPos = new Point (
					(int)float.Parse (light.GetAttribute ("x")),
					(int)float.Parse(light.GetAttribute ("y"))); 
				//shift light position relative to room's world coordinates
				lightPos += new Point(Rect.X * World.TileWidth, Rect.Y * World.TileWidth);
				float brightness = float.Parse(light.SelectSingleNode ("properties/property[@name='brightness']").Attributes ["value"].Value);
				float red = float.Parse(light.SelectSingleNode ("properties/property[@name='red']").Attributes ["value"].Value);
				float green = float.Parse(light.SelectSingleNode ("properties/property[@name='green']").Attributes ["value"].Value);
				float blue = float.Parse(light.SelectSingleNode ("properties/property[@name='blue']").Attributes ["value"].Value);
				lights.Add (new Light (lightPos.ToVector2(), new Color(red,green,blue), brightness, true));
			}
		}

		/// <summary>
		/// Looks for and places any entities indicated in the TMX file
		/// </summary>
		void makeNPCs(XmlDocument doc, TieredPropertySelector propertySelector, Random rand)
		{
			foreach (XmlElement npc in doc.SelectNodes("map/objectgroup[@name='Entities']/object")) {
				Point npcPos = new Point (
					(int)float.Parse (npc.GetAttribute ("x")),
					(int)float.Parse(npc.GetAttribute ("y"))); 
				//shift npc position relative to room's world coordinates
				npcPos += new Point(Rect.X * World.TileWidth, Rect.Y * World.TileWidth);
				string name = npc.SelectSingleNode ("properties/property[@name='name']").Attributes["value"].Value;
				switch (name) {
				case "npc":
					NPC np = new NPC ("Test");
					np.Position = npcPos.ToVector2 ();

					string msg = npc.SelectSingleNode ("properties/property[@name='message']").Attributes ["value"].Value;

					//Split message by backslash character, which should be relatively uncommon and is therefore a safe "new message" delimiter
					var msgs = msg.Split ('\\');
					foreach (string m in msgs) np.TextBox.AddText (m);
					NPCs.Add(np);
					break;
				case "chest":
					string typename = npc.SelectSingleNode ("properties/property[@name='type']").Attributes ["value"].Value;

					Type T = Type.GetType ("ProjectCrusade." + typename);
					Item i = (Item)Activator.CreateInstance (T);

					//If i is a weapon, add random weapon properties based on the level's respective probabilities (encoded in the TieredPropertySelector object)
					if (i is WeaponItem) {
						WeaponItem w = i as WeaponItem;

						w.TierOne = propertySelector.RandomPropertyOne (rand);
						w.TierTwo = propertySelector.RandomPropertyTwo (rand);
						w.TierThree = propertySelector.RandomPropertyThree (rand);
					}

					Chest c = new Chest (i);

					var cNode = npc.SelectSingleNode ("properties/property[@name='count']");
					if (cNode != null) {
						i.Count = int.Parse(cNode.Attributes ["value"].Value);
					}
					c.Position = npcPos.ToVector2 ();

					NPCs.Add (c);
					break;
				}
			}
		}

	}
}

