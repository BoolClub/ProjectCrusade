using System;
using Microsoft.Xna.Framework;

namespace ProjectCrusade
{
	public abstract class ArrowItem : WeaponItem
	{

		public override bool Depletable 		{ get { return true; 	} }
		public override bool Stackable 			{ get { return true; 	} }
		protected override bool Degradable 		{ get { return false; 	} }


		public ArrowItem ()
		{
			if (Player.PlayerType == PlayerType.Archer) {
				Useable = true;
			} else {
				Useable = false;
			}
		}

		//All arrows have same behavior
		public override void PrimaryUse (World world)
		{
			Projectile proj = new Projectile (
				world.Player.CollisionBox.Center.ToVector2(), 
				500f * world.Player.OrientationVector, 
				Damage);
			world.AddEntity (proj);
		}
		public override void SecondaryUse (World world)
		{
			throw new NotImplementedException ();
		}
	}


	//An arrow class for when the player chooses to be use ranged combat.
	public class StarterArrow : ArrowItem
	{
		public override ItemType Type 			{ get { return ItemType.StarterArrow; } }
		public override string Tooltip 		{ get { return "Can be shot."; } }
		protected override int Damage 			{ get { return 50; } }
		protected override string BaseName			{ get { return "Arrow"; }}
	}

}

