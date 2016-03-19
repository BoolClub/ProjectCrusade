using System;
using Microsoft.Xna.Framework;

namespace ProjectCrusade
{
	public static class Util
	{
		public static Color TintColor(Color col, Color tint)
		{
			return new Color (col.ToVector3 () * tint.ToVector3 ());
		}
	}
}

