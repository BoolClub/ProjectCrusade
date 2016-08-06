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
			Useable = true;
		}

		//All swords have same behavior
		public override void PrimaryUse (World world)
		{
			if (!IsCooledDown)
				return; 
			foreach (Entity e in world.activeEntities) {
				if (e is Enemy && e.CollisionBox.Intersects(world.Player.InteractionBox))
					(e as Enemy).RemoveHealth (Damage);
			}
			base.PrimaryUse (world);
		}
		public override void SecondaryUse (World world)
		{
			throw new NotImplementedException ();
		}
	}

	public class Sword : SwordItem
	{

		public override ItemSprite Type 			{ get { return ItemSprite.WoodenSword; } }
		public override string Tooltip 		{ get { return base.Tooltip + "A wooden sword. This could be used to fight..."; } }
		protected override int Damage 			{ get { return 80; } }
		protected override string BaseName 	{ get { return "Sword"; } }
		protected override float CoolDownTime { get { return 1000f; } }
	} 


}

