using System;

namespace ProjectCrusade
{
	public class Coin : Item
	{
		public Coin () {
			Stackable = true;
			ItemID = 2;
		}

		public override String ItemInfo() {
			return CurrentStackSize + " coin(s).";
		}




	} //END OF COIN CLASS



}//END OF NAMESPACE

