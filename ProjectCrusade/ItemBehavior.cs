using System;
using System.Collections.Generic;
using System.Timers;
namespace ProjectCrusade
{
	public class ItemBehavior
	{
		public static void RunBehavior (string type, List<Tuple<string, float, int, int, int>> behaviors, World world)
		{
			if (type.Equals ("consumable")) {
				foreach (Tuple<string, float, int, int, int> behavior in behaviors) {
					switch (behavior.Item1) {
					case "HEALTH": 
						Health (behavior.Item2, behavior.Item3, behavior.Item4, world.Player);
						break;
					
					}
				}
			}
		}
		private static void Health(float value, int time, int length, Player player)
		{
			//TODO: implement effect over time and time length
			player.Sanity+=value;
			if (player.Sanity > player.MaxSanity)
				player.Sanity = player.MaxSanity;
		}
	}
}

