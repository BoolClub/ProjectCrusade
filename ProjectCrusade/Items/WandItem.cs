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

		public override ItemSprite Type 			{ get { return ItemSprite.MagicWand; } }
		public override bool Depletable 		{ get { return false; } }
		protected override int Damage 			{ get { return 10; } }
		public override string Tooltip 		{ get { return base.Tooltip + "A magical wand!"; } }
		protected override string BaseName 	{ get { return "Wand"; } }
		protected override float CoolDownTime { get { return 1000f; } }
		public MagicWand () {

		}


	} //END OF MAGICWAND CLASS


}

