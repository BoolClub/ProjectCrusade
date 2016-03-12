using System;
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
		Dictionary<String, Objective> objectives;



		public ObjectiveManager () {
			objectives = new Dictionary<string, Objective> ();
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

		}


	} //END OF OBJECTIVE MANAGER CLASS

} //END OF NAMESPACE

