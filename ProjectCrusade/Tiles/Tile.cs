using System;

namespace ProjectCrusade
{
	public enum TileType 
	{
		Air = 0,
		Floor = 1,
	}
	public struct Tile
	{
		public TileType Type;

		public Tile (TileType type)
		{
			Type = type;
		}

	}
}

