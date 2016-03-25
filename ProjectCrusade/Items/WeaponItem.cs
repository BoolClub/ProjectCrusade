using System;

namespace ProjectCrusade
{
	public abstract class WeaponItem : Item
	{
		//Boolean for whether or not the weapon can degrade after using it too much.
		protected abstract bool Degradable { get; }

		//Integer for level of degrading, starting from 100 down to 0.
		protected int DegradeLevel { get; set; }

		//Integer for the amount to degrade by each time.
		private const int degradeAmount = 10;

		//Boolean for whether or not the player is capable of using the weapon (based on level or class).
		protected bool Useable { get; set; }

		protected abstract int Damage { get; }

		protected abstract string BaseName { get; }

		public override string Name {
			get {
				string one, two, three;

				one = TierOne.ToString() + ' ';
				two = TierTwo.ToString() + ' ';
				three = TierThree.ToString ();

				if (TierOne == TierOneProperty.None)
					one = "";
				if (TierTwo == TierTwoProperty.None)
					two = "";
				if (TierThree == TierThreeProperty.None)
					three = "";


				one = one.Replace ('_', ' ');
				two = two.Replace ('_', ' ');
				three = three.Replace ('_', ' ');

				return String.Format ("{0}{1}{2} of {3}", one, two, BaseName, three);
			}
		}

		public TierOneProperty TierOne;
		public TierTwoProperty TierTwo;
		public TierThreeProperty TierThree;


		public enum TierOneProperty { 
			None = 0,
			Flaming, 
			Healing, 
			Destroying, 
			Daring, 
			Crying, 
			Stealing, 
			Beguiling, 
			Stunning, 
			Swarming
		}

		public enum TierTwoProperty
		{
			None = 0,
			Youthful,
			Wild,
			Lucky,
			Weird,
			Poisoned,
			Icy,
			Brigands,
			Heavy,
			Old,
			Unlucky,
			Light,
		}

		public enum TierThreeProperty {
			None = 0,
			Uthman,
			the_Fortunate,
			the_Ages,
			the_Griffon,
			Mages,
			Knights,
			Kingdoms,
			Mogar,
			the_Night,
		}

		public WeaponItem () {
			TierOne = TierOneProperty.None;
			TierTwo = TierTwoProperty.None;
			TierThree = TierThreeProperty.None;
		}


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

