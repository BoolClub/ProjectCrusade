using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	/// <summary>
	/// Generic sprite class. 
	/// </summary>
	public abstract class Sprite
	{
		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>The position of the sprite in the world.</value>
		public Vector2 Position { get; set; }


		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>The width of the player on the screen. </value>
		public int Width { get; protected set; }
		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height of the player on the screen.</value>
		public int Height { get; protected set; }


		/// <summary>
		/// Gets the collision box (computed using Position and dimensions).
		/// </summary>
		/// <value>The collision box.</value>
		public Rectangle CollisionBox { get { return new Rectangle((int)Position.X, (int)Position.Y, Width, Height); } }



		public Sprite ()
		{
		}


		abstract public void Initialize ();


		abstract public void Draw (SpriteBatch spriteBatch, TextureManager textureManager);
	}
}

