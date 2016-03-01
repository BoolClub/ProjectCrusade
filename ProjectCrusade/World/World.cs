﻿using System;
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

		Color ambientLighting = new Color(0.35f,0.35f,0.35f);


		/// <summary>
		/// How often to update lighting in ms. Updating lighting is expensive. 
		/// </summary>
		const float lightingUpdatePeriod = 32.0f;
		float lastLightingUpdate = 0.0f;

		Tile[,] worldTiles;

		Fluid fluid;
		Thread fluidThread;
		int fluidUpdateTimeout = 5;

		public World (TextureManager textureManager, int width, int height)
		{
			Player = new Player ("test", PlayerType.Wizard, this);
			Player.Position = new Vector2 (100, 100);
			Width = width;
			Height = height;

			constructWorldTiles ("Content/Levels/world_Floor.csv", "Content/Levels/world_Wall.csv");

			int tilesSize = 0;

			foreach (Tile t in worldTiles)
				tilesSize += System.Runtime.InteropServices.Marshal.SizeOf(t);
			Console.WriteLine ("World size: {0}KB", tilesSize/1024);

			//Init entities.
			entities = new List<Entity> ();
			entities.Add (Player);

			//Init lights.
			lights = new List<Light> ();
			lights.Add (new Light (new Vector2 (10, 10), Color.Orange, 10.0f));
			lights.Add (new Light (new Vector2 (32, 256), Color.Green, 10.0f));


			fluid = new Fluid (width, 0.01f);
			for (int i = 0; i < Width; i++)
				for (int j = 0; j < Height; j++)
					if (worldTiles[i,j].Solid) fluid.SetBoundaryValue (i, j, true);

			fluidThread = new Thread (new ThreadStart (fluidUpdate));
			fluidThread.Start ();

			for (int i = 0; i < Width; i+=2)
				for (int j = 0; j < Height; j+=2) {
					entities.Add (new Particle (tileToWorldCoord (i,j)));
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

		/// <summary>
		/// Constructs the world texture from a CSV file exported from Tiled. 
		/// </summary>
		/// <param name="floorFile">Floor file path</param>
		/// <param name="wallFile">Wall file path</param>
		void constructWorldTiles(string floorFile, string wallFile)
		{
			worldTiles = new Tile[Width,Height];
			StreamReader sFloor = new StreamReader(TitleContainer.OpenStream (floorFile));
			for (int j = 0; j < Height; j++) {
				string line = sFloor.ReadLine ();
				var vals = line.Split (',');

				if (vals.Length != Width)
					throw new Exception ("Floor file format does not match width!");

				for (int i = 0; i < Width; i++) {
					int v = Convert.ToInt32 (vals [i]);
					if (v == -1)
						worldTiles [i, j] = new Tile(TileType.Air, false, new Vector3(1,1,1));
					else worldTiles [i, j] = new Tile((TileType)v, false, new Vector3(1,1,1));
				}
			}
			sFloor.Close ();
			StreamReader sWall = new StreamReader(TitleContainer.OpenStream (wallFile));
			for (int j = 0; j < Height; j++) {
				string line = sWall.ReadLine ();
				var vals = line.Split (',');

				if (vals.Length != Width)
					throw new Exception ("Walls file format does not match width!");

				for (int i = 0; i < Width; i++) {
					int v = Convert.ToInt32 (vals [i]);
					//Only include a wall file if not air
					//Overwrites floor tiles
					if (v != -1) 
						worldTiles [i, j] = new Tile((TileType)v, true, new Vector3(1,1,1));
				}
			}
			sFloor.Close ();

		}


		void updateEntity(GameTime gameTime, Entity entity)
		{

			Vector2 prevPosition = entity.Position;
			entity.Update (gameTime, this);
			Point p = worldToTileCoord (entity.Position);
			if (entity is Player) {
				Vector2 vel = 10*(entity.Position - prevPosition);
				fluid.SetVel (p.X, p.Y, vel);
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
		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager, Camera camera)
		{

			//View of camera in tile space
			//Used for per-tile culling
			Rectangle cameraRectTiles = new Rectangle(camera.ViewRectangle.X/TileWidth,camera.ViewRectangle.Y/TileWidth,camera.ViewRectangle.Width/TileWidth,camera.ViewRectangle.Height/TileWidth); 

			for (int i = cameraRectTiles.Left; i < cameraRectTiles.Right+1; i++)
				for (int j = cameraRectTiles.Top; j < cameraRectTiles.Bottom+1; j++) {
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
							new Color(worldTiles[i,j].Color),
//							new Color(new Vector3(fluid.GetVel(i,j), 0)),
							SpriteEffects.None,
							0);
				}
				


			foreach (Entity entity in entities)
				entity.Draw (spriteBatch, textureManager);
		}
		//TODO: Add procedural world generation



	}
}

