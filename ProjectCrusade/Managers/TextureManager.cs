using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace ProjectCrusade
{

	/// <summary>
	/// Stores all sprite textures centrally and provides a simple way to reuse textures.
	/// </summary>
	public class TextureManager
	{
		/// <summary>
		/// Used to draw rectangles.
		/// </summary>
		public Texture2D WhitePixel { get; private set; }

		Dictionary<string, Texture2D> textures;
		public TextureManager (ContentManager content, Texture2D whitePix)
		{
			textures = new Dictionary<string, Texture2D> ();

			//Automatically load all files in Textures folder.

			DirectoryInfo dir = new DirectoryInfo (content.RootDirectory + "/Textures");

			FileInfo[] files = dir.GetFiles ("*.*");
			foreach (FileInfo file in files) {
				string key = Path.GetFileNameWithoutExtension (file.Name);
				textures [key] = content.Load<Texture2D> ("Textures/" + key);
			}
			WhitePixel = whitePix;
		}

		public Texture2D GetTexture(string id) { return textures[id]; }
	}
}

