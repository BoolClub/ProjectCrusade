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
		public override void PrimaryUse (Player player)
		{
			throw new NotImplementedException ();
		}
		public override void SecondaryUse (Player player)
		{
			throw new NotImplementedException ();
		}
	}
}

