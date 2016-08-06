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
			proj.projectileType = 8;
			world.AddEntity (proj);
		}
		public override void SecondaryUse (World world)
		{
			throw new NotImplementedException ();
		}
	}



	public class MagicWand : WandItem {

		public override ItemSprite Type 	  { get { return ItemSprite.MagicWand; } }
		public override bool Depletable 	  { get { return false; } }
		protected override int Damage 		  { get { return 10; } }
		public override string Tooltip 		  { get { return base.Tooltip + "A magical wand!"; } }
		protected override string BaseName 	  { get { return "Wand"; } }
		protected override float CoolDownTime { get { return 1000f; } }
		public MagicWand () {

		}


	} //END OF MAGICWAND CLASS


}

