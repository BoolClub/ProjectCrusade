using System;

namespace ProjectCrusade
{
	public abstract class FoodItem : Item
	{
		public abstract int HealValue { get; }

		public FoodItem () {
		}

		//All food items heal.
		public override void PrimaryUse (Player player, World world)
		{
			if (player.Sanity + HealValue <= 100) {
				player.Heal (HealValue);
				Console.WriteLine ("Sanity: " + player.Sanity);
			} else {
				player.Sanity = 100;
				Console.WriteLine ("Sanity: " + player.Sanity);
			}
		}
		
	
	} //END OF FOODITEM CLASS

	public class Apple : FoodItem
	{
		public override ItemType Type 			{ get { return ItemType.Apple; } }
		public override bool Stackable 			{ get { return false; 	} }
		public override bool Depletable 		{ get { return true; 	} }
		public override int HealValue 			{ get { return 10; 		} }
		public override string ItemInfo 		{ get { return "An apple. Restores health."; 		} }


		public Apple () { }

	} //END OF APPLE CLASS


	public class Water : FoodItem
	{

		public override ItemType Type 			{ get { return ItemType.Water; } }
		public override bool Stackable 			{ get { return false; 	} }
		public override bool Depletable 		{ get { return true; 	} }
		public override int HealValue 			{ get { return 1; 		} }
		public override string ItemInfo 		{ get { return "Nice, refreshing water."; 		} }

		public Water () { }
	} //END OF WATER CLASS


	public class Bread : FoodItem
	{

		public override ItemType Type 			{ get { return ItemType.Bread; } }
		public override bool Stackable 			{ get { return false; 	} }
		public override bool Depletable 		{ get { return true; 	} }
		public override int HealValue 			{ get { return 1; 		} }
		public override string ItemInfo 		{ get { return "A big loaf of bread."; 	} }

		public Bread () { }
	} 



} //END OF NAMESPACE

