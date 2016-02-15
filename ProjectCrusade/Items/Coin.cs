

namespace ProjectCrusade
{
	public class Coin : Item
	{
		public Coin () {
			Stackable = true;
		}

		public override string ItemInfo() {
			return CurrentStackSize + " coin(s).";
		}




	} //END OF COIN CLASS



}//END OF NAMESPACE

