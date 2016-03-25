using System;
using System.Text;

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

		//TODO: make this cleaner
		public override string Tooltip {
			get {
				StringBuilder build = new StringBuilder ();

				switch (TierOne) {
				case TierOneProperty.Flaming:
					build.AppendLine ("Flaming: ");
					break;
				case TierOneProperty.Healing:
					build.AppendLine ("Healing: ");
					break;
				case TierOneProperty.Destroying:
					build.AppendLine ("Destroying: ");
					break;
				case TierOneProperty.Daring:
					build.AppendLine ("Daring: ");
					break;
				case TierOneProperty.Crying:
					build.AppendLine ("Crying: ");
					break;
				case TierOneProperty.Stealing:
					build.AppendLine ("Stealing: ");
					break;
				case TierOneProperty.Beguiling:
					build.AppendLine ("Beguiling: ");
					break;
				case TierOneProperty.Stunning:
					build.AppendLine ("Stunning: ");
					break;
				case TierOneProperty.Swarming:
					build.AppendLine ("Swarming: ");
					break;
				}
				switch (TierTwo) {
				case TierTwoProperty.Youthful:
					build.AppendLine ("Youthful: ");
					break;
				case TierTwoProperty.Wild:
					build.AppendLine ("Wild: ");
					break;
				case TierTwoProperty.Lucky:
					build.AppendLine ("Lucky: ");
					break;
				case TierTwoProperty.Weird:
					build.AppendLine ("Weird: ");
					break;
				case TierTwoProperty.Poisoned:
					build.AppendLine ("Poisoned: ");
					break;
				case TierTwoProperty.Icy:
					build.AppendLine ("Icy: ");
					break;
				case TierTwoProperty.Brigands:
					build.AppendLine ("Brigands: ");
					break;
				case TierTwoProperty.Heavy:
					build.AppendLine ("Heavy: ");
					break;
				case TierTwoProperty.Old:
					build.AppendLine ("Old: ");
					break;
				case TierTwoProperty.Unlucky:
					build.AppendLine ("Unlucky: ");
					break;
				case TierTwoProperty.Light:
					build.AppendLine ("Light: ");
					break;
				}
				switch (TierThree) {
				case TierThreeProperty.Uthman:
					build.AppendLine ("Uthman: ");
					break;
				case TierThreeProperty.the_Fortunate:
					build.AppendLine ("the Fortunate: ");
					break;
				case TierThreeProperty.the_Ages:
					build.AppendLine ("the Ages: ");
					break;
				case TierThreeProperty.the_Griffon:
					build.AppendLine ("the Griffon: ");
					break;
				case TierThreeProperty.Mages:
					build.AppendLine ("Mages: ");
					break;
				case TierThreeProperty.Knights:
					build.AppendLine ("Knights: ");
					break;
				case TierThreeProperty.Kingdoms:
					build.AppendLine ("Kingdoms: ");
					break;
				case TierThreeProperty.Mogar:
					build.AppendLine ("Mogar: ");
					break;
				case TierThreeProperty.the_Night:
					build.AppendLine ("the Night: ");
					break;
				}
				return build.ToString ();
			}
		}

		public override string Name {
			get {
				string one, two, three;

				one = TierOne.ToString() + ' ';
				two = TierTwo.ToString() + ' ';
				three = " of " + TierThree.ToString ();

				if (TierOne == TierOneProperty.None)
					one = "";
				if (TierTwo == TierTwoProperty.None)
					two = "";
				if (TierThree == TierThreeProperty.None)
					three = "";


				one = one.Replace ('_', ' ');
				two = two.Replace ('_', ' ');
				three = three.Replace ('_', ' ');

				return String.Format ("{0}{1}{2}{3}", one, two, BaseName, three);
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

