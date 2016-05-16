using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ProjectCrusade
{
	/// <summary>
	/// A GUI element that displays explored regions. 
	/// </summary>
	public class Map
	{
		/// <summary>
		/// A texture where every pixel represents a single tile. The color value of each pixel corresponds to the average color value of a tile.
		/// </summary>
		Texture2D tileSummaryTexture;

		Point playerPos;
		Texture2D playerTexture;

		Dictionary<UInt64, int> tiles;

		const int Scale = 2;

		public float Opacity { get; set; }

		public Map (TextureManager textureManager)
		{
			tiles = new Dictionary<ulong, int> ();
			playerPos = new Point (0, 0);
			constructTileSummaryTexture (textureManager);
			Opacity = 0.9f;
			playerTexture = textureManager.GetTexture ("circle");
		}


		/// <summary>
		/// Makes a given tile visible on the map
		/// </summary>
		public void AddTile(Point p, Tile t)
		{
			int id = (int)t.Type;
			tiles [pRep (p)] = id;
		}

		/// <summary>
		/// Sets the internal player position based on the actual world position of the player.
		/// </summary>
		public void SetPlayerPosition(Vector2 pos)
		{
			playerPos = (pos * (1.0f / World.TileWidth)).ToPoint ();
		}

		/// <summary>
		/// Converts a point to a more compact representation for use in a dictionary
		/// </summary>
		UInt64 pRep(Point p)
		{
			return ((ulong)p.X) << 32 | ((ulong)p.Y);
		}
		Point invPRep(ulong key)
		{
			return new Point ((int)(key >> 32), (int)(key));
		}

		Rectangle getSourceRect(int id)
		{
			int x = id % tileSummaryTexture.Width;
			int y = id / tileSummaryTexture.Height;

			return new Rectangle (x, y, 1, 1);
		}

		void constructTileSummaryTexture(TextureManager textureManager)
		{
			Texture2D tiles = textureManager.GetTexture ("tiles");

			int textWidthTiles = tiles.Width / World.TileWidth;
			tileSummaryTexture = new Texture2D (MainGame.graphics.GraphicsDevice, textWidthTiles, textWidthTiles);

			for (int i = 0; i < textWidthTiles; i++) 
			{
				for (int j = 0; j < textWidthTiles; j++) 
				{
					Color[] data = new Color[World.TileWidth * World.TileWidth];

					tiles.GetData<Color> (0, new Rectangle (i * World.TileWidth, j * World.TileWidth, World.TileWidth, World.TileWidth), data, 0, World.TileWidth * World.TileWidth);

					//average data using arithmetic mean
					Vector3 avg = Vector3.Zero;
					int numSamples = 0;
					for (int k = 0; k < data.Length; k++)
					{
						//only add pixel value if opaque
						if (data [k].A >= byte.MaxValue) {
							avg += data [k].ToVector3 ();
							numSamples++;
						}
					}
					avg /= numSamples;
					avg -= Vector3.One * 0.5f;

					//effective contrast level
					avg *= 1.25f;

					avg += Vector3.One * 0.5f;

					tileSummaryTexture.SetData<Color>(0, new Rectangle (i, j, 1, 1), new Color[] {new Color(avg) }, 0, 1);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var pair in tiles) {
				Point pos = invPRep (pair.Key);
				spriteBatch.Draw (tileSummaryTexture, null, new Rectangle(Scale * pos.X, Scale * pos.Y, Scale,Scale), getSourceRect (pair.Value), null, 0, null, Color.White * Opacity, SpriteEffects.None, 0.0f);
			}
			//TODO: make this more general; currently, uses the grass texture (ID 1) for the player.
			spriteBatch.Draw (playerTexture, null, new Rectangle(Scale * playerPos.X, Scale * playerPos.Y, playerTexture.Width/2,playerTexture.Height/2), null, null, 0, null, Color.White * Opacity, SpriteEffects.None, 0.0f);
		}
	}
}

