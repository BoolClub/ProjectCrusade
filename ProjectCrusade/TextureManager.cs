using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ProjectCrusade
{

	/// <summary>
	/// Stores all sprite textures centrally and provides a simple way to reuse textures.
	/// </summary>
	public class TextureManager
	{

		Dictionary<string, Texture2D> textures;
		public TextureManager (ContentManager content)
		{
			textures = new Dictionary<string, Texture2D> ();

			//Load all textures here
			textures["circle"] = content.Load<Texture2D>("Textures/circle");

			textures ["tiles"] = content.Load<Texture2D> ("Textures/tiles");
		}

		public Texture2D GetTexture(string id) { return textures[id]; }
	}
}

