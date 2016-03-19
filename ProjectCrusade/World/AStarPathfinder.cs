using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;


namespace ProjectCrusade
{
	public class AStarPathfinder
	{
		/// <summary>
		/// Converts a point to a more compact representation for use in a dictionary
		/// </summary>
		UInt64 pRep(Point p)
		{
			return ((ulong)p.X) << 32 | ((ulong)p.Y);
		}

		struct Node {
			public float gScore;
			public float fScore;
			public Point cameFrom;
		}

		Dictionary<UInt64, Node> closed;
		Dictionary<UInt64, Node> open;
		Point minOpenNode;


		public AStarPathfinder ()
		{
			closed = new Dictionary<UInt64, Node> ();
			open = new Dictionary<UInt64, Node> ();
		}


		public enum HeuristicType { 
			Manhattan,
			Euclidean
		}

		float euclidean(Point p, Point target)
		{
			return (target - p).ToVector2 ().Length();
		}
		float manhattan(Point p, Point target)
		{
			return Math.Abs (p.X - target.X) + Math.Abs (p.Y - target.Y);
		}


		bool pointInWorld(Point p, int width, int height)
		{
			return (p.X >= 0 && p.Y >= 0 && p.X < width && p.Y < height);
		}

		public Point[] Compute(Point start, Point target, ref Tile[,] worldTiles, HeuristicType heuristicType = HeuristicType.Euclidean)
		{
			Stopwatch s = Stopwatch.StartNew ();

			int width = worldTiles.GetLength (0), height = worldTiles.GetLength (1);

			open [pRep(start)] = new Node{ gScore = 0, fScore = euclidean (start, target), cameFrom = start };

			minOpenNode = start;

			while (open.Count > 0) {
				Point current = minOpenNode;
				float lowScore = -1;
				foreach (var n in open) {
					if (n.Value.fScore < lowScore || lowScore == -1) {
						current = new Point((int)(n.Key >> 32), (int)(n.Key));
						if (pRep (current) != n.Key)
							throw new Exception ();
						lowScore = n.Value.fScore;
					}
				}
				Node curData = open [pRep (current)];
				if (current == target) {
					Debug.WriteLine ("Succeeded in finding path, {0} ms", s.ElapsedMilliseconds);
					return getPath (target, start);
				}


				//moved
				closed[pRep(current)] = open[pRep(current)];
				open.Remove (pRep (current));

				Point[] neighbors = new Point[4];
				neighbors [0] = new Point (current.X + 1, current.Y);
				neighbors [1] = new Point (current.X - 1, current.Y);
				neighbors [2] = new Point (current.X, current.Y+1);
				neighbors [3] = new Point (current.X, current.Y-1);

				for (int i = 0; i < neighbors.Length; i++) {
					if (closed.ContainsKey (pRep (neighbors [i])) || !pointInWorld(neighbors[i], width,height))
						continue;
					if (worldTiles [neighbors [i].X, neighbors [i].Y].Solid)
						continue;

					float gs = curData.gScore + 1;
					float heur = heuristicType==HeuristicType.Manhattan ? manhattan(neighbors [i], target) : euclidean (neighbors [i], target);

					if (!open.ContainsKey (pRep (neighbors [i]))) {
						open[pRep(neighbors[i])] = new Node{};
					} else if (gs >= open [pRep (neighbors [i])].gScore)
						continue;
					
					open [pRep (neighbors [i])] = new Node{gScore = gs, fScore = gs + heur, cameFrom = current };
				}

			}
			Debug.WriteLine ("Failure to find path, {0} ms", s.ElapsedMilliseconds);
			//failed to find a path
			return new Point[] { };
		}

		/// <summary>
		/// Traces backward the path
		/// </summary>
		Point[] getPath(Point target, Point start)
		{
			List<Point> pts = new List<Point> ();

			Point cur = target; 
			while (cur != start) {
				pts.Add (cur);
				if (open.ContainsKey (pRep (cur)))
					cur = open [pRep (cur)].cameFrom;
				else
					cur = closed [pRep (cur)].cameFrom;
			}
			pts.Reverse ();
			return pts.ToArray ();
		}
		
	}
}

