using System;

namespace ProjectCrusade
{
	public class WoodenSword : WeaponItem
	{
		public WoodenSword () {
			//Cannot stack a sword
			Stackable = false;

			//Item's type
			Type = ItemType.Wooden_Sword;

			//A wooden sword is not degradable
			Degradable = false;

			//This is a starter weapon, so it is always useable.
			Useable = true;
		}


		public override string ItemInfo () {
			return "A wooden sword. This could be used to fight...";
		}

	} //END OF WOODENSWORD CLASS





	//An arrow class for when the player chooses to be use ranged combat.
	public class StarterArrow : WeaponItem
	{
		public StarterArrow () {
			//Arrows can be stacked.
			Stackable = true;
			Type = ItemType.Starter_Arrow;
			Degradable = false;
			Useable = true;
		}


		public override string ItemInfo () {
			return "A basic arrow. I could shoot this using a bow.";
		}
	}



} //END OF NAMESPACE

