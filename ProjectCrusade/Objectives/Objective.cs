using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	/// <summary>
	/// The Objective class. Responsible for making an objective for the player to complete.
	/// </summary>
	public class Objective
	{
		/// <summary>
		/// The region (tile) that this objective will be triggered by. This will be used for objectives that need
		/// to be triggered by stepping at a certain location, for example.
		/// </summary>
		public Rectangle Region { get; set; }


		/// <summary>
		/// The NPC that will affect this object. This would typically be something like talking to a particular
		/// npc at a certain point in the game, etc.
		/// **This may or may not be necessary right now, but I thought I might as well throw it in for now.
		/// </summary>
		public NPC npc { get; set; }


		/// <summary>
		/// True/False for whether or not this objective has been completed
		/// </summary>
		public bool Completed { get; set; }


		/// <summary>
		/// True/False for whether or not you can complete this objective right now.
		/// </summary>
		public bool Active { get; set; }


		/// <summary>
		/// Basically just shows what the player must do. (i.e. "Talk to a particular npc.").
		/// </summary>
		/// <value>The short description.</value>
		public string Description { get; set; }


		public event ObjectiveReachedHandler ObjectiveReached;

		public delegate void ObjectiveReachedHandler(Objective obj, ObjectiveManager manager, Player player, World world);

		//Constructor
		public Objective () {
			
		}

		public Objective(Rectangle region, bool active=true) {
			Region = region;
			Active = active;
		}

		public void Update(GameTime time, Player player, World world, ObjectiveManager objManager) {
			if (player.CollisionBox.Intersects (Region) && Active) {
				ObjectiveReached (this, objManager, player, world);
				Active = false;
				Completed = true;
			}
		}

	} //END OF OBJECTIVE CLASS
} //END OF NAMESPACE

