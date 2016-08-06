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
		public float Health { get; set; }

		//Different classes might have different max sanities
		public float MaxHealth { get; set; }

		/// <summary>
		/// Gets the name of the player.
		/// </summary>
		public string Name { get; private set; }


		/// <summary>
		/// The interaction box (rectangle). When something is within this interaction box and the player clicks
		/// a certain button, perform a particular action.
		/// </summary>
		public Rectangle InteractionBox { get; set; }

		/// <summary>
		/// Amount to add to the interaction box on all four sides.
		/// </summary>
		public int Padding = 10;

		public enum PlayerOrientation
		{
			Down 	= 0,
			Right 	= 1,
			Up 		= 2,
			Left 	= 3
		}

		/// <summary>
		/// The direction that the player is facing
		/// </summary>
		public PlayerOrientation Facing { get; private set; }


		/// <summary>
		/// A normal vector describing which direction the player is moving
		/// </summary>
		public Vector2 OrientationVector { get; private set; }


		public Inventory Inventory { get; }


		public Player (string name) {
			Name = name;
			Width = 24;
			Height = 24;
			Speed = 450;

			Health = 100;
			MaxHealth = 100;

			Facing = 0;
			InteractionBox = new Rectangle ((int)Position.X, (int)Position.Y, Width, Height+Padding);


			Position = new Vector2(0,0);

			Inventory = new Inventory (4, 10);

			//Add a bunch of test items to the inventory
			Apple a = new Apple ();
			a.Count = Item.MaxCount-20;
			Inventory.AddItem (a);
			Inventory.AddItem (new Coin(5));
			Inventory.AddItem (new Coin());
			Inventory.AddItem (new Coin());
			Inventory.AddItem (new Sword());
			StarterArrow arr = new StarterArrow ();
			arr.Count = 63;
			Inventory.AddItem (arr);
			Inventory.AddItem (new MagicWand ());

		}

		/// <summary>
		/// How many pixels/sec the player moves.
		/// </summary>
		public float Speed { get; private set; } 


		public override void Update(GameTime time, World world) {
			//Do all the updating for the player here.

			Inventory.Update (time, world);


			checkInput (time, world);
			//Set the interaction box based on the player's direction.
			switch (Facing)
			{
			case PlayerOrientation.Down:
				InteractionBox = new Rectangle ((int)Position.X, (int)Position.Y, Width, Height + Padding);
				break;
			case PlayerOrientation.Right:
				InteractionBox = new Rectangle((int)Position.X, (int)Position.Y, Width+Padding, Height);
				break;
			case PlayerOrientation.Up:
				InteractionBox = new Rectangle((int)Position.X, (int)Position.Y-Padding, Width, Height+Padding);
				break;
			case PlayerOrientation.Left:
				InteractionBox = new Rectangle((int)Position.X-Padding, (int)Position.Y, Width+Padding, Height);
				break;
			}
		}
		public override void Draw(SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, Color color) {
			//Do all the drawing for the player here.

			Texture2D t = textureManager.GetTexture ("PlayerTexture");
			float angle = (float)Math.Atan2 (OrientationVector.Y, OrientationVector.X);

			var shifted = CollisionBox;
			shifted.Offset (Width / 2, Height / 2);
			spriteBatch.Draw (t, null, shifted, null, new Vector2(t.Width/2,t.Height/2), angle-(float)Math.PI/2, null, color, SpriteEffects.None, 0.1f);

		}



		//SETTERS
		public void Damage(float amount) { 

			if (Health - amount >= 0) {
				Health -= amount;
			} else
				Health = 0;

		}
		public void Heal(float amount) { 

			if (Health + amount < MaxHealth)
				Health += amount;
			else
				Health = MaxHealth;
		}



		public bool Moving { get; private set; }

		public static MouseState PrevMouseState { get; set; }
		public static KeyboardState PrevKeyState { get; set; }


		//PLAYER INPUT
		void checkInput(GameTime time, World world) {
			KeyboardState keyState = Keyboard.GetState ();
			Microsoft.Xna.Framework.Input.Keys primaryUse = Keys.Z;
			Microsoft.Xna.Framework.Input.Keys secondaryUse = Keys.X;
			Microsoft.Xna.Framework.Input.Keys interactionKey = Keys.C;



			#region player movement

			float calcDisp = (float)time.ElapsedGameTime.TotalSeconds * Speed;

			Vector2 disp = Vector2.Zero;

			bool changed = false;
			Vector2 prevOrientation = OrientationVector;
			OrientationVector = Vector2.Zero;

			//Move player.
			if (keyState.IsKeyDown (Keys.D) || keyState.IsKeyDown (Keys.Right)) {
				disp += new Vector2 (calcDisp, 0);
				Moving = true;
				Facing = PlayerOrientation.Right;
				OrientationVector+= new Vector2 (1, 0);
				changed = true;
			}
			if (keyState.IsKeyDown (Keys.A) || keyState.IsKeyDown (Keys.Left)) {
				disp += new Vector2 (-calcDisp, 0);
				Moving = true;
				Facing = PlayerOrientation.Left;
				OrientationVector+= new Vector2 (-1, 0);
				changed = true;
			}
			if (keyState.IsKeyDown (Keys.S) || keyState.IsKeyDown (Keys.Down)) {
				disp += new Vector2 (0, calcDisp);
				Moving = true;
				Facing = PlayerOrientation.Down;
				OrientationVector+= new Vector2 (0, 1);
				changed = true;
			}
			if (keyState.IsKeyDown (Keys.W) || keyState.IsKeyDown (Keys.Up)) {
				disp += new Vector2 (0, -calcDisp);
				Moving = true;
				Facing = PlayerOrientation.Up;
				OrientationVector+= new Vector2 (0, -1);
				changed = true;
			}
			OrientationVector.Normalize ();

			if (!changed)
				OrientationVector = prevOrientation;
			
			//Normalize displacement so that you travel the same speed diagonally. 
			if ((keyState.IsKeyDown (Keys.D) && keyState.IsKeyDown (Keys.W)) || (keyState.IsKeyDown (Keys.D) && keyState.IsKeyDown (Keys.S)) || (keyState.IsKeyDown (Keys.A) && keyState.IsKeyDown (Keys.W)) || (keyState.IsKeyDown (Keys.A) && keyState.IsKeyDown (Keys.S))) {
				disp /= (float)Math.Sqrt (2.0);
			}
			if ((keyState.IsKeyDown (Keys.Right) && keyState.IsKeyDown (Keys.Up)) || (keyState.IsKeyDown (Keys.Right) && keyState.IsKeyDown (Keys.Down)) || (keyState.IsKeyDown (Keys.Left) && keyState.IsKeyDown (Keys.Up)) || (keyState.IsKeyDown (Keys.Left) && keyState.IsKeyDown (Keys.Down))) {
				disp /= (float)Math.Sqrt (2.0);
			}
			Position+=disp;


			#endregion



			#region player interaction

			//Primary Use Items
			if (keyState.IsKeyDown (primaryUse) && PrevKeyState.IsKeyUp(primaryUse)) {
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

			//Secondary Use Items
			if (keyState.IsKeyDown(secondaryUse) && PrevKeyState.IsKeyUp(secondaryUse))
			{
				if (Inventory.ActiveSlot != null)
				{

					if (Inventory.ActiveSlot.HasItem)
					{

						Inventory.ActiveSlot.Item.SecondaryUse(world);

						//If the item is depletable, it is removed when used.
						if (Inventory.ActiveSlot.Item.Depletable)
						{
							Inventory.ActiveSlot.RemoveItem();
						}
					}

				}
			}

			/*This can be the "interact" button for now. It just checks if the entity is next to the player 
			and if the entity is not the player, then it will interact. */
			if (keyState.IsKeyDown (interactionKey) && PrevKeyState.IsKeyUp (interactionKey)) {

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
				t.Add (new Sword ());
				t.Add (new StarterArrow ());
				t.Add (new MagicWand ());

				Inventory.AddItem (t [new Random ().Next (t.AsReadOnly().Count)]);
			}


			//Move to the next world -- Just for testing purposes
			if (keyState.IsKeyDown(Keys.H) && PrevKeyState.IsKeyUp(Keys.H)) {
				world.ReadyToAdvance = true;
			}

			#endregion
		}

	} //END OF 'PLAYER' CLASS
















} //END OF NAMESPACE
