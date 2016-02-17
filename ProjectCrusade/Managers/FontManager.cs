using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace ProjectCrusade
{
	public class FontManager
	{
		Dictionary<string, SpriteFont> fonts;
		public FontManager (ContentManager content)
		{
			fonts = new Dictionary<string, SpriteFont> ();

			//Automatically load all files in Fonts folder.

			DirectoryInfo dir = new DirectoryInfo (content.RootDirectory + "/Fonts/");

			FileInfo[] files = dir.GetFiles ("*.*");
			foreach (FileInfo file in files) {
				string key = Path.GetFileNameWithoutExtension (file.Name);
				fonts [key] = content.Load<SpriteFont> ("Fonts/" + key);
			}
		}

		public SpriteFont GetFont(string id) { return fonts[id]; }
	}
}

