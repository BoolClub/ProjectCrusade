using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ProjectCrusade
{
	public struct WorldLayer {

		public Tile[,] Tiles;

		public WorldLayer(int width, int height, TileType solidType = TileType.Air) {

			Tiles = new Tile[width, height];

			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
					Tiles [i, j] = new Tile (solidType, true, Color.White.ToVector3());
		}
	}

	public class World
	{
		public int Width { get; private set; }

		public int Height { get; private set; }

		/// <summary>
		/// Width of sprite sheet in pixels
		/// </summary>
		const int SpriteSheetWidth = 1024;
		/// <summary>
		/// Width of a tile in pixels. Also the height (for square tiles)
		/// </summary>
		const int TileWidth = 32;


		List<WorldLayer> layers;

		public Player Player;

		List<Entity> entities;

		List<Light> lights;

		Color ambientLighting = new Color(0.35f,0.35f,0.35f);

		List<Room> rooms;

		/// <summary>
		/// How often to update lighting in ms. Updating lighting is expensive. 
		/// </summary>
		const float lightingUpdatePeriod = 32.0f;
		float lastLightingUpdate = 0.0f;



		public World (TextureManager textureManager, int width, int height)
		{
			Player = new Player ("test", PlayerType.Wizard, this);
			Player.Position = new Vector2 (100, 100);
			Width = width;
			Height = height;

			//Init layers & tiles.
			layers = new List<WorldLayer> ();
			layers.Add (new WorldLayer (width, height, TileType.Floor)); // floor layer
			layers.Add (new WorldLayer (width, height, TileType.Wall)); // wall layer
			for (int i = 1; i<width - 1; i++)
				for (int j = 1; j<width - 1; j++) 
				{ 
					layers[1].Tiles[i,j].Type = TileType.Air;
					layers[1].Tiles[i,j].Solid=false; 
				}

			//Init entities.
			entities = new List<Entity> ();
			entities.Add (Player);

			//Init lights.
			lights = new List<Light> ();
			lights.Add (new Light (new Vector2 (10, 10), Color.Orange, 10.0f));
			lights.Add (new Light (new Vector2 (32, 256), Color.Green, 10.0f));

			//Init rooms.
			rooms = new List<Room> ();
			generateRoom (new Point (2,2), textureManager.GetTexture ("testRoom")); 
		}

		public void Update(GameTime gameTime)
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
				if (layers [1].Tiles [x0, y0].Solid)
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
					layers [0].Tiles [i, j].Color = ambientLighting.ToVector3();

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
						layers [0].Tiles [i, j].Color += colorsTemp [i, j];
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

			if (layers [1].Tiles [worldToTileCoordX (entity.CollisionBox.Left),worldToTileCoordY (entity.CollisionBox.Top)].Solid)
				return true;

			if (layers [1].Tiles [worldToTileCoordX (entity.CollisionBox.Right),worldToTileCoordY (entity.CollisionBox.Top)].Solid)
				return true;

			if (layers [1].Tiles [worldToTileCoordX (entity.CollisionBox.Left),worldToTileCoordY (entity.CollisionBox.Bottom)].Solid)
				return true;

			if (layers [1].Tiles [worldToTileCoordX (entity.CollisionBox.Right),worldToTileCoordY (entity.CollisionBox.Bottom)].Solid)
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

		//get upper-right-hand corner of a tile
		Vector2 tileToWorldCoord(int x, int y)
		{
			return new Vector2 (TileWidth * x, TileWidth * y);
		}

		Rectangle getTileSourceRect(Tile t)
		{
			int id = (int)t.Type;

			int spriteSheetTileWidth = SpriteSheetWidth / TileWidth;

			int y = id / spriteSheetTileWidth;
			int x = id % spriteSheetTileWidth;

			return new Rectangle (x * TileWidth, y * TileWidth, TileWidth, TileWidth);
		}

		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager)
		{


			foreach (WorldLayer layer in layers) {
				for (int i = 0; i < Width; i++) {
					for (int j = 0; j < Height; j++) {
						if (layer.Tiles[i,j].Type!=TileType.Air) 
							spriteBatch.Draw (textureManager.GetTexture ("tiles"),
								null,
								new Rectangle (i * TileWidth, j * TileWidth, TileWidth, TileWidth),
								getTileSourceRect (layer.Tiles [i, j]),
								null,
								layer.Tiles[i,j].Rotation,
								null,
								new Color(layer.Tiles[i,j].Color),
								SpriteEffects.None,
								0);
					}
				}
			}


			foreach (Entity entity in entities)
				entity.Draw (spriteBatch, textureManager);
		}
		//TODO: Add procedural world generation


		void generateRoom(Point position, Texture2D template)
		{
			rooms.Add (new Room(new Rectangle(position.X, position.Y, template.Width, template.Height)));

			Color[] data = new Color[template.Width * template.Height];

			template.GetData<Color> (data);

			for (int i = 0; i < template.Width; i++) {
				for (int j = 0; j < template.Height; j++) {

					//index of layer represented by green component
					int layerInd = (int)data [i + j * template.Width].G;

					//red component becomes tile ID
					layers [layerInd].Tiles [position.X + i, position.Y + j].Type = (TileType)data [i + j * template.Width].R;

					//If on wall layer, make solid.
					layers [layerInd].Tiles [position.X + i, position.Y + j].Solid = layerInd == 1;
				}
			}
		}
	}
}

