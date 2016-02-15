using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;

namespace ProjectCrusade {
	/// <summary>
	/// This is the player class. It will hold all the information for the player.
	/// </summary>
	public class Player : Sprite {

		/// <summary>
		/// Gets the sanity.
		/// </summary>
		/// <value>This is the player's "health." The variable is called "sanity" since this is what we decided we are doing for our game. </value>
		public int Sanity { get; private set; }

		/// <summary>
		/// Gets the name of the player.
		/// </summary>
		/// <value>The name of the player.</value>
		public string PlayerName { get; private set; }

		/// <summary>
		/// Gets the type of the player.
		/// </summary>
		/// <value>The type of the player.</value>
		public PlayerType PlayerType { get; private set; }




		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectCrusade.Player"/> class.
		/// </summary>
		/// <param name="name">Player name.</param>
		/// <param name="type">Player type.</param>
		public Player (String name, PlayerType type) {
			PlayerName = name;
			PlayerType = type;
			Initialize ();
			Width = 32;
			Height = 32;
			Speed = 200;
		}

		/// <summary>
		/// Gets the speed.
		/// </summary>
		/// <value>How many pixels/sec the player moves.</value>
		public float Speed { get; private set; } 


		public override void Initialize() {
			//Do all the initializing for the player here.
			Position = new Vector2(0,0);
		}
		public override void Update(GameTime time) {
			//Do all the updating for the player here.

<<<<<<< HEAD
			//Checking for player input.
			PlayerInput.CheckInput();
=======
			KeyboardState keyState = Keyboard.GetState ();

			float calcDisp = (float)time.ElapsedGameTime.TotalSeconds * Speed;

			Vector2 disp = Vector2.Zero;


			//Move player.
			if (keyState.IsKeyDown (Keys.D))
				disp += new Vector2 (calcDisp, 0);
			if (keyState.IsKeyDown (Keys.A))
				disp += new Vector2 (-calcDisp, 0);
			if (keyState.IsKeyDown (Keys.S))
				disp += new Vector2 (0, calcDisp);
			if (keyState.IsKeyDown (Keys.W))
				disp += new Vector2 (0, -calcDisp);


			//Normalize displacement so that you travel the same speed diagonally. 
			if ((keyState.IsKeyDown (Keys.D) && keyState.IsKeyDown (Keys.W)) || (keyState.IsKeyDown (Keys.D) && keyState.IsKeyDown (Keys.S)) || (keyState.IsKeyDown (Keys.A) && keyState.IsKeyDown (Keys.W)) || (keyState.IsKeyDown (Keys.A) && keyState.IsKeyDown (Keys.S))) {
				disp /= (float)Math.Sqrt (2.0);
			}
			Position += disp;
>>>>>>> origin/master

		}
		public override void Draw(SpriteBatch spriteBatch, TextureManager textureManager) {
			//Do all the drawing for the player here.

			Texture2D t = textureManager.GetTexture ("circle");

			spriteBatch.Draw (t, CollisionBox, Color.White);
		}



		//SETTERS
		public void Damage(int amount) { Sanity -= amount; }
		public void Heal(int amount) { Sanity += amount; }


	} //END OF 'PLAYER' CLASS




	/// <summary>
	/// This enum will be used to determine what type (class) of player the player is. Since we decided on having different player types and different items that you can pick up depending on your type, we can use this to do all of that. 
	/// </summary>
	public enum PlayerType {
		Rogue,
		Knight,
		Wizard,
		Arrowman,
	} //END OF PLAYERTYPE ENUM

















} //END OF NAMESPACE
