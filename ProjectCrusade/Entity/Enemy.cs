using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	public class Enemy : Entity
	{
		public float Health { get; set; }

		/// <summary>
		/// How many pixels/sec the enemy moves.
		/// </summary>
		public float Speed { get; protected set; }

		public Enemy ()
		{
			Width = 16;
			Height = 16;
			Speed = 120;
		}

		public override void Update (GameTime gameTime, World world) 
		{
			const float interactionRadius = 1000;
			if ((world.Player.Position - Position).LengthSquared () < interactionRadius * interactionRadius) {
				float displacement = (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;
				Vector2 nvec = Vector2.Normalize (world.Player.Position - Position);
				Position += displacement * nvec;
			}
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager)
		{
			spriteBatch.Draw (textureManager.GetTexture ("circle"), null, CollisionBox, null, null, 0, null, null, SpriteEffects.None, 1);
		}
	}
}

