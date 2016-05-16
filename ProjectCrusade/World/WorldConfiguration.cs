using System;
using System.Collections.Generic;

namespace ProjectCrusade
{
	public class WorldConfiguration
	{
		public List<string> RoomNames;
		public TileFamily TileFamily;
		public string TieredPropertyFileName;


		public WorldConfiguration ()
		{
			RoomNames = new List<string> ();
		}

		/// <summary>
		/// Add a group of rooms to the world configuration.
		/// </summary>
		/// <param name="name">Room file name</param>
		/// <param name="multiplicity">Number of rooms to add</param>
		public void AddRooms(string name, int multiplicity)
		{
			for (int i = 0; i < multiplicity; i++)
				RoomNames.Add (name);
		}
	}
}

