using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	public class World
	{
		public int Width { get; private set; }

		public int Height { get; private set; }

		/// <summary>
		/// Width of sprite sheet in pixels
		/// </summary>
		const int SPRITE_SHEET_WIDTH = 1024;
		/// <summary>
		/// Width of a tile in pixels. Also the height (for square tiles)
		/// </summary>
		const int TILE_WIDTH = 32;

		Tile[,] tiles;

		Player player;

		public World (int width, int height)
		{
			player = new Player ("test", PlayerType.Wizard);
			Width = width;
			Height = height;

			tiles = new Tile[Width, Height];

			for (int i = 0; i < Width; i++)
				for (int j = 0; j < Height; j++)
					tiles [i, j] = new Tile (TileType.Floor);
		}

		public void Update(GameTime gameTime)
		{
			player.Update (gameTime);
		}

		Rectangle getTileSourceRect(Tile t)
		{
			int id = (int)t.Type;

			int spriteSheetTileWidth = SPRITE_SHEET_WIDTH / TILE_WIDTH;

			int y = id / spriteSheetTileWidth;
			int x = id % spriteSheetTileWidth;

			return new Rectangle (x * TILE_WIDTH, y * TILE_WIDTH, TILE_WIDTH, TILE_WIDTH);
		}

		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager)
		{
			for (int i = 0; i < Width; i++) {
				for (int j = 0; j < Height; j++) {
					spriteBatch.Draw (textureManager.GetTexture ("tiles"), null, new Rectangle(i*TILE_WIDTH, j*TILE_WIDTH, TILE_WIDTH, TILE_WIDTH),getTileSourceRect(tiles[i,j]), null, 0, null, Color.White, SpriteEffects.None, 0);
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

