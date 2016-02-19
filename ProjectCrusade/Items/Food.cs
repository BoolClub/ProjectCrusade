using System;

namespace ProjectCrusade
{
	public class Apple : FoodItem
	{

		public override bool Depletable { get { return false; } }
		public override int HealValue {
			get {
				return 10;
			}
		}

		public Apple () {
			Type = ItemType.Apple;
		}


		public override String ItemInfo() {
			return "An apple. Restores health.";
		}

	} //END OF APPLE CLASS




	public class Water : FoodItem
	{
		public override int HealValue {
			get {
				return 1;
			}
		}
		public Water () {
			Type = ItemType.Water;
		}

		public override bool Depletable { get { return true; } } 

		public override String ItemInfo() {
			return "Nice, refreshing water.";
		}


	} //END OF WATER CLASS




} //END OF NAMESPACE

