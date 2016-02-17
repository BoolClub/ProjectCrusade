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

		public Inventory Inventory;


		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectCrusade.Player"/> class.
		/// </summary>
		public Player (String name, PlayerType type) {
			PlayerName = name;
			PlayerType = type;
			Initialize ();
			Width = 32;
			Height = 32;
			Speed = 200;
			Inventory = new Inventory (4, 8);
			Inventory.AddItem (new Apple ());
			Inventory.AddItem (new Apple());
		}

		/// <summary>
		/// How many pixels/sec the player moves.
		/// </summary>
		public float Speed { get; private set; } 


		public override void Initialize() {
			//Do all the initializing for the player here.
			Position = new Vector2(0,0);
			PlayerInput.player = this;
		}
		public override void Update(GameTime time) {
			//Do all the updating for the player here.

			Inventory.Update (time);

			//Checking for player input.
			PlayerInput.CheckInput(time);

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
