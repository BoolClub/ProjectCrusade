using System;

using Microsoft.Xna.Framework;

namespace ProjectCrusade
{
	public class Camera
	{
		public Vector2 Position;
		public float Rotation {get; set;}
		public float Scale { get; set; }

		public Matrix TransformMatrix { get; private set; }
		public Matrix InverseMatrix { get; private set; }

		public Camera ()
		{
			Position = new Vector2 ();
			Rotation = 0.0f;
			Scale = 1.0f;
		}

		public void Update() {

			TransformMatrix = Matrix.CreateRotationZ (Rotation) *
				Matrix.CreateScale (new Vector3 (Scale, Scale, 1.0f)) *
				Matrix.CreateTranslation (-Position.X, -Position.Y, 0);
			InverseMatrix = Matrix.Invert (TransformMatrix);
		}
	}
}

