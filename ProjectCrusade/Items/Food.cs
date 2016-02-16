using System;

namespace ProjectCrusade
{
	public class Apple : FoodItem
	{
		public Apple () {
			Type = ItemType.Apple;
		}

		public override void Consume() {
			//Add to the player's health.
		}

		public override String ItemInfo() {
			return "An apple. Restores health.";
		}

	} //END OF APPLE CLASS




	public class Water : FoodItem
	{
		public Water () {
			Type = ItemType.Water;
		}

		public override void Consume() {
			//Add to the player's health.
		}

		public override String ItemInfo() {
			return "Nice, refreshing water.";
		}


	} //END OF WATER CLASS




} //END OF NAMESPACE

