using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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


		/// <summary>
		/// The interaction box (rectangle). When something is within this interaction box and the player clicks
		/// a certain button, perform a particular action.
		/// </summary>
		public Rectangle InteractionBox { get; set; }

		/// <summary>
		/// Amount to add to the interaction box on all four sides.
		/// </summary>
		public int Padding = 10;


		/// <summary>
		/// The direction that the player is facing as an integer.
		/// </summary>
		/// <value> 0 -- DOWN </value>
		/// <value> 1 -- RIGHT </value>
		/// <value> 2 -- UP </value>
		/// <value> 3 -- LEFT </value>
		public int Facing { get; set; }


		public Inventory Inventory { get; }


		public Player (string name, PlayerType type) {
			PlayerName = name;
			PlayerType = type;
			Width = 16;
			Height = 16;
			Speed = 300;

			Sanity = 20;
			MaxSanity = 100;

			Facing = 0;
			InteractionBox = new Rectangle ((int)Position.X, (int)Position.Y, Width, Height+Padding);


			Position = new Vector2(0,0);

			Inventory = new Inventory (4, 10);
			Apple a = new Apple ();
			a.Count = Item.MaxStackSize-1;
			Inventory.AddItem (a);
			Inventory.AddItem (new Coin(5));
			Inventory.AddItem (new Coin());
			Inventory.AddItem (new Coin());
			Inventory.AddItem (new WoodenSword ());
			Inventory.AddItem (new StarterArrow ());
			Inventory.AddItem (new MagicWand ());

		}

		/// <summary>
		/// How many pixels/sec the player moves.
		/// </summary>
		public float Speed { get; private set; } 


		public override void Update(GameTime time, World world) {
			//Do all the updating for the player here.

			Inventory.Update (time, world);

			//Set the interaction box based on the player's direction.
			if (Facing == 0) {
				InteractionBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height+Padding);
			} else if(Facing == 1) {
				InteractionBox = new Rectangle((int)Position.X, (int)Position.Y, Width+Padding, Height);
			} else if(Facing == 2) {
				InteractionBox = new Rectangle((int)Position.X, (int)Position.Y-Padding, Width, Height+Padding);
			} else if(Facing == 3) {
				InteractionBox = new Rectangle((int)Position.X-Padding, (int)Position.Y, Width+Padding, Height);
			}

			checkInput (time, world);
		}
		public override void Draw(SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, Color color) {
			//Do all the drawing for the player here.

			Texture2D t = textureManager.GetTexture ("circle");
			Texture2D ib = textureManager.WhitePixel;

			spriteBatch.Draw (t, null, CollisionBox, null, null, 0, null, color, SpriteEffects.None, 0.1f);

			spriteBatch.Draw (ib, null, InteractionBox,null,null,0,null,Color.Black,SpriteEffects.None,0);
		}



		//SETTERS
		public void Damage(float amount) { 

			if (Sanity - amount >= 0) {
				Sanity -= amount;
			} else
				Sanity = 0;

		}
		public void Heal(float amount) { 

			if (Sanity + amount < MaxSanity)
				Sanity += amount;
			else
				Sanity = MaxSanity;
		}



		public bool Moving { get; private set; }

		public static MouseState PrevMouseState { get; set; }
		public static KeyboardState PrevKeyState { get; set; }


		//PLAYER INPUT
		void checkInput(GameTime time, World world) {
			KeyboardState keyState = Keyboard.GetState ();

			float calcDisp = (float)time.ElapsedGameTime.TotalSeconds * Speed;

			Vector2 disp = Vector2.Zero;


			//Move player.
			if (keyState.IsKeyDown (Keys.D) || keyState.IsKeyDown (Keys.Right)) {
				disp += new Vector2 (calcDisp, 0);
				Moving = true;
				Facing = 1;
			}
			if (keyState.IsKeyDown (Keys.A) || keyState.IsKeyDown (Keys.Left)) {
				disp += new Vector2 (-calcDisp, 0);
				Moving = true;
				Facing = 3;
			}
			if (keyState.IsKeyDown (Keys.S) || keyState.IsKeyDown (Keys.Down)) {
				disp += new Vector2 (0, calcDisp);
				Moving = true;
				Facing = 0;
			}
			if (keyState.IsKeyDown (Keys.W) || keyState.IsKeyDown (Keys.Up)) {
				disp += new Vector2 (0, -calcDisp);
				Moving = true;
				Facing = 2;
			}


			//Primary Use Items
			if (keyState.IsKeyDown (Keys.Q) && PrevKeyState.IsKeyUp(Keys.Q)) {
				if (Inventory.ActiveSlot != null) {

					if (Inventory.ActiveSlot.HasItem) {

						Inventory.ActiveSlot.Item.PrimaryUse (world);

						//If the item is depletable, it is removed when used.
						if (Inventory.ActiveSlot.Item.Depletable) {
							Inventory.ActiveSlot.RemoveItem ();
						}
					}

				}
			}

			/*This can be the "interact" button for now. It just checks if the entity is next to the player 
			and if the entity is not the player, then it will interact. */
			if (keyState.IsKeyDown (Keys.C) && PrevKeyState.IsKeyUp (Keys.C)) {

				foreach(Entity e in world.activeEntities) {
					if (!(e is Player) && e.IsNextToPlayer (world))
						e.InteractWithPlayer (this);
				}
			}


			//Quickly add an item -- (just for testing purposes)
			if (keyState.IsKeyDown (Keys.N) && PrevKeyState.IsKeyUp (Keys.N)) {
				List<Item> t = new List<Item> ();
				t.Add (new Apple());
				t.Add (new Water ());
				t.Add (new Bread ());
				t.Add (new Coin ());
				t.Add (new WoodenSword ());
				t.Add (new IronSword ());
				t.Add (new StoneSword ());
				t.Add (new StarterArrow ());
				t.Add (new MagicWand ());

				Inventory.AddItem (t [new Random ().Next (t.AsReadOnly().Count)]);
			}


			//Normalize displacement so that you travel the same speed diagonally. 
			if ((keyState.IsKeyDown (Keys.D) && keyState.IsKeyDown (Keys.W)) || (keyState.IsKeyDown (Keys.D) && keyState.IsKeyDown (Keys.S)) || (keyState.IsKeyDown (Keys.A) && keyState.IsKeyDown (Keys.W)) || (keyState.IsKeyDown (Keys.A) && keyState.IsKeyDown (Keys.S))) {
				disp /= (float)Math.Sqrt (2.0);
			}
			if ((keyState.IsKeyDown (Keys.Right) && keyState.IsKeyDown (Keys.Up)) || (keyState.IsKeyDown (Keys.Right) && keyState.IsKeyDown (Keys.Down)) || (keyState.IsKeyDown (Keys.Left) && keyState.IsKeyDown (Keys.Up)) || (keyState.IsKeyDown (Keys.Left) && keyState.IsKeyDown (Keys.Down))) {
				disp /= (float)Math.Sqrt (2.0);
			}

			Position+=disp;

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
