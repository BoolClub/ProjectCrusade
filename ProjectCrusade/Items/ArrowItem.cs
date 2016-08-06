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
			Useable = true;
		}

		//All arrows have same behavior
		public override void PrimaryUse (World world)
		{
			if (!IsCooledDown)
				return;
			Projectile proj = new Projectile (
				world.Player.CollisionBox.Center.ToVector2(), 
				500f * world.Player.OrientationVector, 
				Damage);
			proj.projectileType = 7;
			world.AddEntity (proj);

			base.PrimaryUse (world);
		}
		public override void SecondaryUse (World world)
		{
			throw new NotImplementedException ();
		}
	}


	//An arrow class for when the player chooses to be use ranged combat.
	public class StarterArrow : ArrowItem
	{
		public override ItemSprite Type 			{ get { return ItemSprite.BowArrow; } 	}
		public override string Tooltip 		{ get { return base.Tooltip + "Can be shot."; } }
		protected override int Damage 			{ get { return 50; } }
		protected override string BaseName			{ get { return "Bow"; }}
		protected override float CoolDownTime { get { return 100f; } }
	}

}

