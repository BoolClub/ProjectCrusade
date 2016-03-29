using System;

namespace ProjectCrusade
{
	public class Coin : Item
	{

		public override ItemType Type 			{ get { return ItemType.Coin; } }
		public override bool Stackable 			{ get { return true; 	} }
		public override bool Depletable 		{ get { return false; 	} }
		public override string Tooltip 		{ get { return String.Format("{0} coin{1}.", Count, Count==1 ? "" : "s"); 		} }
		public override string Name		{ get { return "Coin"; }}

		public Coin (int stackSize = 1) : base(stackSize) {
		}



	} //END OF COIN CLASS

}

