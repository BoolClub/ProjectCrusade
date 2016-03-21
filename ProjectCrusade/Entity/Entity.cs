using System;
using Microsoft.Xna.Framework;

namespace ProjectCrusade
{
	public abstract class Entity : Sprite
	{
		public bool Delete { get; set; }

		public Entity ()
		{
			Delete = false;
		}

		public abstract void Update(GameTime gameTime, World world);


		/// <summary>
		/// Determines whether this instance is next to player.
		/// </summary>
		/// <returns><c>true</c> if this instance is next to player; otherwise, <c>false</c>.</returns>
		public bool IsNextToPlayer(World world) {
			if (this.CollisionBox.Intersects (world.Player.InteractionBox)) {
				return true;
			} else {
				return false;
			}
		}

		public virtual void InteractWithPlayer (Player player) {}
	}
}

