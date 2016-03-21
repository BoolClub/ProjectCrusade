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

		public Projectile (Vector2 pos, Vector2 vel, float damage)
		{
			Width = 8;
			Height = 8;
			Position = pos;
			Velocity = vel;
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
			spriteBatch.Draw (textureManager.GetTexture ("circle"), null, CollisionBox, null, origin, 0, null, Color.Red, SpriteEffects.None, 1);
		}
	}
}

