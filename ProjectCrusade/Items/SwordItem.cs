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
		public override void PrimaryUse (World world)
		{
			foreach (Entity e in world.activeEntities) {
				if (e is Enemy && e.CollisionBox.Intersects(world.Player.InteractionBox))
					(e as Enemy).RemoveHealth (Damage);
			}
		}
		public override void SecondaryUse (World world)
		{
			throw new NotImplementedException ();
		}
	}

	public class WoodenSword : SwordItem
	{

		public override ItemType Type 			{ get { return ItemType.WoodenSword; } }
		public override string Tooltip 		{ get { return "A wooden sword. This could be used to fight..."; } }
		protected override int Damage 			{ get { return 80; } }
		public override string Name 	{ get { return "wooden sword"; } }
	} 

	public class StoneSword : SwordItem
	{

		public override ItemType Type 			{ get { return ItemType.StoneSword; } }
		public override string Tooltip 		{ get { return "A stone sword. This could be used to fight..."; } }
		protected override int Damage 			{ get { return 100; } }
		public override string Name 	{ get { return "stone sword"; } }
	} 

	public class IronSword : SwordItem
	{

		public override ItemType Type 			{ get { return ItemType.IronSword; } }
		public override string Tooltip 		{ get { return "An iron sword. This could be used to fight..."; } }
		protected override int Damage 			{ get { return 120; } }
		public override string Name 	{ get { return "iron sword"; } }
	} 


}

