using System;

namespace ProjectCrusade
{
	public abstract class WandItem : WeaponItem
	{
		public WandItem ()
		{
			//Can stack arrows
			Stackable = false;
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
}

