using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	/// <summary>
	/// Main game screen. This is where all game logic takes place (most in the World.Update() method).
	/// </summary>
	public class MainGameScreen : GameScreen
	{
		Camera camera;
		World world;

		public MainGameScreen ()
		{
			camera = new Camera ();
			world = new World (16, 16);
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
		}
		public override void Update (GameTime gameTime)
		{
			world.Update (gameTime);
			camera.Position = world.GetPlayerPosition () - new Vector2(MainGame.WINDOW_WIDTH / 2, MainGame.WINDOW_HEIGHT / 2);
			camera.Update ();
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager)
		{

			spriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, camera.TransformMatrix);

			world.Draw (spriteBatch, textureManager);

<<<<<<< Updated upstream
=======
			spriteBatch.Draw (textureManager.GetTexture ("circle"), new Rectangle(100,100,100,100), Color.White);

>>>>>>> Stashed changes
			spriteBatch.End ();

		}
	}
}

