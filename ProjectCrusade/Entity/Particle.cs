using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ProjectCrusade
{
	public class Particle : Entity
	{
		public Particle (Vector2 pos)
		{
			Position = pos;
		}

		public override void Update (GameTime gameTime, World world)
		{
			
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager)
		{
			spriteBatch.Draw (textureManager.WhitePixel, new Rectangle((int)Position.X, (int)Position.Y, 5,5), Color.Black);
		}

	}
}

