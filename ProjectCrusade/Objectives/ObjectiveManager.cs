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
		Dictionary<string, Objective> objectives;




		public ObjectiveManager () {
			objectives = new Dictionary<string, Objective> ();
		}

		public void PushListeners()
		{
			objectives ["NextLevel"].ObjectiveReached += NextLevelObjective;

			/* Once we are ready to add these features in, we can comment out the objectives below. */

//			objectives ["TalkToPriest"].ObjectiveReached += TalkToPriestObjective;
//			objectives ["EnterHouseOne"].ObjectiveReached += EnterHouseOneObjective;
//			objectives ["EnterHouseTwo"].ObjectiveReached += EnterHouseTwoObjective;
//			objectives ["EnterHouseThree"].ObjectiveReached += EnterHouseThreeObjective;
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
			if(objectives["TalkToPriest"].Completed)
				world.AdvanceWorld ();
		}

		//For when the player speaks to the priest for the first time in the game.
		public static void TalkToPriestObjective(Objective obj, ObjectiveManager manager, Player player, World world)
		{

		}

		//For entering the first house in the overworld.
		public static void EnterHouseOneObjective(Objective obj, ObjectiveManager manager, Player player, World world)
		{
			//Something to send the player to inside the first house
		}

		//For entering the second house in the overworld.
		public static void EnterHouseTwoObjective(Objective obj, ObjectiveManager manager, Player player, World world)
		{
			//Something to send the player to inside the second house
		}

		//For entering the third house in the overworld.
		public static void EnterHouseThreeObjective(Objective obj, ObjectiveManager manager, Player player, World world)
		{
			//Something to send the player to inside the third house
		}
		#endregion


	} //END OF OBJECTIVE MANAGER CLASS

} //END OF NAMESPACE

