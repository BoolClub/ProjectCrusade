using Microsoft.Xna.Framework;


namespace ProjectCrusade
{


	public class Light
	{
		public Vector2 Position;
		public Color Color;
		public float Strength;

		public Light (Vector2 pos, Color color, float strength) { 
			Position = pos;
			Color = color;
			Strength = strength;
		}
	}
}

