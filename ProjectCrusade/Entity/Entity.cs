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
	}
}

