using System;

namespace ProjectCrusade
{
	public enum TileType 
	{
		Air = 0,
		Floor = 1,
		Wall = 2,
	}
	public struct Tile
	{
		//We don't use getters/setters because this is a struct, which encapsulates only data
		public TileType Type;
		public bool Solid;

		public Tile (TileType type, bool solid = true)
		{
			Type = type;
			Solid = solid;
		}
	}
}

