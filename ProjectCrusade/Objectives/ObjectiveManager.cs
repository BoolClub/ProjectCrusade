﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	/// <summary>
	/// This class manages all of the objectives in the game.
	/// </summary>
	public class ObjectiveManager
	{
		/// <summary>
		/// A dictionary for storing all of the game's objectives. It stores them with a string identifier and a corresponding 
		/// Objective.
		/// </summary>
		Dictionary<string, Objective> objectives;



		public ObjectiveManager () {
			objectives = new Dictionary<string, Objective> ();
		}

		public void PushListeners()
		{
			objectives ["NextLevel"].ObjectiveReached += NextLevelObjective;
		}

		public void ClearObjectives()
		{
			objectives.Clear ();
		}

		public void AddObjective(string identifier, Objective obj)
		{
			objectives [identifier] = obj;
		}

		public void Update(GameTime time, Player player, World world) {
			foreach (var objective in objectives)
				objective.Value.Update (time, player, world, this);
		}


		#region
		public static void NextLevelObjective(Objective obj, ObjectiveManager manager, Player player, World world)
		{
			world.AdvanceWorld ();
		}
		#endregion


	} //END OF OBJECTIVE MANAGER CLASS

} //END OF NAMESPACE

