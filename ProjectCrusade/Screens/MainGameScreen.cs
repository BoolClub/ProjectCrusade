using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	/// <summary>
	/// Main game screen. This is where all game logic takes place. 
	/// </summary>
	public class MainGameScreen : GameScreen
	{
		Camera camera;
		World world;
		//New player object
		Player player;

		public MainGameScreen ()
		{
			camera = new Camera ();
			world = new World (16, 16);

			player = new Player ("Player_1",PlayerType.Knight);
		}
		public override void Update (GameTime gameTime)
		{
			camera.Update ();

			//Update the player
			player.Update (gameTime);
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager)
		{

			spriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null, camera.TransformMatrix);

			world.Draw (spriteBatch, textureManager);

			//Draw the player (may or may not be needed).
			player.Draw (spriteBatch, textureManager);

			spriteBatch.Draw (textureManager.GetTexture ("circle"), new Rectangle(100,100,100,100), Color.White);

			spriteBatch.End ();

		}
	}
}

