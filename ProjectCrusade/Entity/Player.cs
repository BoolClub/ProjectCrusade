using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrusade {
	/// <summary>
	/// This is the player class. It will hold all the information for the player.
	/// </summary>
	public class Player : Entity {

		/// <summary>
		/// This is the player's "health." The variable is called "sanity" since this is what we decided we are doing for our game. 
		/// </summary>
		public float Sanity { get; set; }


		//Different classes might have different max sanities
		public float MaxSanity { get; set; }

		/// <summary>
		/// Gets the name of the player.
		/// </summary>
		public string PlayerName { get; private set; }

		/// <summary>
		/// Gets the type of the player.
		/// </summary>
		public static PlayerType PlayerType { get; private set; }

		public Inventory Inventory { get; }

		public World world;



		public Player (String name, PlayerType type, World w) {
			PlayerName = name;
			PlayerType = type;
			Width = 32;
			Height = 32;
			Speed = 200;

			Sanity = 20;
			MaxSanity = 100;

			world = w;
			Inventory = new Inventory (4, 10);
			for (int i = 0; i < 15; i++)
				Inventory.AddItem (new Apple ());
			Inventory.AddItem (new Coin(5));
			Inventory.AddItem (new Coin());
			Inventory.AddItem (new Coin());
			Inventory.AddItem (new WoodenSword ());
			Inventory.AddItem (new StarterArrow ());
			Inventory.AddItem (new MagicWand ());

			Initialize ();
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
		public override void Update(GameTime time, World world) {
			//Do all the updating for the player here.

			Inventory.Update (time, world);




			//Checking for player input.
			PlayerInput.CheckInput(time);

		}
		public override void Draw(SpriteBatch spriteBatch, TextureManager textureManager) {
			//Do all the drawing for the player here.

			Texture2D t = textureManager.GetTexture ("circle");

			spriteBatch.Draw (t, null, CollisionBox, null, null, 0, null, Color.White, SpriteEffects.None, 0);
		}



		//SETTERS
		public void Damage(int amount) { Sanity -= amount; }
		public void Heal(int amount) { 

			if (Sanity + amount <= MaxSanity)
				Sanity += amount;
			else
				Sanity = MaxSanity;
		}


	} //END OF 'PLAYER' CLASS




	/// <summary>
	/// This enum will be used to determine what type (class) of player the player is. Since we decided on having different player types and different items that you can pick up depending on your type, we can use this to do all of that. 
	/// </summary>
	public enum PlayerType {
		Rogue,
		Knight,
		Wizard,
		Archer,
	} //END OF PLAYERTYPE ENUM

















} //END OF NAMESPACE
