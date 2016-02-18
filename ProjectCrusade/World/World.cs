﻿using System;
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
					Tiles [i, j] = new Tile (solidType);
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

		Player player;

		List<Entity> entities;


		public Inventory PlayerInventory { get { return player.Inventory; } }

		public World (int width, int height)
		{
			player = new Player ("test", PlayerType.Wizard);
			player.Position = new Vector2 (100, 100);
			Width = width;
			Height = height;
			layers = new List<WorldLayer> ();
			layers.Add (new WorldLayer (width, height, TileType.Floor)); // floor layer
			layers.Add (new WorldLayer (width, height, TileType.Wall)); // wall layer
			for (int i = 1; i<width - 1; i++)for (int j = 1; j<width - 1; j++) { layers[1].Tiles[i,j].Type = TileType.Air; layers[1].Tiles[i,j].Solid=false; }
			entities = new List<Entity> ();

			entities.Add (player);
		}

		public void Update(GameTime gameTime)
		{
			foreach (Entity entity in entities) {
				updateEntity (gameTime, entity);
			}

		}

		void updateEntity(GameTime gameTime, Entity entity)
		{

			Vector2 prevPosition = entity.Position;
			entity.Update (gameTime);
			Vector2 newPosition = entity.Position;
			entity.Position = prevPosition;
			//X collision
			entity.Position = new Vector2(newPosition.X, entity.Position.X);
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
								Color.White,
								SpriteEffects.None,
								0);
					}
				}
			}
			player.Draw (spriteBatch, textureManager);
		}
		//TODO: Add procedural world generation


		public Vector2 GetPlayerPosition() {
			return player.Position;
		}
	}
}
