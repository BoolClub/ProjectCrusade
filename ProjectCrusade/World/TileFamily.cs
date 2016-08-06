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

		public abstract TileType Rock 				{ get; }
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

			public override TileType Rock				{ get { return TileType.CaveRock; }   }   
		}
		public class IceCave : TileFamily
		{
			public override TileType Floor				{ get { return TileType.IceCaveFloor; } }
			public override TileType FloorTopLeft		{ get { return TileType.IceCaveFloorTopLeft; }   }
			public override TileType FloorTopRight		{ get { return TileType.IceCaveFloorTopRight; }   }
			public override TileType FloorBottomLeft 	{ get { return TileType.IceCaveFloorBottomLeft; }   }
			public override TileType FloorBottomRight	{ get { return TileType.IceCaveFloorBottomRight; }   }

			public override TileType Wall				{ get { return TileType.IceCaveWall; }   }
			public override TileType WallTopLeft     	{ get { return TileType.IceCaveWallTopLeft; }   }
			public override TileType WallTopRight    	{ get { return TileType.IceCaveWallTopRight; }   }
			public override TileType WallBottomLeft  	{ get { return TileType.IceCaveWallBottomLeft; }   }
			public override TileType WallBottomRight	{ get { return TileType.IceCaveWallBottomRight; }   }
			public override TileType WallTop         	{ get { return TileType.IceCaveWallTop; }   }
			public override TileType WallBottom      	{ get { return TileType.IceCaveWallBottom; }   }
			public override TileType WallLeft        	{ get { return TileType.IceCaveWallLeft; }   }
			public override TileType WallRight			{ get { return TileType.IceCaveWallRight; }   }   

			public override TileType Rock				{ get { return TileType.IceCaveRock; }   }   
		}




		public class SandCave : TileFamily
		{
			public override TileType Floor				{ get { return TileType.SandCaveFloor; } }
			public override TileType FloorTopLeft		{ get { return TileType.SandCaveFloorTopLeft; }   }
			public override TileType FloorTopRight		{ get { return TileType.SandCaveFloorTopRight; }   }
			public override TileType FloorBottomLeft 	{ get { return TileType.SandCaveFloorBottomLeft; }   }
			public override TileType FloorBottomRight	{ get { return TileType.SandCaveFloorBottomRight; }   }

			public override TileType Wall				{ get { return TileType.SandCaveWall; }   }
			public override TileType WallTopLeft     	{ get { return TileType.SandCaveWallTopLeft; }   }
			public override TileType WallTopRight    	{ get { return TileType.SandCaveWallTopRight; }   }
			public override TileType WallBottomLeft  	{ get { return TileType.SandCaveWallBottomLeft; }   }
			public override TileType WallBottomRight	{ get { return TileType.SandCaveWallBottomRight; }   }
			public override TileType WallTop         	{ get { return TileType.SandCaveWallTop; }   }
			public override TileType WallBottom      	{ get { return TileType.SandCaveWallBottom; }   }
			public override TileType WallLeft        	{ get { return TileType.SandCaveWallLeft; }   }
			public override TileType WallRight			{ get { return TileType.SandCaveWallRight; }   }   

			public override TileType Rock				{ get { return TileType.SandCaveRock; }   }   
		}

		public class GreenCave : TileFamily
		{
			public override TileType Floor { get { return TileType.GreenCaveFloor; } }
			public override TileType FloorTopLeft { get { return TileType.GreenCaveFloorTopLeft; } }
			public override TileType FloorTopRight { get { return TileType.GreenCaveFloorTopRight; } }
			public override TileType FloorBottomLeft { get { return TileType.GreenCaveFloorBottomLeft; } }
			public override TileType FloorBottomRight { get { return TileType.GreenCaveFloorBottomRight; } }

			public override TileType Wall { get { return TileType.GreenCaveWall; } }
			public override TileType WallTopLeft { get { return TileType.GreenCaveWallTopLeft; } }
			public override TileType WallTopRight { get { return TileType.GreenCaveWallTopRight; } }
			public override TileType WallBottomLeft { get { return TileType.GreenCaveWallBottomLeft; } }
			public override TileType WallBottomRight { get { return TileType.GreenCaveWallBottomRight; } }
			public override TileType WallTop { get { return TileType.GreenCaveWallTop; } }
			public override TileType WallBottom { get { return TileType.GreenCaveWallBottom; } }
			public override TileType WallLeft { get { return TileType.GreenCaveWallLeft; } }
			public override TileType WallRight { get { return TileType.GreenCaveWallRight; } }   

			public override TileType Rock { get { return TileType.GreenCaveRock; } }   
		}
	}
}

