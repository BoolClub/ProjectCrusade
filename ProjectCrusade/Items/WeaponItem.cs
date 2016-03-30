using System;
using System.Text;
using Microsoft.Xna.Framework;

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

		protected abstract float CoolDownTime { get; }
		private float coolDownRemaining = 0f;
		protected bool IsCooledDown { get { return coolDownRemaining >= CoolDownTime; } }

		//TODO: make this cleaner
		public override string Tooltip {
			get {
				return getDescriptions ();
			}
		}


		string getDescriptions()
		{
			string[] desc1 = {
				"",
				"Flaming: \n",
				"Healing: \n",
				"Destroying: \n",
				"Daring: \n",
				"Crying: \n",
				"Stealing: \n",
				"Beguiling: \n",
				"Stunning: \n",
				"Swarming: \n"
			};

			string[] desc2 = {
				"",
				"Youthful: \n",
				"Wild: \n",
				"Lucky: \n",
				"Weird: \n",
				"Poisoned: \n",
				"Icy: \n",
				"Brigands: \n",
				"Heavy: \n",
				"Old: \n",
				"Unlucky: \n",
				"Light: \n",
			};

			string[] desc3 = {
				"",
				"Uthman: \n",
				"the Fortunate: \n",
				"the Ages: \n",
				"the Griffon: \n",
				"Mages: \n",
				"Knights: \n",
				"Kingdoms: \n",
				"Mogar: \n",
				"the Night: \n"
			};
			return 
				desc1 [(int)TierOne] +
			desc2 [(int)TierTwo] +
			desc3 [(int)TierThree];
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

		public override void PrimaryUse (World world)
		{
			coolDownRemaining = 0f;
		}

		public override void Update(GameTime gameTime)
		{
			coolDownRemaining += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
		}

	} //END OF WEAPONITEM CLASS


} //END OF NAMESPACE

