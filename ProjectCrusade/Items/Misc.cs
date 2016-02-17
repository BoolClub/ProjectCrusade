

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

		public override void PrimaryUse (Player player)
		{
			throw new System.NotImplementedException ();
		}
		public override void SecondaryUse (Player player)
		{
			throw new System.NotImplementedException ();
		}
	} //END OF COIN CLASS






}

