using System;

namespace ProjectCrusade
{
	public abstract class FoodItem : Item
	{
		public FoodItem () {
			Stackable = false;
			CurrentStackSize = 1;
		}


		//A method for consuming the food item.
		public abstract void Consume();
	
	
	} //END OF FOODITEM CLASS


} //END OF NAMESPACE

