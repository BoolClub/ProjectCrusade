using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ProjectCrusade
{
	/// <summary>
	/// Handles generating hallways
	/// </summary>
	public class MazeGenerator
	{
		/// <summary>
		/// Maze data. 
		/// 0 represents unfilled region
		/// -1 represents maze walls
		/// -2 represents a rock
		/// 1 and larger represents an individual room
		/// </summary>
		int[,] maze;
		int width, height;

		int roomCount = 0;
		Random rand = new Random();

		public bool PruneDeadEnds { get; set; }
		public bool ClearThinWalls { get; set; }
		public bool RemoveFloatingPillars { get; set; }
		public bool AddRocks { get; set; }
		public bool MakeBorder { get; set; }

		public double RockGenerationProbability { get; set; }
		public double PruningProbability { get; set; }

		public MazeGenerator (int width, int height)
		{
			this.width = width;
			this.height = height;
			maze = new int[width, height];

			//Set default configuration.

			PruneDeadEnds = true;
			ClearThinWalls = true;
			RemoveFloatingPillars = true;
			AddRocks = true;
			MakeBorder = true;
			RockGenerationProbability = 0.03;
			PruningProbability = 1.0;
		}

		public void ShadeRoom(Room room)
		{
			roomCount++;
			//Fill room
			for (int i = 0; i < room.Rect.Width; i++) {
				for (int j = 0; j < room.Rect.Height; j++) {
					maze [room.Rect.Left + i, room.Rect.Top + j] = roomCount;
				}
			}
			foreach (Point entrance in room.Entrances) {
				Point globalPos = entrance + room.Rect.Location;
				if (globalPos.X - 1 >= 0 && globalPos.X+1< width)
				if (maze [globalPos.X - 1, globalPos.Y] == roomCount)
					maze [globalPos.X + 1, globalPos.Y] = -1;
				if (globalPos.X - 1 >= 0 && globalPos.X+1< width)
				if (maze [globalPos.X + 1, globalPos.Y] == roomCount)
					maze [globalPos.X - 1, globalPos.Y] = -1;
				if (globalPos.Y - 1 >= 0 && globalPos.Y+1< height)
				if (maze [globalPos.X, globalPos.Y-1] == roomCount)
					maze [globalPos.X, globalPos.Y+1] = -1;
				if (globalPos.Y - 1 >= 0 && globalPos.Y+1< height)
				if (maze [globalPos.X, globalPos.Y+1] == roomCount)
					maze [globalPos.X, globalPos.Y-1] = -1;
			}
		}

		/// <summary>
		/// Returns a list of all possible maze moves around a given point (determined by whether or not there are tiles with value 0 in neighborhood).
		/// </summary>
		List<int> getTraversalOptions(Point p)
		{
			List<int> options = new List<int> ();

			/*
			 * 0: traverse right
			 * 1: traverse down
			 * 2: traverse left
			 * 3: traverse up
			 * */
			if (p.X < 0 || p.X >= width || p.Y < 0 || p.Y >= height)
				return options;
			if (p.X + 2 < width)
			if (maze [p.X + 2, p.Y] == 0 && maze [p.X + 1, p.Y] == 0)
				options.Add(0);
			if (p.Y+ 2 < height)
			if (maze [p.X, p.Y+2] == 0 && maze [p.X, p.Y+1] == 0)
				options.Add(1);

			if (p.X - 2 >= 0)
			if (maze [p.X - 2, p.Y] == 0 && maze [p.X - 1, p.Y] == 0)
				options.Add(2);
			if (p.Y- 2 >= 0)
			if (maze [p.X, p.Y-2] == 0 && maze [p.X, p.Y-1] == 0)
				options.Add(3);
			return options;
		}

		public bool IsHall(int i, int j) {
			return maze [i, j] == -1;
		}

		public bool IsRoom(int i, int j) {
			return maze [i, j] > 0;
		}

		public Tile GetMazeTile(TileFamily family, int i, int j)
		{
			//If wall
			if (maze [i, j] == 0) {
				TileType ttype = family.Wall;

				if (i - 1 >= 0 && j - 1 >= 0)
				if (maze [i - 1, j] == -1 && maze [i, j - 1] == -1)
					ttype = family.WallTopLeft;
				if (i + 1 < width && j - 1 >= 0)
				if (maze [i + 1, j] == -1 && maze [i, j - 1] == -1)
					ttype = family.WallTopRight;
				if (i - 1 >= 0 && j + 1 < height)
				if (maze [i - 1, j] == -1 && maze [i, j + 1] == -1)
					ttype = family.WallBottomLeft;
				if (i + 1 < width && j + 1 < height)
				if (maze [i + 1, j] == -1 && maze [i, j + 1] == -1)
					ttype = family.WallBottomRight;
				if (j - 1 >= 0)
				if (maze [i, j - 1] == -1 && ttype==family.Wall)
					ttype = family.WallTop;
				if (j + 1 <height)
				if (maze [i, j + 1] == -1 && ttype==family.Wall)
					ttype = family.WallBottom;
				if (i - 1 >= 0)
				if (maze [i-1, j] == -1 && ttype==family.Wall)
					ttype = family.WallLeft;
				if (i+1 <width)
				if (maze [i+1, j] == -1 && ttype==family.Wall)
					ttype = family.WallRight;

				if (i + 1 < width && j + 1 < height)
				if (maze [i + 1, j + 1] == -1 && maze [i + 1, j] == 0 && maze [i, j + 1] == 0)
					ttype = family.FloorTopLeft;
				if (i - 1 >= 0 && j + 1 < height)
				if (maze [i - 1, j + 1] == -1 && maze [i - 1, j] == 0 && maze [i, j + 1] == 0)
					ttype = family.FloorTopRight;
				if (i + 1 < width && j - 1 >= 0)
				if (maze [i + 1, j - 1] == -1 && maze [i + 1, j] == 0 && maze [i, j - 1] == 0)
					ttype = family.FloorBottomLeft;
				if (i - 1 >= 0 && j - 1 >= 0)
				if (maze [i - 1, j - 1] == -1 && maze [i - 1, j] == 0 && maze [i, j - 1] == 0)
					ttype = family.FloorBottomRight;

				return new Tile (ttype, true, Color.White.ToVector3 ());
			} else {
				if (maze[i,j]==-2) return new Tile (family.Rock, true, Color.White.ToVector3 ());
				return new Tile (family.Floor, false, Color.White.ToVector3 ());
			}
		}

		public void Generate()
		{
			makeMaze ();
			if (PruneDeadEnds) pruneDeadEnds ();
			if (ClearThinWalls) clearThinWalls ();
			if (RemoveFloatingPillars) removeFloatingPillars ();
			if (MakeBorder) makeBorder ();
			if (AddRocks) addRocks ();
		}

		/// <summary>
		/// Number of nearby neighbors with maze value 0. Used to look for dead ends, etc. 
		/// </summary>
		int numNeighborsSolid(Point p)
		{
			int neighbors = 0;
			if (p.X+1 < width) if (maze [p.X + 1, p.Y] == 0)
				neighbors++;
			if (p.X-1>=0)if (maze [p.X - 1, p.Y] == 0)
				neighbors++;
			if (p.Y+1 < height) if (maze [p.X, p.Y+1] == 0)
				neighbors++;
			if (p.Y-1 >= 0) if (maze [p.X, p.Y-1] ==0)
				neighbors++;
			return neighbors;	
		}

		void makeMaze()
		{
			Stack<Point> finishedTiles = new Stack<Point> ();
			Point currTile = new Point (0, 0);
			maze [currTile.X, currTile.Y] = -1;
			List<int> options = new List<int> ();
			//TODO: change loop condition
			for (int i = 0; i<width*height;i++)
			{
				options = getTraversalOptions (currTile);
				if (options.Count > 0) {
					int selectedOption = options[rand.Next (0, options.Count)];
					Point newTile = new Point (0, 0);
					switch (selectedOption) {
					case 0:
						newTile = new Point (currTile.X + 2, currTile.Y);
						maze [currTile.X + 1, currTile.Y] = -1;
						break;
					case 1:
						newTile = new Point (currTile.X, currTile.Y+2);
						maze [currTile.X, currTile.Y+1] = -1;
						break;
					case 2:
						newTile = new Point (currTile.X - 2, currTile.Y);
						maze [currTile.X - 1, currTile.Y] = -1;
						break;
					case 3:
						newTile = new Point (currTile.X, currTile.Y-2);
						maze [currTile.X, currTile.Y-1] = -1;
						break;

					}
					finishedTiles.Push (currTile);
					currTile = newTile;
					maze [currTile.X, currTile.Y] = -1;
				} else if (finishedTiles.Count > 0) {
					currTile = finishedTiles.Pop();
				}
			}
		}

		void pruneDeadEnds()
		{
			List<Point> deadEnds = new List<Point>();
			for (int i = 1; i < width-1; i++) {
				for (int j = 1; j < height-1; j++) {
					Point p = new Point (i, j);
					if (numNeighborsSolid (p) == 3)
						deadEnds.Add (p);
				}
			}

			foreach (Point deadEnd in deadEnds) {
				if (rand.NextDouble () > PruningProbability)
					continue;
				Point currTile = deadEnd;

				int i = 0; 
				while ((neighbors = numNeighborsSolid (currTile)) ==3 && i < 1000) {
					maze [currTile.X, currTile.Y] = 0;
					if (currTile.X + 1 < width) if (maze [currTile.X + 1, currTile.Y] == -1) {
						currTile = new Point (currTile.X + 1, currTile.Y);
						continue;
					}
					if (currTile.X - 1 >= 0) if (maze [currTile.X - 1, currTile.Y] == -1) {
						currTile = new Point (currTile.X - 1, currTile.Y);
						continue;
					}
					if (currTile.Y + 1 < height) if (maze [currTile.X, currTile.Y+1] == -1) {
						currTile = new Point (currTile.X, currTile.Y+1);
						continue;
					}
					if (currTile.Y - 1 >= 0) if (maze [currTile.X, currTile.Y-1] == -1) {
						currTile = new Point (currTile.X, currTile.Y-1);
						continue;
					}
					i++;
				}
			}
		}


		bool vonNeumannNeighborhoodEmpty(int i, int j)
		{
			if (i > 0)
			if (maze [i - 1, j] != -1)
				return false;
			if (j > 0)
			if (maze [i, j-1] != -1)
				return false;

			if (i < width-1)
			if (maze [i + 1, j] != -1)
				return false;
			if (j <height-1)
			if (maze [i, j+1] != -1)
				return false;
			return true;
		}

		bool mooreNeighborhoodEmpty(int i, int j)
		{
			if (!vonNeumannNeighborhoodEmpty (i, j))
				return false;
			if (i > 0 && j > 0)
			if (maze [i - 1, j - 1] != -1)
				return false;
			if (i < width-1 && j > 0)
			if (maze [i + 1, j - 1] != -1)
				return false;
			if (i < 0 && j <height-1)
			if (maze [i - 1, j + 1] != -1)
				return false;
			if (i < width-1 && j <height-1)
			if (maze [i + 1, j + 1] != -1)
				return false;
			return true;
		}

		void clearThinWalls()
		{
			var newMaze = maze;
			for (int i = 1; i < width - 1; i++)
				for (int j = 1; j < height - 1; j++) {
					if (maze [i, j] != 0)
						continue;
					if (maze [i + 1, j] == -1 && maze [i - 1, j] == -1)
						newMaze [i, j] = -1;
					if (maze [i, j+1] == -1 && maze [i, j+1] == -1)
						newMaze [i, j] = -1;
				}
			maze = newMaze;
		}

		void removeFloatingPillars()
		{
			var newMaze = maze;
			for (int i = 1; i < width - 1; i++)
				for (int j = 1; j < height - 1; j++) {
					if (maze [i, j] != 0)
						continue;
					if ((maze [i + 1, j] == -1 && maze [i - 1, j] == -1 && maze [i, j+1] == -1) || (maze[i+1,j]==-1 && maze[i,j-1]==-1 && maze[i,j+1]==-1))
						newMaze [i, j] = -1;
					
				}
			maze = newMaze;
		}



		void addRocks()
		{
			for (int i = 1; i < width - 1; i++)
				for (int j = 1; j < height - 1; j++) {
					if (mooreNeighborhoodEmpty (i, j) && rand.NextDouble() < RockGenerationProbability)
						maze [i, j] = -2;//make tile rock
				}
		}

		void makeBorder()
		{
			for (int i = 0; i < width; i++) {
				maze [i, 0] = 0;
				maze [i, height - 1] = 0;
			}
			for (int j = 0; j < height-1; j++) {
				maze [0, j] = 0;
				maze [width-1, j] = 0;
			}
		}

	}
}

