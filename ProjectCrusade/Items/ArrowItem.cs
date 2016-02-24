using System;

namespace ProjectCrusade
{
	public abstract class ArrowItem : WeaponItem
	{

		public override bool Depletable 		{ get { return true; 	} }
		public override bool Stackable 			{ get { return true; 	} }
		protected override bool Degradable 		{ get { return false; 	} }


		public ArrowItem ()
		{
			if (Player.PlayerType == PlayerType.Arrowman) {
				Useable = true;
			} else {
				Useable = false;
			}
		}

		//All arrows have same behavior
		public override void PrimaryUse (Player player, World world)
		{
			throw new NotImplementedException ();
		}
		public override void SecondaryUse (Player player, World world)
		{
			throw new NotImplementedException ();
		}
	}


	//An arrow class for when the player chooses to be use ranged combat.
	public class StarterArrow : ArrowItem
	{
		public override ItemType Type 			{ get { return ItemType.StarterArrow; } }
		public override string ItemInfo 		{ get { return "A basic arrow. I could shoot this using a bow."; } }
		protected override int Damage 			{ get { return 10; } }

		public StarterArrow () { 
			
		}

	}

}

