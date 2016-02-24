

namespace ProjectCrusade
{
	public class Coin : Item
	{

		public override ItemType Type 			{ get { return ItemType.Coin; } }
		public override bool Stackable 			{ get { return true; 	} }
		public override bool Depletable 		{ get { return false; 	} }
		public override string ItemInfo 		{ get { return CurrentStackSize + " coin(s)."; 		} }


		public Coin (int stackSize = 1) : base(stackSize) {
		}


	} //END OF COIN CLASS

}

