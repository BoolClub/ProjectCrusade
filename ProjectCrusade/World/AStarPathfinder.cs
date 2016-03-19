using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace ProjectCrusade
{
	public class AStarPathfinder
	{
		/// <summary>
		/// Converts a 
		/// </summary>
		UInt64 PointRepresentation(Point p)
		{
			return (ulong)p.X | ((ulong)p.Y >> 32);
		}
		/// <summary>
		/// Tile scores
		/// </summary>
		Dictionary<UInt64, float> tileValues;
		/// <summary>
		/// Closed
		/// </summary>
		Dictionary<UInt64, float> tileValues;
		public AStarPathfinder ()
		{
			tileValues = new Dictionary<ulong, float> ();
		}

		float heuristic(Point p, Point target)
		{
			return (target - p).ToVector2 ().Length();
		}

		bool tileSet(Point p) { return tileValues.ContainsKey(PointRepresentation(p)); }
		float getTileValue(Point p) { return tileValues[PointRepresentation(p)]; }
		void setTileValue(Point p, float val) { tileValues [PointRepresentation (p)] = val; }

		bool pointInWorld(Point p, int width, int height)
		{
			return (p.X >= 0 && p.Y >= 0 && p.X < width && p.Y < height);
		}

		public Point[] Compute(Point start, Point target, ref Tile[,] worldTiles)
		{
			int worldWidth = worldTiles.GetLength (0), worldHeight = worldTiles.GetLength (1);
			Stack<Point> currPath = new Stack<Point> ();
			setTileValue (start, 0);
			currPath.Push (start);

			while (currPath.Peek () != target) {
				Point p = currPath.Peek ();
				float currScore = getTileValue (p);
				Tuple<Point, float>[] neighbors = new Tuple<Point, float>[4];
				neighbors [0] = new Tuple<Point, float> (new Point (p.X + 1, p.Y), currScore + 1 + heuristic (new Point (p.X + 1, p.Y), target));
				neighbors [1] = new Tuple<Point, float> (new Point (p.X - 1, p.Y), currScore + 1 + heuristic (new Point (p.X - 1, p.Y), target));
				neighbors [2] = new Tuple<Point, float> (new Point (p.X, p.Y + 1), currScore + 1 + heuristic (new Point (p.X, p.Y + 1), target));
				neighbors [3] = new Tuple<Point, float> (new Point (p.X, p.Y - 1), currScore + 1 + heuristic (new Point (p.X, p.Y - 1), target));

				float minScore = -1;
				int minInd = -1;
				for (int i=0;i<neighbors.Length;i++)
				{
					if (!pointInWorld (neighbors [i].Item1, worldWidth, worldHeight))
						continue;
					if ((neighbors [i].Item2 < minScore || minInd == -1) && !worldTiles[neighbors[i].Item1.X,neighbors[i].Item1.Y].Solid && !tileSet(neighbors[i].Item1)) {
						minInd = i;
						minScore = neighbors [i].Item2;
					}
				}
				if (minInd == -1) {
					setTileValue (p, currScore);
					currPath.Pop ();
				}
				else {
					currPath.Push (neighbors [minInd].Item1);
				}
				
			}
			return currPath.ToArray ();
		}
		
	}
}

