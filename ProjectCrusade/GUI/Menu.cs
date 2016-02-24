using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	public class MenuItem
	{
		public string Text;


		public event ActivatedHandler Activated;
		EventArgs e = null;
		public delegate void ActivatedHandler(MenuItem item, EventArgs e);



		public MenuItem(string text) { 
			Text = text;
		}

		public void Activate ()
		{
			Activated (this, e);
		}
	}


	public class Menu
	{
		List<MenuItem> menuItems;
		int selectedMenuItem = 0;
		public Vector2 Position { get; set; }
		public float TextHeight { get; set; }

		string fontName;

		public Menu (Vector2 pos, string _fontName, float textHeight)
		{
			menuItems = new List<MenuItem> ();
			Position = pos;
			fontName = _fontName;
			TextHeight = textHeight;
		}

		public void AddItem(MenuItem item) {
			menuItems.Add (item);
		}

		public void Update(KeyboardState prevKeyState)
		{
			if (Keyboard.GetState ().IsKeyDown (Keys.Enter) && prevKeyState.IsKeyUp(Keys.Enter))
				menuItems [selectedMenuItem].Activate ();


			if ((Keyboard.GetState ().IsKeyDown (Keys.W) && prevKeyState.IsKeyUp (Keys.W)) || (Keyboard.GetState ().IsKeyDown (Keys.Up) && prevKeyState.IsKeyUp (Keys.Up)))
				selectedMenuItem--;
			if ((Keyboard.GetState ().IsKeyDown (Keys.S) && prevKeyState.IsKeyUp (Keys.S)) || (Keyboard.GetState ().IsKeyDown (Keys.Down) && prevKeyState.IsKeyUp (Keys.Down)))
				selectedMenuItem++;

			//Wrap-around
			if (selectedMenuItem < 0)
				selectedMenuItem += menuItems.Count;
			if (selectedMenuItem >= menuItems.Count)
				selectedMenuItem -= menuItems.Count;

		}

		public void Draw(SpriteBatch spriteBatch, FontManager fontManager)
		{
			for (int i = 0; i < menuItems.Count; i++) {
				spriteBatch.DrawString (
					fontManager.GetFont(fontName), 
					menuItems [i].Text, 
					Position + new Vector2 (
						-(float)fontManager.GetFont(fontName).MeasureString(menuItems[i].Text).X/2, 
						i * TextHeight), 
					i==selectedMenuItem ? Color.Red : Color.White);
			}
		}
	}
}

