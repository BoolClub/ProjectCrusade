using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	public class Projectile : Entity
	{
		//in pix/s
		public Vector2 Velocity { get; set; }
		public float Damage { get; set; }

		//The type of projectile to draw based on where it is on the spritesheet.
		public int projectileType { get; set; }

		public Projectile (Vector2 pos, Vector2 vel, float damage)
		{
			Width = 32;
			Height = 32;
			Position = pos;
			Velocity = vel;
			if (Velocity == Vector2.Zero)
				Delete = true;
			Damage = damage;
		}

		public override void Update (GameTime gameTime, World world)
		{

			foreach (Entity ent in world.activeEntities) {
				if (ent is Enemy) {
					Enemy e = ent as Enemy;

					if (CollisionBox.Intersects(e.CollisionBox)) { 
						e.RemoveHealth (Damage);
						Delete = true;
						break;
					}
				}
			}
			Vector2 disp = Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
			Position += disp;

		}

		//TODO: implement multiple sprites
		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, Color color)
		{
			//render at center
			Vector2 origin = new Vector2(Width/2,Height/2);
			float rotation = (float)(Math.Atan2 (Velocity.Y, Velocity.X));
			spriteBatch.Draw (textureManager.GetTexture("items"), null, CollisionBox, Item.GetTextureSourceRect(projectileType), origin, rotation, null, color, SpriteEffects.None, 1);
		}
	}
}

