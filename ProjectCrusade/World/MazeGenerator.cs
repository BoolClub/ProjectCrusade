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
		/// 1 and larger represents an individual room
		/// </summary>
		int[,] maze;
		int width, height;

		int roomCount = 0;
		Random rand = new Random();



		public MazeGenerator (int width, int height)
		{
			this.width = width;
			this.height = height;
			maze = new int[width, height];
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

		public void Generate()
		{
			Stack<Point> finishedTiles = new Stack<Point> ();
			Point currTile = new Point (0, 0);
			maze [currTile.X, currTile.Y] = -1;
			List<int> options = new List<int> ();
			//TODO: change loop condition
			for (int i = 0; i<1000;i++)
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
	}
}

