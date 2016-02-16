

namespace ProjectCrusade
{
	public class Coin : Item
	{
		public Coin () {
			Stackable = true;
			Type = ItemType.Coin;
		}

		public override string ItemInfo() {
			return CurrentStackSize + " coin(s).";
		}
	} //END OF COIN CLASS






}

