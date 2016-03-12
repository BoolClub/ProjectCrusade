using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace ProjectCrusade
{
	public class SoundEffectManager
	{

		Dictionary<string, SoundEffect> soundEffects;
		public SoundEffectManager (ContentManager content)
		{
			soundEffects = new Dictionary<string, SoundEffect> ();

			//Automatically load all files in Fonts folder.

			DirectoryInfo dir = new DirectoryInfo (content.RootDirectory + "/SoundEffects/");

			FileInfo[] files = dir.GetFiles ("*.*");
			foreach (FileInfo file in files) {
				string key = Path.GetFileNameWithoutExtension (file.Name);
				soundEffects [key] = content.Load<SoundEffect> ("SoundEffects/" + key);
			}
		}

		public SoundEffect GetSoundEffect(string id) { return soundEffects[id]; }
	}
}

