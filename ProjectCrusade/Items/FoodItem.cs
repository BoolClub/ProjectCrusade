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
		}
		
	
	}

	public class Apple : FoodItem
	{
		public override ItemSprite Type 			{ get { return ItemSprite.Apple; } }
		public override bool Stackable 			{ get { return true; 	} }
		public override bool Depletable 		{ get { return true; 	} }
		public override int HealValue 			{ get { return 10; 		} }
		public override string Tooltip 		{ get { return "An apple. Restores health."; 		} }
		public override string Name 	{ get { return "Apple"; } }
	}


	public class Water : FoodItem
	{

		public override ItemSprite Type 			{ get { return ItemSprite.Water; } }
		public override bool Stackable 			{ get { return true; 	} }
		public override bool Depletable 		{ get { return true; 	} }
		public override int HealValue 			{ get { return 1; 		} }
		public override string Tooltip 		{ get { return "Nice, refreshing water."; 		} }
		public override string Name 	{ get { return "Water"; } }
	}


	public class Bread : FoodItem
	{

		public override ItemSprite Type 			{ get { return ItemSprite.Bread; } }
		public override bool Stackable 			{ get { return true; 	} }
		public override bool Depletable 		{ get { return true; 	} }
		public override int HealValue 			{ get { return 1; 		} }
		public override string Tooltip 		{ get { return "A big loaf of bread."; 	} }
		public override string Name 	{ get { return "Bread"; } }
	} 



} //END OF NAMESPACE

