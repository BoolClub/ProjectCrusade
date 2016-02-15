using System;

namespace ProjectCrusade
{
	public class Apple : FoodItem
	{
		public Apple () {
			ItemID = 1;
		}

		public override void Consume() {
			//Add to the player's health.
		}

		public override String ItemInfo() {
			return "An apple. Restores health.";
		}


	} //	END OF CLASS




}//END OF NAMESPACE

