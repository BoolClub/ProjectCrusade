using System;

namespace ProjectCrusade
{
	public abstract class WeaponItem : Item
	{
		//Boolean for whether or not the weapon can degrade after using it too much.
		protected bool Degradable { get; set; }

		//Integer for level of degrading, starting from 100 down to 0.
		protected int DegradeLevel { get; set; }

		//Integer for the amount to degrade by each time.
		private const int degradeAmount = 10;

		//Boolean for whether or not the player is capable of using the weapon (based on level or class).
		protected bool Useable { get; set; }

		protected abstract int Damage { get; }


		public WeaponItem () {}


		//Degrade the item every time it is used.
		public void Degrade() {
			if (DegradeLevel - degradeAmount >= 0) {
				DegradeLevel -= degradeAmount;
			} else {
				DegradeLevel = 0;
			}
		}




	} //END OF WEAPONITEM CLASS


} //END OF NAMESPACE

