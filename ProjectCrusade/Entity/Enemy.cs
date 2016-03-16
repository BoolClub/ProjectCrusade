using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	public class Enemy : Entity
	{
		public float Health { get; set; }



		public Enemy ()
		{
			Width = 16;
			Height = 16;
		}

		public override void Update (GameTime gameTime, World world) 
		{
			const float interactionRadius = 1000;
			if ((world.Player.Position - Position).LengthSquared () < interactionRadius * interactionRadius) {
				Vector2 nvec = Vector2.Normalize (world.Player.Position - Position);
				Position += nvec;
			}
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager)
		{
			spriteBatch.Draw (textureManager.GetTexture ("circle"), null, CollisionBox, null, null, 0, null, null, SpriteEffects.None, 1);
		}
	}
}

