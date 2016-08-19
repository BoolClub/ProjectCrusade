using UnityEngine;
using System;
using System.Collections;

public class Item {
	public const float WOODEN_SWORD_DAMAGE = 8;
	public const float MACE_DAMAGE = 13;
	public const float CURVED_SWORD_DAMAGE = 10;
	public const float LONG_SWORD_DAMAGE = 7;
	public const float IRON_SWORD_DAMAGE = 11;
	public const float STEEL_SWORD_DAMAGE = 12;
	public const float FLAMING_SWORD_DAMAGE = 20;
	public const float HEALING_SWORD_DAMAGE = 15;
	public const float ELECTRIC_SWORD_DAMAGE = 17;

	public float StoredHP = 0;
	public int FlameSwordCharge = 5;
	public float chargeTimeElectricSword = 700f;

	#region References For Simplicity

		public Healthbar HPBar = GameObject.Find("HPBarFill").GetComponent<Healthbar>();
		public PlayerControls Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
		public Inventory TheInventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();

		GameObject arrow = Resources.Load("Projectiles/Arrow") as GameObject;
		GameObject magicbolt = Resources.Load("Projectiles/Magic Bolt") as GameObject;
		GameObject magicbolt2 = Resources.Load("Projectiles/Magic Bolt2") as GameObject;
		GameObject fire = Resources.Load("Projectiles/Fire Bolt") as GameObject;	

	#endregion


	/// <summary>
	/// The item type.
	/// </summary>
	public ItemType Type;

	/// <summary>
	/// Whether or not it is stackable.
	/// </summary>
	public bool Stackable;

	/// <summary>
	/// The quantity.
	/// </summary>
	public int Quantity = 1;

	/// <summary>
	/// The name.
	/// </summary>
	public string Name;



	public Item(ItemType type)
	{
		Type = type;
		DetermineNameAndStackable();
	}
	public Item(ItemType type, int quantity)
	{
		Type = type;
		DetermineNameAndStackable();
		if (Stackable == true)
		{
			Quantity = quantity;
		}
		else {
			Quantity = 1;
		}
	}


	// Adds itm to this item if they are stackable and of the same type. If not, it just sets the item.
	public void Add(Item itm)
	{
		if (Type == itm.Type)
		{
			Quantity += itm.Quantity;
		}
		else {
			Type = itm.Type;
			Stackable = itm.Stackable;
			Quantity = itm.Quantity;
			Name = itm.Name;
		}
	}


	public void DetermineNameAndStackable()
	{
		if (Type == ItemType.Apple)
		{
			Name = "Apple";
			Stackable = true;
		}
		if (Type == ItemType.Arrow)
		{
			Name = "Arrow";
			Stackable = true;
		}
		if (Type == ItemType.BowAndArrow)
		{
			Name = "Bow";
			Stackable = false;
		}
		if (Type == ItemType.Bread)
		{
			Name = "Bread";
			Stackable = true;
		}
		if (Type == ItemType.CurvedSword)
		{
			Name = "Curved Sword";
			Stackable = false;
		}
		if (Type == ItemType.ElectricSword)
		{
			Name = "Electric Sword";
			Stackable = false;
		}
		if (Type == ItemType.FlamingSword)
		{
			Name = "Flaming Sword";
			Stackable = false;
		}
		if (Type == ItemType.HealingSword)
		{
			Name = "Healing Sword";
			Stackable = false;
		}
		if (Type == ItemType.IronSword)
		{
			Name = "Iron Sword";
			Stackable = false;
		}
		if (Type == ItemType.LongSword)
		{
			Name = "Long Sword";
			Stackable = false;
		}
		if (Type == ItemType.Mace)
		{
			Name = "Mace";
			Stackable = false;
		}
		if (Type == ItemType.MagicWand)
		{
			Name = "Magic Wand";
			Stackable = false;
		}
		if (Type == ItemType.SteelSword)
		{
			Name = "Steel Sword";
			Stackable = false;
		}
		if (Type == ItemType.Water)
		{
			Name = "Water";
			Stackable = true;
		}
		if (Type == ItemType.WoodenSword)
		{
			Name = "Wooden Sword";
			Stackable = false;
		}
	}

	/// <summary>
	/// PRIMARY USE OF ITEMS.
	/// </summary>
	/// <returns>The use.</returns>
	public void PrimaryUse()
	{
		if (Type == ItemType.Apple)
		{
			HPBar = GameObject.Find("HPBarFill").GetComponent<Healthbar>();
			if (HPBar.Health < 100f)
			{
				HPBar.Health += 7f;
			}
			Quantity--;
		}

		if (Type == ItemType.Arrow)
		{
			if (TheInventory.Contains(ItemType.BowAndArrow))
			{
				Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
				MonoBehaviour.Instantiate(arrow, Player.transform.position, Quaternion.identity);
				Quantity--;
			}
		}

		if (Type == ItemType.BowAndArrow)
		{
			if (TheInventory.Contains(ItemType.Arrow))
			{
				Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
				MonoBehaviour.Instantiate(arrow, Player.transform.position, Quaternion.identity);
				TheInventory.Find(ItemType.Arrow).Quantity--;
			}
		}

		if (Type == ItemType.Bread)
		{
			HPBar = GameObject.Find("HPBarFill").GetComponent<Healthbar>();
			if (HPBar.Health < 100f)
			{
				HPBar.Health += 15f;
			}
			Quantity--;
		}

		if (Type == ItemType.CurvedSword)
		{
			HurtEnemy(CURVED_SWORD_DAMAGE);
		}

		if (Type == ItemType.ElectricSword)
		{
			float number = new FloatRange(0, 100).Random;
			bool willStunEnemy = (!(number % 2).Equals(0) && number < 20) ? true : false;

			// Chanec of stunning
			if (willStunEnemy == true)
			{
				Enemy enem = HurtEnemy(ELECTRIC_SWORD_DAMAGE, false, true);
				if (enem != null)
				{
					enem.stunTime = 5f;
				}
			}
			else {
				HurtEnemy(ELECTRIC_SWORD_DAMAGE, false, false);
			}
		}

		if (Type == ItemType.FlamingSword)
		{
			float number = new FloatRange(0, 100).Random;
			bool willBurnEnemy = (!(number % 2).Equals(0) && number < 20) ? true : false;

			// Chance of burning
			if (willBurnEnemy == true)
				HurtEnemy(FLAMING_SWORD_DAMAGE, true, false);
			else
				HurtEnemy(FLAMING_SWORD_DAMAGE, false, false);
		}

		if (Type == ItemType.HealingSword)
		{
			//Store one fourth of the damage you do
			StoredHP += HurtEnemy(HEALING_SWORD_DAMAGE) / 4;
		}

		if (Type == ItemType.IronSword)
		{
			HurtEnemy(IRON_SWORD_DAMAGE);
		}

		if (Type == ItemType.LongSword)
		{
			HurtEnemy(LONG_SWORD_DAMAGE);
		}

		if (Type == ItemType.Mace)
		{
			HurtEnemy(MACE_DAMAGE);
		}

		if (Type == ItemType.MagicWand)
		{
			Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
			MonoBehaviour.Instantiate(magicbolt, Player.transform.position, Quaternion.identity);
		}

		if (Type == ItemType.SteelSword)
		{
			HurtEnemy(STEEL_SWORD_DAMAGE);
		}

		if (Type == ItemType.Water)
		{
			HPBar = GameObject.Find("HPBarFill").GetComponent<Healthbar>();
			if (HPBar.Health < 100f)
			{
				HPBar.Health += 5f;
			}
			Quantity--;
		}

		if (Type == ItemType.WoodenSword)
		{
			HurtEnemy(WOODEN_SWORD_DAMAGE);
		}
	}


	/// <summary>
	/// SECONDARY USE OF ITEMS
	/// </summary>
	/// <returns>The use.</returns>
	public void SecondaryUse()
	{
		if (Type == ItemType.Apple) { }
		if (Type == ItemType.Arrow) { }
		if (Type == ItemType.BowAndArrow) { }
		if (Type == ItemType.Bread) { }
		if (Type == ItemType.CurvedSword) { }

		if (Type == ItemType.ElectricSword) 
		{
			if (chargeTimeElectricSword.Equals(700f))
			{
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
				{
					// Secondary use of the electric sword stuns all enemies on the map
					Enemy en = enemy.GetComponent<Enemy>();
					en.Stunned = true;
					en.stunTime = 15f;
				}
				chargeTimeElectricSword = 0;
			}
			else {
				GameObject.FindWithTag("Label").GetComponent<TextLabel>().enabled = true;
				GameObject.FindWithTag("Label").GetComponent<TextLabel>().Text = "This item needs time to recharge.";
			}
		}

		if (Type == ItemType.FlamingSword) 
		{
			if (FlameSwordCharge > 0)
			{
				float number = new FloatRange(0, 100).Random;
				bool willBurnEnemy = (!(number % 2).Equals(0) && number < 20) ? true : false;

				// Chance of burning
				if (willBurnEnemy == true)
					HurtEnemy(FLAMING_SWORD_DAMAGE, true, false);
				else
					HurtEnemy(FLAMING_SWORD_DAMAGE, false, false);
				

				//Shoot fire projectile
				Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
				MonoBehaviour.Instantiate(fire, Player.transform.position, Quaternion.identity);

				FlameSwordCharge--;
			}
			else {
				GameObject.FindWithTag("Label").GetComponent<TextLabel>().enabled = true;
				GameObject.FindWithTag("Label").GetComponent<TextLabel>().Text = "This item needs time to recharge.";
			}
		}

		if (Type == ItemType.HealingSword) 
		{
			if (StoredHP > 0)
			{
				HPBar = GameObject.Find("HPBarFill").GetComponent<Healthbar>();
				HPBar.Health += StoredHP;
				StoredHP = 0;
			}
			else {
				GameObject.FindWithTag("Label").GetComponent<TextLabel>().enabled = true;
				GameObject.FindWithTag("Label").GetComponent<TextLabel>().Text = "There is no energy stored in this item.";
			}
		}

		if (Type == ItemType.IronSword) { }
		if (Type == ItemType.LongSword) { }
		if (Type == ItemType.Mace) { }

		if (Type == ItemType.MagicWand) 
		{
			Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
			MonoBehaviour.Instantiate(magicbolt2, GameObject.FindWithTag("Player").transform.position, Quaternion.identity);
		}

		if (Type == ItemType.SteelSword) { }
		if (Type == ItemType.Water) { }
		if (Type == ItemType.WoodenSword) { }
	}




	float HurtEnemy(float Damage)
	{
		float damage = 0;
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			if (enemy.GetComponent<Enemy>().IsNextToPlayer)
			{
				damage = (float)Math.Round(new FloatRange(0, Damage).Random, 2);

				InstantiateDamageLabel(enemy, damage);

				// Decrease enemy health and create damage label.
				enemy.GetComponent<Enemy>().DecreaseHealth(damage);
			}
		}
		return damage;
	}
	Enemy HurtEnemy(float Damage, bool Burn, bool Stun)
	{
		Enemy enm = null;
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			if (enemy.GetComponent<Enemy>().IsNextToPlayer)
			{
				float damage = (float)Math.Round(new FloatRange(0, Damage).Random, 2);

				InstantiateDamageLabel(enemy, damage);

				// Decrease enemy health and create damage label.
				enm = enemy.GetComponent<Enemy>();
				enm.DecreaseHealth(damage);

				if (Burn == true)
				{
					enm.Burned = true;
				}
				if (Stun == true)
				{
					enm.Stunned = true;
				}
			}
		}
		return enm;
	}

	public void InstantiateDamageLabel(GameObject onTopOff, float damage)
	{
		GameObject damagelabel = Resources.Load("DamageLabel") as GameObject;
		damagelabel.GetComponent<DamageLabel>().Text = "" + damage;
		MonoBehaviour.Instantiate(damagelabel.gameObject,
					new Vector3(onTopOff.transform.position.x, onTopOff.transform.position.y, -2),
					Quaternion.identity);
	}


	public override string ToString()
	{
		return "Name: " + Name + ", Type: " + Type.ToString() + "Quantity: " + Quantity + ", Stackable: " + Stackable;
	}
}
