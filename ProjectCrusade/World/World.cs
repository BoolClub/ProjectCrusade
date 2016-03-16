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

		//Only active entities (which necessarily includes the player and nearby enemies) 
		//are drawn.
		List<Entity> entities;
		List<Entity> activeEntities;

		List<Light> lights;

		List<Room> rooms;

		Color ambientLighting = new Color(0.9f, 0.9f, 0.9f);

		WorldConfiguration configuration;

		/// <summary>
		/// How often to update lighting in ms. Updating lighting is expensive. 
		/// </summary>
		const float lightingUpdatePeriod = 32.0f;
		float lastLightingUpdate = 0.0f;
		/// <summary>
		/// Through how many tiles a light ray can penetrate. Light is diminished each time.
		/// </summary>
		const int lightingPenetration = 5;


		Tile[,] worldTiles;

		#region Fluid variables
		Fluid fluid;
		Thread fluidThread;
		int fluidUpdateTimeout = 5;

		bool drawSmoke = false;
		bool updateSmoke = false;

		#endregion 

		Random rand = new Random ();

		/// <summary>
		/// A rectangle in tile space describing the view of the camera. Used for culling/optimization.
		/// </summary>
		Rectangle cameraRectangle;


		public World (TextureManager textureManager, int width, int height, ObjectiveManager objManager)
		{
			configuration = new WorldConfiguration ();
			configuration.TileFamily = new TileFamilies.Cave();
			configuration.AddRooms ("Level1/Room1.tmx",1);
			configuration.AddRooms ("Level1/Room2.tmx",2);
			configuration.AddRooms ("Level1/Room3.tmx",1);
			configuration.AddRooms ("Level1/Room4.tmx",1);
			configuration.AddRooms ("Level1/Room5.tmx",1);
			configuration.AddRooms ("Level1/Room6.tmx",1);
			configuration.AddRooms ("Level1/Room7.tmx",1);
			configuration.AddRooms ("Level1/Room8.tmx",1);


			Player = new Player ("test", PlayerType.Wizard, this);
			Width = width;
			Height = height;

			int tilesSize = 0;

			worldTiles = new Tile[Width, Height];

			foreach (Tile t in worldTiles)
				tilesSize += System.Runtime.InteropServices.Marshal.SizeOf(t);
			Console.WriteLine ("World size: {0}KB", tilesSize/1024);


			//Init entities.
			entities = new List<Entity> ();
			entities.Add (Player);

			//Init lights.
			lights = new List<Light> ();
			lights.Add (new Light (new Vector2 (10, 10), Color.White, 3.0f));
			lights [0].Position = Player.Position;
//			lights.Add (new Light (new Vector2 (32, 256), Color.Green, 10.0f));

			generateWorld (objManager);
			Player.Position = rooms [0].Center;//prevent player from getting stuck in tile

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

		/// <summary>
		/// With a certain probability, spawns an enemy at that point
		/// </summary>
		void trySpawnEnemy(Point p)
		{
			const double spawnProbability = 0.01;
			if (rand.NextDouble () < spawnProbability) {
				Enemy e = new Enemy ();
				e.Position = tileToWorldCoord (p);
				entities.Add (e);
			}
		}


		void generateWorld(ObjectiveManager objManager)
		{
			for (int i = 0; i < Width; i++)
				for (int j = 0; j < Height; j++) {
					worldTiles [i, j] = new Tile (TileType.CaveWall, true, Color.White.ToVector3 ());
				}
			//Init rooms
			rooms = new List<Room>();


			foreach (string name in configuration.RoomNames)
			{
				//Lock room positions

				Room room = null;
				bool foundRoom = false;

				while (!foundRoom)
				{
					Point p = new Point (rand.Next (2, Width-2) / 2 * 2, rand.Next (2, Height-2) / 2 * 2);
					bool intersectedOtherRoom = false;
					room = new Room (p, "Content/Levels/" + name);
					foreach (Room r2 in rooms)
						if (r2.Rect.Intersects (room.ExpandedRect)) {
							intersectedOtherRoom = true;
							break;
						}
					
					if (!(room.Rect.Right >= Width || room.Rect.Bottom >= Height || intersectedOtherRoom))
						foundRoom = true;
				}
				room.GenerateRoom (ref worldTiles, ref lights);
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
					if (!generator.IsRoom (i, j)) {
						worldTiles [i, j] = generator.GetMazeTile (configuration.TileFamily, i, j);
						if (generator.IsHall (i, j)) {
							trySpawnEnemy (new Point (i, j));
						}
					}
				}
		}

		public void Update(GameTime gameTime, Camera camera)
		{
			updateEntities (gameTime);

			for (int i = entities.Count - 1; i >= 0; i--) {
				if (entities [i].Delete)
					entities.RemoveAt (i);
			}


			lights [0].Position += (Player.Position - lights [0].Position) * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
				if (updateSmoke) fluid.Update ();
				Thread.Sleep (fluidUpdateTimeout);
			}
		}

		//where distance2 is the squared distance (in tile lengths)
		float lightFalloffFunction(float distance2) {
			return 1.0f / (distance2*0.1f + 1.0f);
		}

		/// <summary>
		/// Outside of this rectangle, no light rays are rendered, regardless of whether they passed through the camera 
		/// viewport.
		/// </summary>
		Rectangle getLightCullingRegion()
		{
			const int buffer = 20;
			return new Rectangle (
				cameraRectangle.X - buffer, 
				cameraRectangle.Y - buffer, 
				cameraRectangle.Width + 2 * buffer, 
				cameraRectangle.Height + 2 * buffer);
		}

		/// <summary>
		/// Outside of this region, no the enemies or other entities are updated (for efficiency). 
		/// </summary>
		Rectangle getActiveEntityRegion()
		{
			const int buffer = 50;
			return new Rectangle (
				cameraRectangle.X - buffer, 
				cameraRectangle.Y - buffer, 
				cameraRectangle.Width + 2 * buffer, 
				cameraRectangle.Height + 2 * buffer);
		}

		//From http://stackoverflow.com/questions/18525214/efficient-2d-tile-based-lighting-system
		//Implementation of Bresenham's algorithm
		/// <summary>
		/// Used to perform ray tracing for lighting.
		/// </summary>
		public List<Tuple<Point,int>> GetLine(Point start, Point target) {
			

			List<Tuple<Point,int>> ret = new List<Tuple<Point,int>>();
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
			int currInc = 0;
			bool enteredCameraRectangle = false;
			for(;;){  /* loop */
				if (cameraRectangle.Contains (x0, y0))
					enteredCameraRectangle = true;
				ret.Add( new Tuple<Point,int>(new Point(x0,y0), currInc) );
				if (x0==x1 && y0==y1) break;

				if ((!cameraRectangle.Contains(x0,y0) && enteredCameraRectangle) || !getLightCullingRegion().Contains(x0,y0))
					break; //break if ray exits camera field of vision or is simply too far away
				if (worldTiles [x0, y0].Solid)
					currInc++;// break if rays hit wall--no use of iterating if light won't pass a wall
				if (currInc >= lightingPenetration)
					break;
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

				for (int i = 0; i<boundary.Count;i++)
				{
					Point p = boundary [i];
					var line = GetLine (new Point (worldToTileCoordX ((int)light.Position.X), worldToTileCoordY ((int)light.Position.Y)), p);

					for (int k = 0; k < line.Count; k++) {
						if (!(line [k].Item1.X >= 0 && line [k].Item1.X < Width && line [k].Item1.Y >= 0 && line [k].Item1.Y < Height))
							break;
						float dist2 = (light.Position - tileToWorldCoord (line [k].Item1.X, line [k].Item1.Y)).LengthSquared () / (TileWidth*TileWidth) ;
						colorsTemp[line[k].Item1.X, line[k].Item1.Y] =
							light.Strength
							* light.Color.ToVector3 ()
							* lightFalloffFunction (dist2)
							* (float)(lightingPenetration-line[k].Item2)/lightingPenetration;
					}
				}
				for (int i = 0; i < Width; i++)
					for (int j = 0; j < Height; j++) {
						worldTiles[i,j].Color+=colorsTemp [i, j];
					}
			}
		}

		void updateEntities(GameTime gameTime)
		{
			Rectangle activeEntityRegion = getActiveEntityRegion ();
			//convert to world coordinates.
			activeEntityRegion.X*=TileWidth;
			activeEntityRegion.Y*=TileWidth;
			activeEntityRegion.Width*=TileWidth;
			activeEntityRegion.Height*=TileWidth;

			//construct active entities 
			activeEntities = entities.FindAll(e => activeEntityRegion.Intersects(e.CollisionBox));

			foreach (Entity e in activeEntities)
				updateEntity (gameTime, e);
		}

		void updateEntity(GameTime gameTime, Entity entity)
		{

			Vector2 prevPosition = entity.Position;
			entity.Update (gameTime, this);
			Point p = worldToTileCoord (entity.Position);

			//Disable fluid influence.
//
//			if (entity is Player) {
//				Vector2 vel = 5*(entity.Position - prevPosition);
//				fluid.SetVel (p.X, p.Y, vel);
//				fluid.SetDensity (p.X, p.Y, 0.9f);
//			} else { 
//				entity.Position += fluid.GetVel (p.X, p.Y);
//			}
//
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
				Vector2 tileToWorldCoord(Point p)
		{
					return new Vector2 (TileWidth * p.X, TileWidth * p.Y);
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

			cameraRectangle = cameraRectTiles;

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

			foreach (Entity entity in entities)
				entity.Draw (spriteBatch, textureManager, fontManager);
			spriteBatch.End ();
			spriteBatch.Begin (SpriteSortMode.Texture, BlendState.Additive, null, null, null, null, camera.TransformMatrix);
			//Draw smoke/additive lighting
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
							new Color(0.25f*(worldTiles[i,j].Color - Vector3.One)),
							SpriteEffects.None,
							0);
					}

		}
		//TODO: Add procedural world generation



	}
}

