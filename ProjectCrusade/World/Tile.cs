using System;
using Microsoft.Xna.Framework;

namespace ProjectCrusade
{
	public enum TileType 
	{
		Air=0,
		Grass=1,
		TreeTop=2,
		FlowersGrass=3,
		TreeBottom=18,
		SandCornersTopLeft=34,
		SandTop = 35,
		SandCornersTopRight=36,
		Sand=51,
		SandSideRight=52,
		SandCornersBottomLeft=66,
		SandBottom=67,
		SandCornersBottomRight=68,

		CaveFloor = 21,
		CaveFloorBottomRight = 6,
		CaveFloorBottomLeft = 7,
		CaveFloorTopLeft=22,
		CaveFloorTopRight=23,

		CaveWall=54,
		CaveWallTopLeft=37,
		CaveWallTopRight = 39,
		CaveWallBottomLeft = 69,
		CaveWallBottomRight = 71,
		CaveWallTop = 38,
		CaveWallBottom = 70,
		CaveWallLeft = 53,
		CaveWallRight=55,

		CaveRock = 5
	}
	public struct Tile
	{
		//We don't use getters/setters because this is a struct, which encapsulates only data
		public TileType Type;
		public bool Solid;
		public Vector3 Color; //used for lighting purposes

		public enum Orientation
		{
			Up,
			Left,
			Down,
			Right
		}

		Orientation TileOrientation;

		public float Rotation {
			get { 
				switch (this.TileOrientation) {
				case Orientation.Up:
					return 0 * (float)Math.PI / 2;
				case Orientation.Left:
					return 1 * (float)Math.PI / 2;
				case Orientation.Down:
					return 2 * (float)Math.PI / 2;
				case Orientation.Right:
					return 3 * (float)Math.PI / 2;
				}
				return 0;
			}
		}

		public Tile (TileType type, bool solid, Vector3 color, Orientation orientation = Orientation.Up)
		{
			Type = type;
			Solid = solid;
			Color = color;
			TileOrientation = orientation;
		}
	}
}

