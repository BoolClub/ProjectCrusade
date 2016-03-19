using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ProjectCrusade
{
	/// <summary>
	/// A Non-Player Character. The NPC will typically have some sort of information to relay to the player. To
	/// handle this, each NPC object will have a list of spoken text that must be set upon creation or initialization.
	/// That is the text that will be displayed when the player interacts with the NPC. 
	/// </summary>
	public class NPC : Entity
	{
		/// <summary>
		/// The world that the NPC is in.
		/// </summary>
		public World world;

		/// <summary>
		/// This is the text box for the NPC. It will display the speech for the NPC to say when interacting
		/// with the player.
		/// </summary>
		public TextBox TextBox;

		/// <summary>
		/// Gets or sets the name of the NPC.
		/// </summary>
		/// <value>The name of the NPC.</value>
		public string Name { get; set; }



		public NPC (string name, World w) {
			Name = name;
			world = w;
			Width = 32; 
			Height = 32;
			Position = new Vector2 (0,0);
			TextBox = new TextBox (Position);
		}

	
		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, Color color) {
			//Draw a temporary box for the NPC

			spriteBatch.Draw (textureManager.WhitePixel, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Util.TintColor(color, Color.Pink));



			//If interacting with the player...
			TextBox.Draw (spriteBatch, textureManager, fontManager);
		}

		public override void Update(GameTime gameTime, World world) {
			
		}




	} //END OF NPC CLASS

} //END OF NAMESPACE

