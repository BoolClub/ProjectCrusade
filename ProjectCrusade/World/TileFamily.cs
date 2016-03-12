using System;

namespace ProjectCrusade
{
	public abstract class TileFamily
	{
		public abstract TileType Floor				{ get; }
		public abstract TileType FloorTopLeft		{ get; }
		public abstract TileType FloorTopRight		{ get; }
		public abstract TileType FloorBottomLeft 	{ get; }
		public abstract TileType FloorBottomRight	{ get; }

		public abstract TileType Wall				{ get; }
		public abstract TileType WallTopLeft     	{ get; }
		public abstract TileType WallTopRight    	{ get; }
		public abstract TileType WallBottomLeft  	{ get; }
		public abstract TileType WallBottomRight	{ get; }
		public abstract TileType WallTop         	{ get; }
		public abstract TileType WallBottom      	{ get; }
		public abstract TileType WallLeft        	{ get; }
		public abstract TileType WallRight			{ get; }                                
	}
	namespace TileFamilies
	{
		public class Cave : TileFamily
		{
			public override TileType Floor				{ get { return TileType.CaveFloor; } }
			public override TileType FloorTopLeft		{ get { return TileType.CaveFloorTopLeft; }   }
			public override TileType FloorTopRight		{ get { return TileType.CaveFloorTopRight; }   }
			public override TileType FloorBottomLeft 	{ get { return TileType.CaveFloorBottomLeft; }   }
			public override TileType FloorBottomRight	{ get { return TileType.CaveFloorBottomRight; }   }

			public override TileType Wall				{ get { return TileType.CaveWall; }   }
			public override TileType WallTopLeft     	{ get { return TileType.CaveWallTopLeft; }   }
			public override TileType WallTopRight    	{ get { return TileType.CaveWallTopRight; }   }
			public override TileType WallBottomLeft  	{ get { return TileType.CaveWallBottomLeft; }   }
			public override TileType WallBottomRight	{ get { return TileType.CaveWallBottomRight; }   }
			public override TileType WallTop         	{ get { return TileType.CaveWallTop; }   }
			public override TileType WallBottom      	{ get { return TileType.CaveWallBottom; }   }
			public override TileType WallLeft        	{ get { return TileType.CaveWallLeft; }   }
			public override TileType WallRight			{ get { return TileType.CaveWallRight; }   }   
		}
	}
}

