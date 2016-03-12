using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace ProjectCrusade
{
	public class SongManager
	{

		Dictionary<string, Song> songs;
		public SongManager (ContentManager content)
		{
			songs = new Dictionary<string, Song> ();

			//Automatically load all files in Fonts folder.

			DirectoryInfo dir = new DirectoryInfo (content.RootDirectory + "/Songs/");

			FileInfo[] files = dir.GetFiles ("*.*");
			foreach (FileInfo file in files) {
				string key = Path.GetFileNameWithoutExtension (file.Name);
				songs [key] = content.Load<Song> ("Songs/" + key);
			}
		}

		public Song GetSong(string id) { return songs[id]; }
	}
}

