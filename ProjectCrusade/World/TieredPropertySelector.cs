using System;
using System.Xml;

namespace ProjectCrusade
{
	public class TieredPropertySelector
	{
		double[] probabilitiesOne;
		double[] probabilitiesOneAccumulated;
		double[] probabilitiesTwo;
		double[] probabilitiesTwoAccumulated;
		double[] probabilitiesThree;
		double[] probabilitiesThreeAccumulated;



		public TieredPropertySelector (string filename)
		{
			int numOne = Enum.GetNames (typeof(WeaponItem.TierOneProperty)).Length-1;//discount None
			int numTwo = Enum.GetNames (typeof(WeaponItem.TierTwoProperty)).Length-1;//discount None
			int numThree = Enum.GetNames (typeof(WeaponItem.TierThreeProperty)).Length-1;//discount None

			probabilitiesOne = new double[numOne];
			probabilitiesTwo = new double[numTwo];
			probabilitiesThree = new double[numThree];


			XmlReader reader = XmlReader.Create (filename);

			XmlDocument doc = new XmlDocument ();
			doc.Load (reader);

			foreach (XmlElement elem in doc.SelectNodes("tiers/tier")) {
				Type t;

				double[] currProb;

				int val = int.Parse (elem.GetAttribute ("num"));
				switch (val) {
				case 1:
					t = typeof(WeaponItem.TierOneProperty);
					currProb = probabilitiesOne;
					break;
				case 2:
					t = typeof(WeaponItem.TierTwoProperty);
					currProb = probabilitiesTwo;
					break;
				case 3:
					t = typeof(WeaponItem.TierThreeProperty);
					currProb = probabilitiesThree;
					break;
				default: 
					throw new Exception ("This tier value is not covered.");
				}

				foreach (XmlElement prop in elem.ChildNodes) {
					var e = Enum.Parse (t, prop.Name);
					double probability = double.Parse (prop.InnerText);

					currProb [(int)e - 1] = probability;
				}

			}
			accumulateProbabilities (probabilitiesOne, ref probabilitiesOneAccumulated);
			accumulateProbabilities (probabilitiesTwo, ref probabilitiesTwoAccumulated);
			accumulateProbabilities (probabilitiesThree, ref probabilitiesThreeAccumulated);
		}


		void accumulateProbabilities ( double[] probabilities, ref double[] probabilitiesAccumulated)
		{
			probabilitiesAccumulated = new double[probabilities.Length];
			double acc = 0;
			for (int i = 0; i < probabilities.Length; i++) {
				probabilitiesAccumulated [i] = acc;
				acc += probabilities [i];
				if (acc > 1)
					throw new Exception ("Probabilties add to more than 1!");
			}
		}
		/// <summary>
		/// Returns the integer corresponding to a random property
		/// </summary>
		int randomProperty(Random rand, double[] probabilitiesAccumulated) { 
			double val = rand.NextDouble ();

			for (int i = 0; i < probabilitiesAccumulated.Length; i++) {
				if (probabilitiesAccumulated [i] > val)
					return i+1;
			}
			return -1;
		}

		public WeaponItem.TierOneProperty RandomPropertyOne(Random rand)
		{
			int p = randomProperty (rand, probabilitiesOneAccumulated);
			if (p > 0)
				return (WeaponItem.TierOneProperty)p;
			else
				return WeaponItem.TierOneProperty.None;
		}

		public WeaponItem.TierTwoProperty RandomPropertyTwo(Random rand)
		{
			int p = randomProperty (rand, probabilitiesTwoAccumulated);
			if (p > 0)
				return (WeaponItem.TierTwoProperty)p;
			else
				return WeaponItem.TierTwoProperty.None;
		}

		public WeaponItem.TierThreeProperty RandomPropertyThree(Random rand)
		{
			int p = randomProperty (rand, probabilitiesThreeAccumulated);
			if (p > 0)
				return (WeaponItem.TierThreeProperty)p;
			else
				return WeaponItem.TierThreeProperty.None;
		}
	}
}

