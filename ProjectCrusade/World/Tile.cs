using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	public enum TileType 
	{
		Air=0,
		Grass=18,
		TreeTop=1,
		FlowersGrass=2,
		TreeBottom=17,
		SandCornersTopLeft=33,
		SandTop = 34,
		SandCornersTopRight=35,
		Sand=50,
		SandSideRight=51,
		SandCornersBottomLeft=65,
		SandBottom=66,
		SandCornersBottomRight=67,

		CaveFloor = 20,
		CaveFloorBottomRight = 5,
		CaveFloorBottomLeft = 6,
		CaveFloorTopLeft=21,
		CaveFloorTopRight=21,

		CaveWall=53,
		CaveWallTopLeft=36,
		CaveWallTopRight = 38,
		CaveWallBottomLeft = 68,
		CaveWallBottomRight = 70,
		CaveWallTop = 37,
		CaveWallBottom = 69,
		CaveWallLeft = 52,
		CaveWallRight=53,

		CaveRock = 4,


		IceCaveFloor = 22,
		IceCaveFloorBottomRight = 8,
		IceCaveFloorBottomLeft = 9,
		IceCaveFloorTopLeft=24,
		IceCaveFloorTopRight=25,

		IceCaveWall=55,
		IceCaveWallTopLeft=39,
		IceCaveWallTopRight = 41,
		IceCaveWallBottomLeft = 70,
		IceCaveWallBottomRight = 73,
		IceCaveWallTop = 40,
		IceCaveWallBottom = 72,
		IceCaveWallLeft = 55,
		IceCaveWallRight=57,

		IceCaveRock = 4,


		SandCaveFloor = 26,
		SandCaveFloorBottomRight = 11,
		SandCaveFloorBottomLeft = 12,
		SandCaveFloorTopLeft=27,
		SandCaveFloorTopRight=28,

		SandCaveWall=59,
		SandCaveWallTopLeft=42,
		SandCaveWallTopRight = 44,
		SandCaveWallBottomLeft = 74,
		SandCaveWallBottomRight = 76,
		SandCaveWallTop = 43,
		SandCaveWallBottom = 75,
		SandCaveWallLeft = 58,
		SandCaveWallRight=60,

		SandCaveRock = 4,

		GreenCaveFloor = 202,
		GreenCaveFloorBottomRight = 187,
		GreenCaveFloorBottomLeft = 188,
		GreenCaveFloorTopLeft=203,
		GreenCaveFloorTopRight=204,

		GreenCaveWall=		235,
		GreenCaveWallTopLeft=218,
		GreenCaveWallTopRight = 220,
		GreenCaveWallBottomLeft = 250,
		GreenCaveWallBottomRight = 252,
		GreenCaveWallTop = 219,
		GreenCaveWallBottom = 251,
		GreenCaveWallLeft = 234,
		GreenCaveWallRight=236,

		GreenCaveRock = 4
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

		static bool[] transparentValues;
		public static void CheckTileTransparency(TextureManager textureManager)
		{
			Texture2D texture = textureManager.GetTexture ("tiles");
			int swidth = World.SpriteSheetWidth / World.TileWidth;
			transparentValues = new bool[swidth * swidth];

			for (int i = 0; i < swidth; i++)
				for (int j = 0; j < swidth; j++) {
					Color[] subdata = new Color[World.TileWidth * World.TileWidth];
					texture.GetData<Color>(0, new Rectangle(i*World.TileWidth,j*World.TileWidth,World.TileWidth,World.TileWidth), subdata, 0, World.TileWidth*World.TileWidth);
					bool anyTransparent = false;

					foreach (Color c in subdata) {
						if (c.A < byte.MaxValue) {
							anyTransparent = true;
							break;
						}
					}
					transparentValues [i + j * swidth] = anyTransparent;
				}
		}


		public bool IsTransparent { get { return transparentValues [(int)Type]; } }

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

