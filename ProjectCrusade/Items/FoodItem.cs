using System;

namespace ProjectCrusade
{
	public abstract class FoodItem : Item
	{
		public abstract int HealValue { get; }

		//All food items heal.
		public override void PrimaryUse (World world)
		{
				world.Player.Heal (HealValue);
				Console.WriteLine ("Sanity: " + world.Player.Sanity);
		}
		
	
	}

	public class Apple : FoodItem
	{
		public override ItemType Type 			{ get { return ItemType.Apple; } }
		public override bool Stackable 			{ get { return true; 	} }
		public override bool Depletable 		{ get { return true; 	} }
		public override int HealValue 			{ get { return 10; 		} }
		public override string Tooltip 		{ get { return "An apple. Restores health."; 		} }
		public override string Name 	{ get { return "apple"; } }
	}


	public class Water : FoodItem
	{

		public override ItemType Type 			{ get { return ItemType.Water; } }
		public override bool Stackable 			{ get { return true; 	} }
		public override bool Depletable 		{ get { return true; 	} }
		public override int HealValue 			{ get { return 1; 		} }
		public override string Tooltip 		{ get { return "Nice, refreshing water."; 		} }
		public override string Name 	{ get { return "water"; } }
	}


	public class Bread : FoodItem
	{

		public override ItemType Type 			{ get { return ItemType.Bread; } }
		public override bool Stackable 			{ get { return true; 	} }
		public override bool Depletable 		{ get { return true; 	} }
		public override int HealValue 			{ get { return 1; 		} }
		public override string Tooltip 		{ get { return "A big loaf of bread."; 	} }
		public override string Name 	{ get { return "bread"; } }
	} 



} //END OF NAMESPACE

