

namespace ProjectCrusade
{
	public class Coin : Item
	{

		public override bool Depletable { get { return false; } }
		public Coin (int stackSize = 1) : base(stackSize) {
			Stackable = true;
			Type = ItemType.Coin;
		}

		public override string ItemInfo() {
			return CurrentStackSize + " coin(s).";
		}

	} //END OF COIN CLASS






}

