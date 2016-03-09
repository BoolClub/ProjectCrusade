using System;

namespace ProjectCrusade
{
	public abstract class WandItem : WeaponItem
	{
		public override bool Depletable 		{ get { return false; 	} }
		public override bool Stackable 			{ get { return false; 	} }
		protected override bool Degradable 		{ get { return false; 	} }

		public WandItem ()
		{
			if (Player.PlayerType == PlayerType.Wizard) {
				Useable = true;
			} else {
				Useable = false;
			}
		}

		//All arrows have same behavior
		public override void PrimaryUse (World world)
		{
			throw new NotImplementedException ();
		}
		public override void SecondaryUse (World world)
		{
			throw new NotImplementedException ();
		}
	}



	public class MagicWand : WandItem {

		public override ItemType Type 			{ get { return ItemType.MagicWand; } }
		public override bool Depletable 		{ get { return false; } }
		protected override int Damage 			{ get { return 10; } }
		public override string Tooltip 		{ get { return "A magical wand!"; } }


		public MagicWand () {

		}


	} //END OF MAGICWAND CLASS


}

