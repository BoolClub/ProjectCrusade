using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
			return 
				desc1 [(int)TierOne] +
			desc2 [(int)TierTwo];
		}

		public override string Name {
			get {
				string one, two;

				one = TierOne.ToString() + ' ';
				two = TierTwo.ToString() + ' ';

				if (TierOne == TierOneProperty.None)
					one = "";
				if (TierTwo == TierTwoProperty.None)
					two = "";


				one = one.Replace ('_', ' ');
				two = two.Replace ('_', ' ');

				return String.Format ("{0}{1}{2}", one, two, BaseName);
			}
		}

		public TierOneProperty TierOne;
		public TierTwoProperty TierTwo;


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

		public WeaponItem () {
			TierOne = TierOneProperty.None;
			TierTwo = TierTwoProperty.None;
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

		const int TieredPropertySpriteSheetWidth = 320;
		const int TieredPropertySpriteSheetHeight = 10;

		private Rectangle getTierOneSourceRect(TierOneProperty prop)
		{
			int i = (int)prop - 1;

			if (i * TieredPropertySpriteSheetHeight > TieredPropertySpriteSheetWidth)
				throw new Exception ("Property not found on sprite sheet!");

			return new Rectangle (i * TieredPropertySpriteSheetHeight, 0, TieredPropertySpriteSheetHeight, TieredPropertySpriteSheetHeight);
		}

		private Rectangle getTierTwoSourceRect(TierTwoProperty prop)
		{
			
			int i = (int)prop
				+ Enum.GetNames(typeof(TierOneProperty)).Length
				- 1;

			if (i * TieredPropertySpriteSheetHeight > TieredPropertySpriteSheetWidth)
				throw new Exception ("Property not found on sprite sheet!");

			return new Rectangle (i * TieredPropertySpriteSheetHeight, 0, TieredPropertySpriteSheetHeight, TieredPropertySpriteSheetHeight);
		}

		public override void Draw (Rectangle rect, SpriteBatch spriteBatch, TextureManager textureManager)
		{
			base.Draw (rect, spriteBatch, textureManager);

			Rectangle prop1Rect = new Rectangle (
				rect.X, rect.Y, TieredPropertySpriteSheetHeight, TieredPropertySpriteSheetHeight);
			Rectangle prop2Rect = new Rectangle (
				rect.X, rect.Y + TieredPropertySpriteSheetHeight + 1, TieredPropertySpriteSheetHeight, TieredPropertySpriteSheetHeight);
			
			if (TierOne!=TierOneProperty.None)
				spriteBatch.Draw (
					textureManager.GetTexture ("tiered-properties"),
					null, 
					prop1Rect, 
					getTierOneSourceRect(TierOne), 
					null, 
					0, 
					null, 
					null, 
					SpriteEffects.None, 
					0);

			if (TierTwo!=TierTwoProperty.None)
				spriteBatch.Draw (
					textureManager.GetTexture ("tiered-properties"),
					null, 
					prop2Rect, 
					getTierTwoSourceRect(TierTwo), 
					null, 
					0, 
					null, 
					null, 
					SpriteEffects.None, 
					0);
		}
	}
}

