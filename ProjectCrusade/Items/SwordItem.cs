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

	public class Sword : SwordItem
	{

		public override ItemType Type 			{ get { return ItemType.Sword; } }
		public override string Tooltip 		{ get { return base.Tooltip + "A wooden sword. This could be used to fight..."; } }
		protected override int Damage 			{ get { return 80; } }
		protected override string BaseName 	{ get { return "Sword"; } }
	} 


}

