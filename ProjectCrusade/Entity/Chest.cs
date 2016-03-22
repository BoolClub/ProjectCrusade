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
		public Chest (Item _item)
		{
			Width = 32;
			Height = 32;
			item = _item;
		}
		public override void InteractWithPlayer (Player player)
		{
			if (item!=null) player.Inventory.AddItem (item);
			item = null;
		}
		public override void Update (GameTime gameTime, World world)
		{
			
		}
		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, Color color)
		{
			spriteBatch.Draw (textureManager.WhitePixel, null, CollisionBox, null, null, 0, null, Color.Orange, SpriteEffects.None, 0.1f);
		}
	}
}

