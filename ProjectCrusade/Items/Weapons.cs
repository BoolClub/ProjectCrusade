using System;

namespace ProjectCrusade
{
	public class WoodenSword : SwordItem
	{
		protected override int Damage {
			get {
				return 10;
			}
		}

		public WoodenSword () : base() {

			//Item's type
			Type = ItemType.WoodenSword;

			//A wooden sword is not degradable
			Degradable = false;

			//This is a starter weapon, so it is always useable as long as the player is of a specific class.
			if (Player.PlayerType == PlayerType.Knight || Player.PlayerType == PlayerType.Rogue) {
				Useable = true;
			} else {
				Useable = false;
			}
		}


		public override string ItemInfo () {
			return "A wooden sword. This could be used to fight...";
		}


	} //END OF WOODENSWORD CLASS





	//An arrow class for when the player chooses to be use ranged combat.
	public class StarterArrow : ArrowItem
	{
		protected override int Damage {
			get {
				return 10;
			}
		}

		public StarterArrow () {
			//Arrows can be stacked.
			Stackable = true;
			Type = ItemType.StarterArrow;
			Degradable = false;

			if (Player.PlayerType == PlayerType.Arrowman) {
				Useable = true;
			} else {
				Useable = false;
			}
		}


		public override string ItemInfo () {
			return "A basic arrow. I could shoot this using a bow.";
		}
	}




	public class MagicWand : WandItem {

		protected override int Damage {
			get {
				return 10;
			}
		}

		public MagicWand () {
			//Arrows can be stacked.
			Stackable = true;
			Type = ItemType.MagicWand;
			Degradable = false;

			if (Player.PlayerType == PlayerType.Wizard) {
				Useable = true;
			} else {
				Useable = false;
			}
		}


		public override string ItemInfo () {
			return "A magical wand!";
		}


	} //END OF MAGICWAND CLASS








} //END OF NAMESPACE

