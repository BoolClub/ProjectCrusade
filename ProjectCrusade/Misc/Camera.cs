using System;

using Microsoft.Xna.Framework;

namespace ProjectCrusade
{
	/// <summary>
	/// Stores and handles camera transformations (matrix transformations). Allows for translation, rotation, and scaling.
	/// </summary>
	public class Camera
	{
		public Vector2 Position;
		public float Rotation {get; set;}
		public float Scale { get; set; }

		/// <summary>
		/// Passed into SpriteBatch.Begin to transform all drawn sprites from world space to screen space.
		/// </summary>
		/// <value>The transform matrix.</value>
		public Matrix TransformMatrix { get; private set; }
		/// <summary>
		/// Converts screen space to world space.
		/// </summary>
		/// <value>The inverse matrix.</value>
		public Matrix InverseMatrix { get; private set; }

		public Camera ()
		{
			Position = new Vector2 ();
			Rotation = 0.0f;
			Scale = 1.0f;
			Update ();
		}

		/// <summary>
		/// Updates transformation matrices.
		/// </summary>
		public void Update() {

			TransformMatrix = Matrix.CreateRotationZ (Rotation) *
				Matrix.CreateScale (new Vector3 (Scale, Scale, 1.0f)) *
				Matrix.CreateTranslation (-Position.X, -Position.Y, 0);
			InverseMatrix = Matrix.Invert (TransformMatrix);
		}

		/// <summary>
		/// Used to convert a position on the screen to a position in world space (i.e. cursor position to world space).
		/// </summary>
		/// <returns>The world position.</returns>
		/// <param name="screenPosition">Screen coordinates.</param>
		public Vector2 GetWorldPosition (Vector2 screenPosition)
		{
			return Vector2.Transform (screenPosition, InverseMatrix);
		}
	}
}

