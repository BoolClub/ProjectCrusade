using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ProjectCrusade
{
	public class WorldChunk
	{
		public Tile[,] Tiles;
		public readonly int Width;
		public readonly int Height;
		public bool Visible { get; set; }

		public WorldChunk (ref Tile[,] tiles, Rectangle sourceRect)
		{
			Width = sourceRect.Width;
			Height = sourceRect.Height;
			Visible = true;

			Tiles = new Tile[sourceRect.Width, sourceRect.Height];

			for (int i = 0; i < sourceRect.Width; i++) {
				for (int j = 0; j < sourceRect.Height; j++) {
					Tiles [i, j] = tiles [i + sourceRect.Left, j + sourceRect.Top];
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager, Point position, Rectangle cameraRectTiles)
		{
			Point pTiles = new Point (position.X / World.TileWidth, position.Y / World.TileWidth);

			for (int i = Math.Max(cameraRectTiles.Left - pTiles.X,0); i < Math.Min(cameraRectTiles.Right - pTiles.X+1,Width); i++) {
			for (int j = Math.Max(cameraRectTiles.Top - pTiles.Y,0); j < Math.Min(cameraRectTiles.Bottom - pTiles.Y+1,Height); j++) {
					if (Tiles [i, j].Type != TileType.Air)
						spriteBatch.Draw (textureManager.GetTexture ("tiles"),
							null,
							new Rectangle (position.X + i * World.TileWidth, position.Y + j * World.TileWidth, World.TileWidth, World.TileWidth),
							getTileSourceRect (Tiles [i, j]),
							null,
							Tiles [i, j].Rotation,
							null,
							Color.White,
							SpriteEffects.None,
							0);
				}
			}
		}

		Rectangle getTileSourceRect(Tile t)
		{
			int id = (int)t.Type;

			int spriteSheetTileWidth = World.SpriteSheetWidth / World.TileWidth;

			int y = id / spriteSheetTileWidth;
			int x = id % spriteSheetTileWidth;

			return new Rectangle (x * World.TileWidth, y * World.TileWidth, World.TileWidth, World.TileWidth);
		}

	}
}

