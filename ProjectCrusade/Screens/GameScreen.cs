using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	public abstract class GameScreen
	{
		public GameScreen ()
		{
		}

		abstract public void Update(GameTime gameTime);
		abstract public void Draw (SpriteBatch spriteBatch, TextureManager textureManager);
	}
}

