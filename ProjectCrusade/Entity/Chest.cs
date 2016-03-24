using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	/// <summary>
	/// Entity that gives the player a certain item when interacted with
	/// </summary>
	public class Chest : Entity
	{
		public bool Empty { get { return item == null; } }
		Item item;
		TextBox textbox;
		bool textBoxVisible = false;
		float lastInteracted = 0f;
		const float idleTime = 8e3f; //if no interaction for this time in ms, hide text box

		public Chest (Item _item)
		{
			Width = 32;
			Height = 32;
			item = _item;
			textbox = new TextBox (256,150,Position,Color.Black, Color.White);
		}
		public override void InteractWithPlayer (Player player)
		{
			if (item!=null) player.Inventory.AddItem (item);
			//item = null;

			if (item != null) {
				textbox.AddText ("You received a " + item.ItemName);
				if (!textBoxVisible) {
					textBoxVisible = true;
				} else {
					textBoxVisible = false;
					textbox.RemoveAllText ();
				}
				lastInteracted = 0f;
			}
		}
		public override void Update (GameTime gameTime, World world)
		{
			if(item != null)
				if (textBoxVisible) textbox.Update (gameTime);

			if (lastInteracted > idleTime) {
				textBoxVisible = false;
				item = null;
			}
			lastInteracted += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
		}
		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, Color color)
		{
			spriteBatch.Draw (textureManager.WhitePixel, null, CollisionBox, null, null, 0, null, Color.Orange, SpriteEffects.None, 0.1f);
		
			//If interacting with the player...
			if(item != null)
				textbox.Position = Position;
				if (textBoxVisible) textbox.Draw (spriteBatch, textureManager, fontManager);
		}
	}
}

