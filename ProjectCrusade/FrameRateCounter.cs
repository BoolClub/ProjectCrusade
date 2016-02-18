using System;
using Microsoft.Xna.Framework;

namespace ProjectCrusade
{
	/// <summary>
	/// Provides smooth frame rates (a moving average of elapsed frame times).
	/// </summary>
	public class FrameRateCounter
	{
		int bufferSize;
		float[] avgBuffer;
		int currentIndex = 0;

		/// <summary>
		/// Average time taken to update a frame.
		/// </summary>
		public float AverageElapsedMilliseconds { get; private set; }

		/// <summary>
		/// Average frames per second.
		/// </summary>
		public float AverageFrameRate { get { return 1.0e3f / AverageElapsedMilliseconds; } }

		/// <summary>
		/// Initializes a frame rate counter.
		/// </summary>
		/// <param name="bufferSize">Over how many frames to average.</param>
		public FrameRateCounter (int bufferSize = 16)
		{
			this.bufferSize = bufferSize;
			avgBuffer = new float[bufferSize];
		}

		public void Update(GameTime time)
		{
			avgBuffer [currentIndex] = (float)time.ElapsedGameTime.TotalMilliseconds;
			AverageElapsedMilliseconds = 0;
			for (int i = 0; i < bufferSize; i++)
				AverageElapsedMilliseconds += avgBuffer [i];
			AverageElapsedMilliseconds /= bufferSize;

			currentIndex++;
			if (currentIndex >= bufferSize)
				currentIndex = 0;
		}
	}
}

