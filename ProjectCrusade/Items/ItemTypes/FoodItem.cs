using System;

namespace ProjectCrusade
{
	public abstract class FoodItem : Item
	{
		public abstract int HealValue { get; }

		public FoodItem () {
			Stackable = false;
			CurrentStackSize = 1;
		}

		//All food items heal.
		public override void PrimaryUse (Player player)
		{
			player.Heal (HealValue);
		}
		public override void SecondaryUse (Player player)
		{
			throw new NotImplementedException ();
		}
		
	
	} //END OF FOODITEM CLASS


} //END OF NAMESPACE

