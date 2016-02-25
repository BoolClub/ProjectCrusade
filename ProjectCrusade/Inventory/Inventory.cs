using System;
using System.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ProjectCrusade
{
	public class Inventory {
		
		//The number of rows and columns (inventory slots) to have in the Inventory.
		public int Rows { get; private set; }
		public int Columns { get; private set; }


		//The slots
		InventorySlot[,] slots;

		//The slot that is currently being selected. Used for moving inventory items.
		public InventorySlot SelectedSlot;
		//The slot that is currently active. Used for performing actions on inventory items.
		int activeSlotIndex;
		//returns the slot that corresponds to activeSlotIndex
		public InventorySlot ActiveSlot { get {
				int x = activeSlotIndex % Columns;
				int y = activeSlotIndex / Columns;
				return slots[x,y];
			}
		}

		//Whether or not full inventory is open
		public bool Open { get; set; }

		//The number of items in the inventory (see 'checkInventoryFull' method below).
		private int numItems = 0;

		//Boolean for whether or not the inventory is full.
		public bool IsFull = false;

		/// <summary>
		/// Slot spacing in pixels.
		/// </summary>
		const int SlotSpacing = 8;

		public float Opacity { get; set; }
		public float MainbarOpacity { get; set; }

		Vector2 mainbarPosition = new Vector2 (16, 16);
		Vector2 screenPosition = new Vector2 (16, 16);

		Vector2 tooltipPosition;
		string tooltipText;

		public Rectangle BoundingRect { get { return new Rectangle (
				(int)screenPosition.X, 
				(int)screenPosition.Y, 
				(Item.SpriteWidth + SlotSpacing) * Columns,
				(Item.SpriteWidth + SlotSpacing) * Rows);
				} }


		public Inventory (int rows, int columns) {
			Rows = rows;
			Columns = columns;
			slots = new InventorySlot[Columns, Rows];
			SelectedSlot = null;
			Opacity = 0.25f;
			MainbarOpacity = 0.5f;
			Initialize ();
		}

		public void Initialize() {
			for (int x = 0; x < Columns; x++) {
				for (int y = 0; y < Rows; y++) {

					//Screen rectangle
					Rectangle r = 
						new Rectangle (
							(int)screenPosition.X + (Item.SpriteWidth + SlotSpacing) * x,
							(int)screenPosition.Y+ (Item.SpriteWidth + SlotSpacing) * y, 
							Item.SpriteWidth, 
							Item.SpriteWidth);

					slots [x,y] = new InventorySlot (r);
				}
			}
			activeSlotIndex = 0;
		}



		public void Update(GameTime time, World world) {
			checkInventoryFull ();

			if (Open) checkInventoryItemSelected ();
			if (!Open)
				SelectedSlot = null;

			if (Mouse.GetState ().ScrollWheelValue - PlayerInput.PrevMouseState.ScrollWheelValue > 0)
				activeSlotIndex++;
			if (Mouse.GetState ().ScrollWheelValue - PlayerInput.PrevMouseState.ScrollWheelValue < 0)
				activeSlotIndex--;


			//TODO: Implement better controls.
			if (activeSlotIndex < 0)
				activeSlotIndex += Columns;
			if (activeSlotIndex >= Columns)
				activeSlotIndex -= Columns;

			//loop over number keys
			for (int i = 0; i < Columns; i++) {
				if (Columns > 10)
					throw new Exception ("Cannot have a mainbar larger than 10!");
				if (i != 9 && 
					Keyboard.GetState ().IsKeyDown (Keys.D1 + i) &&
				    PlayerInput.PrevKeyState.IsKeyUp (Keys.D1 + i))
					activeSlotIndex = i;
				else if (i==9 && 
					Keyboard.GetState ().IsKeyDown (Keys.D0) &&
					PlayerInput.PrevKeyState.IsKeyUp (Keys.D0))
					activeSlotIndex = i;
			}
			updateTooltip ();
		}

		void updateTooltip()
		{
			bool foundCursor = false;
			if (Open) {
				for (int i = 0; i < Columns; i++) {
					if (foundCursor)
						break;
					for (int j = 0; j < Rows; j++) {
						if (foundCursor)
							break;
						if (slots [i, j].CollisionBox.Contains (Mouse.GetState ().Position.X, Mouse.GetState ().Position.Y)) {
							if (slots [i, j].HasItem && slots[i,j]!=SelectedSlot) {
								tooltipText = slots [i, j].Item.ItemInfo;
								tooltipPosition = Mouse.GetState ().Position.ToVector2();
							}
							foundCursor = true;
						}
					}
				}
			}
			if (!foundCursor)
				tooltipText = "";
		}


		void drawSlot(SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, int i, int j, 
			float opacity)
		{

			int disp = SlotSpacing + Item.SpriteWidth;

			int x = (int)screenPosition.X + disp * i;
			int y = (int)screenPosition.Y + disp * j;


			//Box expanded by two to occupy a bit more space than item sprite itself.
			Rectangle rBox = new Rectangle (x-2, y-2, Item.SpriteWidth+4, Item.SpriteWidth+4);
			Rectangle r = new Rectangle (x, y, Item.SpriteWidth, Item.SpriteWidth);

			//draw background of slot
			spriteBatch.Draw (textureManager.GetTexture("inventory_box"), rBox,  (slots [i, j] == ActiveSlot ? Color.Red : Color.White) * opacity);
			//draw item itself
			if (slots [i, j].HasItem && slots [i, j] != SelectedSlot) {
				spriteBatch.Draw (textureManager.GetTexture ("items"),
					null,
					r,
					slots [i, j].Item.getTextureSourceRect (),
					null,
					0,
					null,
					Color.White,
					SpriteEffects.None,
					0);
				if (slots[i,j].Item.Stackable) 
					spriteBatch.DrawString (
						fontManager.GetFont ("Arial"),
						String.Format ("{0}", slots [i, j].Item.CurrentStackSize),
						new Vector2 (slots [i, j].CollisionBox.X, slots [i, j].CollisionBox.Y),
						Color.Black);
			}
		}


		/// <summary>
		/// Draw mainbar.
		/// </summary>
		public void DrawPartial(SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager) {
			//Draw only the top row
			for (int i = 0; i < Columns; i++) {
				drawSlot (spriteBatch, textureManager, fontManager, i, 0, MainbarOpacity);
			}
			drawDraggingItem (spriteBatch, textureManager, fontManager);
		}
		/// <summary>
		/// Draw entire inventory.
		/// </summary>
		public void DrawComplete(SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager) {

			for (int i = 0; i < Columns; i++)
				for (int j = 0; j < Rows; j++) {
					drawSlot (spriteBatch, textureManager, fontManager, i, j, Opacity);
				}
			drawDraggingItem (spriteBatch, textureManager, fontManager);
			drawTooltip (spriteBatch, fontManager);
		}

		void drawDraggingItem(SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager)
		{
			if (SelectedSlot != null) {
				if (SelectedSlot.HasItem) {

					Rectangle r = new Rectangle (Mouse.GetState ().Position.X, Mouse.GetState ().Position.Y, Item.SpriteWidth, Item.SpriteWidth);

					spriteBatch.Draw (textureManager.GetTexture ("items"),
						null,
						r,
						SelectedSlot.Item.getTextureSourceRect (),
						null,
						0,
						null,
						Color.White,
						SpriteEffects.None,
						0);
				}
			}
		}

		void drawTooltip(SpriteBatch spriteBatch, FontManager fontManager)
		{
			if (tooltipText != "" && Open) {
				spriteBatch.DrawString (fontManager.GetFont ("Arial"), tooltipText, 
					tooltipPosition
					+ new Vector2(1,1-fontManager.GetFont("Arial").MeasureString(tooltipText).Y), Color.Black);
				spriteBatch.DrawString (fontManager.GetFont ("Arial"), tooltipText, 
					tooltipPosition
					+ new Vector2(0,0-fontManager.GetFont("Arial").MeasureString(tooltipText).Y), Color.White);
			}
		}

		/// <summary>
		/// Checks if the inventory is full. If it is, you cannot add another item.
		/// </summary>
		private void checkInventoryFull() {
			numItems = 0;
			foreach (InventorySlot slot in slots) {
				
				if (slot.HasItem == true) {
					numItems++;
				}
			}

			if (numItems == Rows * Columns) {
				IsFull = true;
			} else {
				IsFull = false;
			}
		}

		/// <summary>
		/// Adds an item to the first empty/similar slot in the inventory.
		/// </summary>
		/// <returns><c>true</c>, if item was added, <c>false</c> otherwise.</returns>
		public bool AddItem(Item item) 
		{
			for (int j = 0; j < Rows; j++)
				for (int i = 0; i < Columns; i++) {
					if (slots [i,j].AddItem (item))
						return true;
				}
			return false;
		}


		/// <summary>
		/// Checks if an item in the inventory is going to be selected.
		/// </summary>
		private void checkInventoryItemSelected() {

			for (int j = 0; j < Rows; j++) {
				for (int i = 0; i < Columns; i++) {
					if (slots [i, j].CollisionBox.Contains (Mouse.GetState ().Position.X, Mouse.GetState().Position.Y) && 
						(Mouse.GetState().LeftButton == ButtonState.Pressed && PlayerInput.PrevMouseState.LeftButton==ButtonState.Released)) {

						if (slots [i, j] == SelectedSlot && SelectedSlot != null) {
							SelectedSlot = null;
							continue;
						}
						//If there is no selected slot, set the selected slot.
						if (SelectedSlot == null && slots [i, j].HasItem) {
						
							SelectedSlot = slots [i, j];
							continue;
						}
							
						//If you then click on another slot that is not the selected slot...
						if (SelectedSlot != slots [i, j] && SelectedSlot != null) {

							//If that slot	 doesn't have an item.
							if (slots [i, j].HasItem == false) {
								
								slots [i, j].AddItem (SelectedSlot.Item);
								SelectedSlot.Item = null;
								SelectedSlot = null;

								//If it does have an item.
							} else {

								//If the items are of the same type.
								if (slots [i, j].Item.Type == SelectedSlot.Item.Type) {

									//If they are stackable
									if (slots [i, j].Item.Stackable == true) {
									
										slots [i, j].Item.AddToStack (SelectedSlot.Item.CurrentStackSize);
										SelectedSlot.Item = null;
										SelectedSlot = null;
									} else {

										Console.WriteLine ("You cannot stack this item.");
										SelectedSlot = null;

									}

									//If they are not the same item you cannot stack them.
								} else {

									Console.WriteLine ("You cannot stack this item.");
									SelectedSlot = null;

								}
							}
						}
					}
				}
			}
		}


	}//END OF INVENTORY CLASS


} //END OF NAMESPACE


