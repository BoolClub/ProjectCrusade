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
		/// A dictionary for storing all of the game's objectives. It stores them with a Name and a corresponding 
		/// Objective.
		/// </summary>
		public Dictionary<String, Objective> Objectives { get; set; }



		public ObjectiveManager () {
			
		}


		public void Update(GameTime time, Player player, World world) {

		}


	} //END OF OBJECTIVE MANAGER CLASS

} //END OF NAMESPACE

