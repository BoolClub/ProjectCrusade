using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	public class Enemy : Entity
	{
		public float Health { get; set; }

		/// <summary>
		/// How many pixels/sec the enemy moves.
		/// </summary>
		public float Speed { get; protected set; }

		public enum State { 
			Chasing, 
			Patrolling,
			Stationary
		}

		public State CurrState;
		/// <summary>
		/// A normal vector representing the current movement direction of the enemy
		/// </summary>
		Vector2 patrollingDirection;

		float patrollingTime = 0.0f;
		/// <summary>
		/// How much time has passed in ms since the enemy last saw the player
		/// </summary>
		float lastSeenPlayer;
		/// <summary>
		/// Time during which the enemy still chases after the player, even if it doesn't have a line of sight.
		/// </summary>
		const float playerMemoryTime = 10e3f;
		const float maxPatrollingTime = 1e3f;//in millis

		/// <summary>
		/// Relative to chasing speed, how slowly the enemy moves when patrolling.
		/// </summary>
		const float patrollingSpeedMult = 0.5f;


		/// <summary>
		/// A factor in the range [0,1) that determines how smoothly the enemy follows the new path. Uses a kind of exponential interpolation.
		/// </summary>
		const float pathFollowingSmoothness = 0.9f;

		Point[] pathToPlayer;
		int currPointOnPath = 0;
		/// <summary>
		/// How many frames have passed since last update of path
		/// </summary>
		int lastPathUpdate = 0;
		/// <summary>
		/// How many frames to skip until next path update.
		/// </summary>
		const int pathUpdatePeriod = 24;



		public Enemy ()
		{
			Width = 16;
			Height = 16;
			Speed = 120;
			patrollingDirection = new Vector2 (1, 0);

			lastSeenPlayer = playerMemoryTime;
		}

		/// <summary>
		/// Returns a random normal vector
		/// </summary>
		Vector2 randomDirection(Random rand)
		{
			return Vector2.Normalize(new Vector2 ((float)rand.NextDouble () - .5f, (float)rand.NextDouble () - .5f));
		}


		public override void Update (GameTime gameTime, World world) 
		{
			bool line_sight = world.HasLineOfSight (world.WorldToTileCoord (Position), world.WorldToTileCoord (world.Player.Position));
			if (line_sight)
				lastSeenPlayer = 0;
			const float interactionRadius = 1000;
			if (lastSeenPlayer < playerMemoryTime && (world.Player.Position - Position).LengthSquared () < interactionRadius * interactionRadius)
				CurrState = State.Chasing;
			else if (CurrState==State.Chasing) 
				CurrState = State.Patrolling;

			//Change direction
			if (world.rand.NextDouble() < 0.001 * gameTime.ElapsedGameTime.TotalMilliseconds)
			{
				if (CurrState == State.Patrolling)
					CurrState = State.Stationary;
				else if (CurrState == State.Stationary) {
					CurrState = State.Patrolling;
					patrollingDirection = randomDirection (world.rand);
				}
				patrollingTime = 0.0f;
			}
			float displacement = (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;
			switch (CurrState) {
			case State.Chasing:
				if (line_sight) {
					patrollingDirection = Vector2.Normalize (world.Player.Position - Position);
				}
				else {
					if (lastPathUpdate > pathUpdatePeriod || pathToPlayer==null) {
						pathToPlayer = world.Pathfind (world.WorldToTileCoord (CollisionBox.Center), world.WorldToTileCoord (world.Player.CollisionBox.Center));
						lastPathUpdate = 0;
					}
					for (int i = 0; i < pathToPlayer.Length; i++)
						if (world.WorldToTileCoord (Position) == pathToPlayer [i]) {
							currPointOnPath = i;
							break;
						}
					//if the enemy is basically at the player's location, break
					if (currPointOnPath >= pathToPlayer.Length - 1)
						break;
					Vector2 nvec = Vector2.Normalize (world.TileToWorldCoord (pathToPlayer [currPointOnPath + 1]) - Position);
					patrollingDirection = Vector2.Normalize (pathFollowingSmoothness * patrollingDirection + (1 - pathFollowingSmoothness) * nvec);
				}

				Position += displacement * (patrollingDirection);
				break;
			case State.Patrolling:
				Position += patrollingDirection * displacement*patrollingSpeedMult;
				break;
			case State.Stationary:

				break;
			}
			patrollingTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			lastSeenPlayer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			lastPathUpdate++;
		}

		public override void Draw (SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager, Color color)
		{
			spriteBatch.Draw (textureManager.GetTexture ("circle"), null, CollisionBox, null, null, 0, null, color, SpriteEffects.None, 0.1f);
		}
	}
}

