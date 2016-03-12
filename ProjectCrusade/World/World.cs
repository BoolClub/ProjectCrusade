using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ProjectCrusade
{
	public class World
	{
		public int Width { get; private set; }

		public int Height { get; private set; }

		/// <summary>
		/// Width of sprite sheet in pixels
		/// </summary>
		public const int SpriteSheetWidth = 512;
		/// <summary>
		/// Width of a tile in pixels. Also the height (for square tiles)
		/// </summary>
		public const int TileWidth = 32;



		public Player Player;

		List<Entity> entities;

		List<Light> lights;

		List<Room> rooms;

		Color ambientLighting = new Color(0.9f, 0.9f, 0.9f);


		/// <summary>
		/// How often to update lighting in ms. Updating lighting is expensive. 
		/// </summary>
		const float lightingUpdatePeriod = 32.0f;
		float lastLightingUpdate = 0.0f;

		Tile[,] worldTiles;

		Fluid fluid;
		Thread fluidThread;
		int fluidUpdateTimeout = 5;

		bool drawSmoke = false;

		Random rand = new Random ();

		TileFamily family;

		public World (TextureManager textureManager, int width, int height, ObjectiveManager objManager)
		{
			Player = new Player ("test", PlayerType.Wizard, this);
			Width = width;
			Height = height;

			int tilesSize = 0;

			worldTiles = new Tile[Width, Height];

			foreach (Tile t in worldTiles)
				tilesSize += System.Runtime.InteropServices.Marshal.SizeOf(t);
			Console.WriteLine ("World size: {0}KB", tilesSize/1024);

			family = new TileFamilies.Cave();

			//Init entities.
			entities = new List<Entity> ();
			entities.Add (Player);

			//Init lights.
			lights = new List<Light> ();
			lights.Add (new Light (new Vector2 (10, 10), Color.Orange, 10.0f));
			lights.Add (new Light (new Vector2 (32, 256), Color.Green, 10.0f));

			generateWorld (objManager);
			Player.Position = rooms [0].Center;

			//Init fluid.
			fluid = new Fluid (width, 0.01f);
			fluid.DecayRate = 0.025f;
			fluid.SmokeDiffusionConstant = 0.000001f;
			fluid.Viscosity = 0.001f;
			for (int i = 0; i < Width; i++)
				for (int j = 0; j < Height; j++)
					if (worldTiles[i,j].Solid) fluid.SetBoundaryValue (i, j, true);

			fluidThread = new Thread (new ThreadStart (fluidUpdate));
			fluidThread.Start ();

		}

		void generateWorld(ObjectiveManager objManager)
		{
			for (int i = 0; i < Width; i++)
				for (int j = 0; j < Height; j++) {
					worldTiles [i, j] = new Tile (TileType.CaveWall, true, Color.White.ToVector3 ());
				}
			//Init rooms
			rooms = new List<Room>();

			const int numRooms = 1;


			for (int i = 0; i < numRooms; i++) {
				//Lock room positions
				Point p = new Point (rand.Next (2, Width-2) / 2 * 2, rand.Next (2, Height-2) / 2 * 2);
				Room room = new Room (p, "Content/Levels/RestRoom.tmx");
				bool intersectedOtherRoom = false;
				foreach (Room r2 in rooms)
					if (r2.Rect.Intersects (room.ExpandedRect)) {
						intersectedOtherRoom = true;
						break;
					}
				if (room.Rect.Right >= Width || room.Rect.Bottom >= Height || intersectedOtherRoom)
					continue;
				room.GenerateRoom (ref worldTiles);
				rooms.Add (room);
			}
			//TODO: procedural generation.

			List<Tuple<Point, int>> entrances = new List<Tuple<Point, int>> ();
			for (int i = 0; i<rooms.Count;i++)
				foreach (Point p in rooms[i].Entrances)
					entrances.Add (new Tuple<Point, int>(new Point(p.X + rooms[i].Rect.Left, p.Y + rooms[i].Rect.Top), i));


			//get objectives
			foreach (Room room in rooms) {
				foreach (var objpair in room.Objectives) {
					Objective obj = new Objective (objpair.Item2, true);

					objManager.AddObjective (objpair.Item1, obj);
				}
			}
			//Attach event listeners to loaded objectives
			objManager.PushListeners ();


			MazeGenerator generator = new MazeGenerator (Width, Height);
			foreach (Room room in rooms) generator.ShadeRoom(room);
			generator.Generate ();
			for (int i = 0; i < Width; i++)
				for (int j = 0; j < Height; j++) {
					if (!generator.IsRoom (i, j))
						worldTiles [i, j] = generator.GetMazeTile (family, i, j);
				}
		}




		public void Update(GameTime gameTime, Camera camera)
		{
			foreach (Entity entity in entities) {
				updateEntity (gameTime, entity);
			}

			for (int i = entities.Count - 1; i >= 0; i--) {
				if (entities [i].Delete)
					entities.RemoveAt (i);
			}


			lights [0].Position = Player.Position;
			//Updating lighting can be expensive, so only do it so often. 
			if (lastLightingUpdate > lightingUpdatePeriod) {
				updateLighting ();
				lastLightingUpdate = 0;
			}
			lastLightingUpdate += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
		}
		void fluidUpdate()
		{

			while (true) {
				fluid.Update ();
				Thread.Sleep (fluidUpdateTimeout);
			}
		}

		//where distance2 is the squared distance (in tile lengths)
		float lightFalloffFunction(float distance2) {
			return 1.0f / (distance2 + 1.0f);
		}

		//From http://stackoverflow.com/questions/18525214/efficient-2d-tile-based-lighting-system
		//Implementation of Bresenham's algorithm
		/// <summary>
		/// Used to perform ray tracing for lighting.
		/// </summary>
		public List<Point> GetLine(Point start, Point target) {
			List<Point> ret = new List<Point>();
			int x0 =  start.X;
			int y0 =  start.Y;

			int x1 = target.X;
			int y1 = target.Y;

			int sx = 0;
			int sy = 0;

			int dx =  Math.Abs(x1-x0);
			sx = x0<x1 ? 1 : -1;
			int dy = -1*Math.Abs(y1-y0);
			sy = y0<y1 ? 1 : -1; 
			int err = dx+dy, e2; /* error value e_xy */

			for(;;){  /* loop */
				ret.Add( new Point(x0,y0) );
				if (x0==x1 && y0==y1) break;
				if (worldTiles[x0,y0].Solid)
					break; // break if rays hit wall--no use of iterating if light won't pass a wall
				e2 = 2*err;
				if (e2 >= dy) { err += dy; x0 += sx; } /* e_xy+e_x > 0 */
				if (e2 <= dx) { err += dx; y0 += sy; } /* e_xy+e_y < 0 */
			}
			return ret;
		}

		void updateLighting()
		{
			for (int i = 0; i < Width; i++)
				for (int j = 0; j < Height; j++)
					worldTiles[i,j].Color = ambientLighting.ToVector3();

			foreach (Light light in lights) {

				List<Point> boundary = new List<Point>(2 * Width + 2 * (Height - 2));
				Vector3[,] colorsTemp = new Vector3[Width, Height];
				for (int i = 0; i < Width; i++) {
					boundary.Add (new Point (i, 0));
					boundary.Add (new Point (i, Height - 1));
				}
				for (int j = 1; j < Height - 1; j++) {
					boundary.Add (new Point (0, j));
					boundary.Add (new Point (Width - 1, j));
				}

				foreach (Point p in boundary)
				{
					var line = GetLine (new Point (worldToTileCoordX ((int)light.Position.X), worldToTileCoordY ((int)light.Position.Y)), p);

					for (int k = 0; k < line.Count; k++) {
						float dist2 = (light.Position - tileToWorldCoord (line [k].X, line [k].Y)).LengthSquared () / (TileWidth*TileWidth) ;
						colorsTemp[line[k].X, line[k].Y] =light.Strength * light.Color.ToVector3 () * lightFalloffFunction (dist2);
					}
				}
				for (int i = 0; i < Width; i++)
					for (int j = 0; j < Height; j++) {
						worldTiles[i,j].Color+=colorsTemp [i, j];
					}
			}


//			foreach (WorldLayer layer in layers) {
//
//
//
//				for (int i = 0; i < Width; i++) {
//					for (int j = 0; j < Height; j++) {
//						Vector3 totalColor = ambientLighting.ToVector3();
//						//TODO: optimize this. Bad complexity. 
//						//TODO: update within a certain distance of each light
//						//		This will become inefficient when the world becomes large.
//						foreach (Light light in lights) {
//							float distance = (tileToWorldCoord (i, j)
//								- light.Position).LengthSquared();
//							totalColor+=light.Strength * light.Color.ToVector3() * lightFalloffFunction (distance);
//
//						}
//
//						layer.Tiles [i, j].Color.R = (byte)(MathHelper.Clamp(totalColor.X * 255, 0, 255));
//						layer.Tiles [i, j].Color.G = (byte)(MathHelper.Clamp(totalColor.Y * 255, 0, 255));
//						layer.Tiles [i, j].Color.B = (byte)(MathHelper.Clamp(totalColor.Z * 255, 0, 255));
//					}
//				}
//
//
//			}
		}

		void updateEntity(GameTime gameTime, Entity entity)
		{

			Vector2 prevPosition = entity.Position;
			entity.Update (gameTime, this);
			Point p = worldToTileCoord (entity.Position);
			if (entity is Player) {
				Vector2 vel = 5*(entity.Position - prevPosition);
				fluid.SetVel (p.X, p.Y, vel);
				fluid.SetDensity (p.X, p.Y, 0.9f);
			} else { 
				entity.Position += fluid.GetVel (p.X, p.Y);
			}

			Vector2 newPosition = entity.Position;
			entity.Position = prevPosition;
			//X collision
			entity.Position = new Vector2(newPosition.X, entity.Position.Y);
			if (entityWallCollision (entity))
				entity.Position = new Vector2(prevPosition.X, entity.Position.Y);
			//Y collision
			entity.Position = new Vector2(entity.Position.X, newPosition.Y);
			if (entityWallCollision (entity))
				entity.Position = new Vector2(entity.Position.X, prevPosition.Y);
		}

		bool entityWallCollision(Entity entity) {

			if (worldToTileCoordX (entity.CollisionBox.Left) < 0)
				return true;
			if (worldToTileCoordX (entity.CollisionBox.Left) >= Width)
				return true;
			if (worldToTileCoordY (entity.CollisionBox.Top) < 0)
				return true;
			if (worldToTileCoordY (entity.CollisionBox.Top) >= Height)
				return true;
			if (worldToTileCoordX (entity.CollisionBox.Right) < 0)
				return true;
			if (worldToTileCoordX (entity.CollisionBox.Right) >= Width)
				return true;
			if (worldToTileCoordY (entity.CollisionBox.Bottom) < 0)
				return true;
			if (worldToTileCoordY (entity.CollisionBox.Bottom) >= Height)
				return true;
			

			if (worldTiles[worldToTileCoordX (entity.CollisionBox.Left),worldToTileCoordY (entity.CollisionBox.Top)].Solid)
				return true;

			if (worldTiles[worldToTileCoordX (entity.CollisionBox.Right),worldToTileCoordY (entity.CollisionBox.Top)].Solid)
				return true;

			if (worldTiles[worldToTileCoordX (entity.CollisionBox.Left),worldToTileCoordY (entity.CollisionBox.Bottom)].Solid)
				return true;

			if (worldTiles[worldToTileCoordX (entity.CollisionBox.Right),worldToTileCoordY (entity.CollisionBox.Bottom)].Solid)
				return true;
			return false;

		}

		int worldToTileCoordX(int x) 
		{
			return x / TileWidth;
		}
		int worldToTileCoordY(int y) 
		{
			return y / TileWidth;
		}

		Point worldToTileCoord(Vector2 pos) {
			return new Point ((int)(pos.X / TileWidth), (int)(pos.Y / TileWidth));
		}

		//get upper-right-hand corner of a tile
		Vector2 tileToWorldCoord(int x, int y)
		{
			return new Vector2 (TileWidth * x, TileWidth * y);
		}


		Rectangle getTileSourceRect(Tile t)
		{
			int id = (int)t.Type;

			int spriteSheetTileWidth = World.SpriteSheetWidth / World.TileWidth;

			int y = id / spriteSheetTileWidth;
			int x = id % spriteSheetTileWidth;

			return new Rectangle (x * World.TileWidth, y * World.TileWidth, World.TileWidth, World.TileWidth);
		}

		/// <summary>
		/// Draw the world and each of its chunks
		/// </summary>
		/// <param name="camera">Camera needed for tile culling</param>
		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, Camera camera)
		{
			//View of camera in tile space
			//Used for per-tile culling
			Rectangle cameraRectTiles = new Rectangle (camera.ViewRectangle.X / TileWidth, camera.ViewRectangle.Y / TileWidth, camera.ViewRectangle.Width / TileWidth, camera.ViewRectangle.Height / TileWidth); 

			for (int i = cameraRectTiles.Left; i < cameraRectTiles.Right + 1; i++)
				for (int j = cameraRectTiles.Top; j < cameraRectTiles.Bottom + 1; j++) {
					if (i < 0 || i >= Width || j < 0 || j >= Height)
						continue;

					if (worldTiles [i, j].Type != TileType.Air)
						spriteBatch.Draw (textureManager.GetTexture ("tiles"),
							null,
							new Rectangle (i * World.TileWidth, j * World.TileWidth, World.TileWidth, World.TileWidth),
							getTileSourceRect (worldTiles [i, j]),
							null,
							worldTiles [i, j].Rotation,
							null,
							new Color (worldTiles [i, j].Color),
//							new Color(new Vector3(fluid.GetVel(i,j), 0)), 
							SpriteEffects.None,
							0);
				}

			//Draw smoke
			if (drawSmoke) {
				for (int i = cameraRectTiles.Left; i < cameraRectTiles.Right + 1; i++)
					for (int j = cameraRectTiles.Top; j < cameraRectTiles.Bottom + 1; j++) {
						if (i < 0 || i >= Width || j < 0 || j >= Height)
							continue;

						spriteBatch.Draw (textureManager.WhitePixel,
							null,
							new Rectangle (i * World.TileWidth, j * World.TileWidth, World.TileWidth, World.TileWidth),
							null,
							null,
							0,
							null,
							Color.White * fluid.GetDensity (i, j),
							SpriteEffects.None,
							0);
					}
			}
					


			foreach (Entity entity in entities)
				entity.Draw (spriteBatch, textureManager, fontManager);
		}
		//TODO: Add procedural world generation



	}
}

