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
		const float idleTime = 4e3f; //if no interaction for this time in ms, hide text box

		public Chest (Item _item)
		{
			Width = 32;
			Height = 32;
			item = _item;
			textbox = new TextBox (256,150,Position,Color.Black, Color.White, 0.5f);

		}
		public override void InteractWithPlayer (Player player)
		{
			if (item != null) {
				item = new Sword ();
				Sword s = item as Sword;
				s.TierOne = WeaponItem.TierOneProperty.Beguiling;
				s.TierTwo = WeaponItem.TierTwoProperty.Heavy;
				s.TierThree = WeaponItem.TierThreeProperty.the_Ages;
				player.Inventory.AddItem (item);
				string name = item.Name;

				//check if first letter is a vowel 
				bool isVowel = "aeiouAEIOU".IndexOf(name[0]) >= 0;

				textbox.AddText (String.Format("You received {0}\n\n{1}{2}.", item.Count==1 ? (isVowel ? "an" : "a") : item.Count.ToString(), name, item.Count == 1 ? "" : "s"));
				textBoxVisible = true;
				item = null;		
				lastInteracted = 0f;
			}
		}
		public override void Update (GameTime gameTime, World world)
		{
			if (textBoxVisible) textbox.Update (gameTime);

			if (lastInteracted > idleTime) {
				textBoxVisible = false;
			}
			textbox.Position = Position;
			lastInteracted += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
		}
		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, Color color)
		{
			spriteBatch.Draw (textureManager.WhitePixel, null, CollisionBox, null, null, 0, null, Color.Orange, SpriteEffects.None, 0.1f);
		
			if (textBoxVisible) textbox.Draw (spriteBatch, textureManager, fontManager);
		}
	}
}

