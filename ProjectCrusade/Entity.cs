using System;
using Microsoft.Xna.Framework;

namespace ProjectCrusade
{
	public abstract class Entity : Sprite
	{
		

		public Entity ()
		{
		}

		public abstract void Update(GameTime gameTime);
	}
}

