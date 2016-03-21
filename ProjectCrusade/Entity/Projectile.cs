using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	public class Projectile : Entity
	{
		//in pix/s
		public Vector2 Velocity { get; set; }

		public Projectile (Vector2 pos, Vector2 vel)
		{
			Position = pos;
			Velocity = vel;
		}

		public override void Update (GameTime gameTime, World world)
		{
			Vector2 disp = Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
			Position += disp;
		}

		//TODO: implement multiple sprites
		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, Color color)
		{
			spriteBatch.Draw (textureManager.GetTexture ("circle"), Position, null, null, null, 0, null, Color.Red, SpriteEffects.None, 1);
		}
	}
}

