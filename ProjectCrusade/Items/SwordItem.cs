using System;

namespace ProjectCrusade
{
	public abstract class SwordItem : WeaponItem
	{
		public override bool Stackable 			{ get { return false; 	} }
		public override bool Depletable 		{ get { return false; 	} }
		protected override bool Degradable		{ get { return true;  	} }

		public SwordItem ()
		{
			//It is useable as long as the player is of a specific class.
			if (Player.PlayerType == PlayerType.Knight || Player.PlayerType == PlayerType.Rogue) {
				Useable = true;
			} else {
				Useable = false;
			}
		}

		//All swords have same behavior
		public override void PrimaryUse (Player player, World world)
		{
			throw new NotImplementedException ();
		}
		public override void SecondaryUse (Player player, World world)
		{
			throw new NotImplementedException ();
		}
	}

	public class WoodenSword : SwordItem
	{

		public override ItemType Type 			{ get { return ItemType.WoodenSword; } }
		public override string ItemInfo 		{ get { return "A wooden sword. This could be used to fight..."; } }
		protected override int Damage 			{ get { return 10; } }


		public WoodenSword () {	}
	} 

	public class StoneSword : SwordItem
	{

		public override ItemType Type 			{ get { return ItemType.StoneSword; } }
		public override string ItemInfo 		{ get { return "A stone sword. This could be used to fight..."; } }
		protected override int Damage 			{ get { return 20; } }

		public StoneSword () { }
	} 

	public class IronSword : SwordItem
	{

		public override ItemType Type 			{ get { return ItemType.IronSword; } }
		public override string ItemInfo 		{ get { return "An iron sword. This could be used to fight..."; } }
		protected override int Damage 			{ get { return 30; } }

		public IronSword () { }
	} 


}

