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
	public const float ICE_SWORD_DAMAGE = 16;

	public float StoredHP = 0;
	public int FlameSwordCharge = 5;
	public float chargeTimeElectricSword = 700f;

	// Used for adding extra effects for certain items.
	public delegate void WeaponExtras(ItemType itemType, Enemy enem);
	public WeaponExtras PerformWeaponExtra;

	#region References For Simplicity

		public Healthbar HPBar = GameObject.Find("HPBarFill").GetComponent<Healthbar>();
		public PlayerControls Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
		public Inventory TheInventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();

		GameObject arrow = Resources.Load("Projectiles/Arrow") as GameObject;
		GameObject poisonarrow = Resources.Load("Projectiles/Poison Arrow") as GameObject;
		GameObject magicbolt = Resources.Load("Projectiles/Magic Bolt") as GameObject;
		GameObject magicbolt2 = Resources.Load("Projectiles/Magic Bolt2") as GameObject;
		GameObject magicbolt3 = Resources.Load("Projectiles/Magic Bolt3") as GameObject;
		GameObject fire = Resources.Load("Projectiles/Fire Bolt") as GameObject;
		GameObject iceshard = Resources.Load("Projectiles/Ice Shard") as GameObject;

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
		Quantity = 1;
		SetupItem();
	}
	public Item(ItemType type, int quantity)
	{
		Type = type;
		SetupItem();
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

	public void Remove()
	{
		Type = ItemType.EMPTY;
		Quantity = 0;
	}

	public void SetupItem()
	{
		PerformWeaponExtra = new WeaponExtras(CalculateWeaponExtras);

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
		if (Type == ItemType.IceSword)
		{
			Name = "Ice Sword";
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
		if (Type == ItemType.Meat)
		{
			Name = "Meat";
			Stackable = true;
		}
		if (Type == ItemType.PoisonArrow)
		{
			Name = "Poison Arrow";
			Stackable = true;
		}
		if (Type == ItemType.Staff)
		{
			Name = "Staff";
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
			Healthbar.AddHP(7f);
			Quantity--;
		}

		if (Type == ItemType.Arrow)
		{
			if (TheInventory.Contains(ItemType.BowAndArrow))
			{
				Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
				arrow.GetComponent<Projectile>().Launcher = Player.gameObject;
				MonoBehaviour.Instantiate(arrow, Player.transform.position, Quaternion.identity);
				Quantity--;
			}
		}

		if (Type == ItemType.BowAndArrow)
		{
			if (TheInventory.Contains(ItemType.Arrow))
			{
				Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
				arrow.GetComponent<Projectile>().Launcher = Player.gameObject;
				MonoBehaviour.Instantiate(arrow, Player.transform.position, Quaternion.identity);
				TheInventory.Find(ItemType.Arrow).Quantity--;
			}
		}

		if (Type == ItemType.Bread)
		{
			Healthbar.AddHP(15f);
			Quantity--;
		}

		if (Type == ItemType.CurvedSword)
		{
			HurtEnemy(CURVED_SWORD_DAMAGE, null);
		}

		if (Type == ItemType.ElectricSword)
		{
			HurtEnemy(ELECTRIC_SWORD_DAMAGE, PerformWeaponExtra);
		}

		if (Type == ItemType.FlamingSword)
		{
			HurtEnemy(FLAMING_SWORD_DAMAGE, PerformWeaponExtra);
		}

		if (Type == ItemType.HealingSword)
		{
			//Store one fourth of the damage you do
			StoredHP += HurtEnemy(HEALING_SWORD_DAMAGE, null) / 4;
		}

		if (Type == ItemType.IceSword)
		{
			HurtEnemy(ICE_SWORD_DAMAGE, PerformWeaponExtra);
		}

		if (Type == ItemType.IronSword)
		{
			HurtEnemy(IRON_SWORD_DAMAGE, null);
		}

		if (Type == ItemType.LongSword)
		{
			HurtEnemy(LONG_SWORD_DAMAGE, null);
		}

		if (Type == ItemType.Mace)
		{
			HurtEnemy(MACE_DAMAGE, null);
		}

		if (Type == ItemType.MagicWand)
		{
			Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
			magicbolt.GetComponent<Projectile>().Launcher = Player.gameObject;
			MonoBehaviour.Instantiate(magicbolt, Player.transform.position, Quaternion.identity);
		}

		if (Type == ItemType.Meat)
		{
			Healthbar.AddHP(25f);
			Quantity--;
		}

		if (Type == ItemType.PoisonArrow)
		{
			if (TheInventory.Contains(ItemType.BowAndArrow))
			{
				Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
				poisonarrow.GetComponent<Projectile>().Launcher = Player.gameObject;
				MonoBehaviour.Instantiate(poisonarrow, Player.transform.position, Quaternion.identity);
				Quantity--;
			}
		}

		if (Type == ItemType.Staff)
		{
			Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
			magicbolt3.GetComponent<Projectile>().Launcher = Player.gameObject;
			MonoBehaviour.Instantiate(magicbolt3, Player.transform.position, Quaternion.identity);
		}

		if (Type == ItemType.SteelSword)
		{
			HurtEnemy(STEEL_SWORD_DAMAGE, null);
		}

		if (Type == ItemType.Water)
		{
			Healthbar.AddHP(5f);
			Quantity--;
		}

		if (Type == ItemType.WoodenSword)
		{
			HurtEnemy(WOODEN_SWORD_DAMAGE, null);
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
				HurtEnemy(FLAMING_SWORD_DAMAGE, PerformWeaponExtra);

				//Shoot fire projectile
				Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
				fire.GetComponent<Projectile>().Launcher = Player.gameObject;
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
				Healthbar.Health += StoredHP;
				StoredHP = 0;
			}
			else {
				GameObject.FindWithTag("Label").GetComponent<TextLabel>().enabled = true;
				GameObject.FindWithTag("Label").GetComponent<TextLabel>().Text = "There is no energy stored in this item.";
			}
		}

		if (Type == ItemType.IceSword)
		{
			HurtEnemy(ICE_SWORD_DAMAGE, PerformWeaponExtra);

			//Shoot ice shard projectile
			Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
			iceshard.GetComponent<Projectile>().Launcher = Player.gameObject;
			MonoBehaviour.Instantiate(iceshard, Player.transform.position, Quaternion.identity);
		}

		if (Type == ItemType.IronSword) { }
		if (Type == ItemType.LongSword) { }
		if (Type == ItemType.Mace) { }

		if (Type == ItemType.MagicWand) 
		{
			Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
			magicbolt2.GetComponent<Projectile>().Launcher = Player.gameObject;
			MonoBehaviour.Instantiate(magicbolt2, GameObject.FindWithTag("Player").transform.position, Quaternion.identity);
		}

		if (Type == ItemType.Meat) { }

		if (Type == ItemType.PoisonArrow)
		{
			// None right now, but all arrows will have a secondary use later (shoot three arrows at a time).
		}

		if (Type == ItemType.Staff)
		{
			// This magic item will have a secondary use later on as well.
			// The player will be able to shoot magic bolts in all different directions just like the first boss.
			MonoBehaviour mb = GameObject.Find("GameManager").GetComponent<MonoBehaviour>();
			mb.StartCoroutine(MultiShotMagicAttack());
		}

		if (Type == ItemType.SteelSword) { }
		if (Type == ItemType.Water) { }
		if (Type == ItemType.WoodenSword) { }
	}




	float HurtEnemy(float Damage, WeaponExtras extra)
	{
		float damage = 0;
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			if (enemy.GetComponent<Enemy>().IsNextToPlayer)
			{
				damage = (float)Math.Round(new FloatRange(0, Damage).Random, 2);

				// Carry out the extra function of the item if it has one.
				if (extra != null)
				{
					extra(this.Type, enemy.GetComponent<Enemy>());
				}

				// Decrease enemy health and create damage label.
				enemy.GetComponent<Enemy>().DecreaseHealth(damage);
			}
		}
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Boss"))
		{
			if (enemy.GetComponent<Enemy>().IsNextToPlayer)
			{
				damage = (float)Math.Round(new FloatRange(0, Damage).Random, 2);

				// Carry out the extra function of the item if it has one.
				if (extra != null)
				{
					extra(this.Type, enemy.GetComponent<Enemy>());
				}

				// Decrease enemy health and create damage label.
				enemy.GetComponent<Enemy>().DecreaseHealth(damage);
			}
		}
		return damage;
	}
   	Enemy GetEnemy()
	{
		Enemy enm = null;
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			if (enemy.GetComponent<Enemy>().IsNextToPlayer)
			{
				enm = enemy.GetComponent<Enemy>();
			}
		}
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Boss"))
		{
			if (enemy.GetComponent<Enemy>().IsNextToPlayer)
			{
				enm = enemy.GetComponent<Enemy>();
			}
		}
		return enm;
	}



	private void CalculateWeaponExtras(ItemType itemType, Enemy enem)
	{
		if (itemType == ItemType.ElectricSword)
		{
			// Calculate the added effect of stunning the enemy
			float number = new FloatRange(0, 100).Random;
			bool willStunEnemy = (!(number % 2).Equals(0) && number < 20) ? true : false;

			// Chanec of stunning
			if (willStunEnemy == true)
			{
				if (enem != null)
				{
					enem.stunTime = 5f;
				}
			}
		}

		if (itemType == ItemType.FlamingSword)
		{
			// Chance of burning the nemey
			float number = new FloatRange(0, 100).Random;
			bool willBurnEnemy = (!(number % 2).Equals(0) && number < 20) ? true : false;

			if (willBurnEnemy == true)
			{
				if (enem != null)
				{
					enem.Burned = true;
				}
			}
		}

		if (itemType == ItemType.IceSword)
		{
			// Chance of freezing the nemey
			float number = new FloatRange(0, 100).Random;
			bool willFreezeEnemy = (!(number % 2).Equals(0) && number < 20) ? true : false;

			if (willFreezeEnemy == true)
			{
				if (enem != null)
				{
					enem.Frozen = true;
				}
			}
		}
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

	/* This is for the secondary use of the staff. It is the same as it is in the boss class, but fitted to
	suit the player. Shoots a magic bolt in all directions. */

	IEnumerator MultiShotMagicAttack()
	{
		Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
		magicbolt3.GetComponent<Projectile>().Launcher = Player.gameObject;

		Player.Direction = Direction.North;
		MonoBehaviour.Instantiate(magicbolt3, new Vector2(Player.transform.position.x, Player.transform.position.y + 1), Quaternion.identity);
		yield return new WaitForSeconds(0.001f);

		Player.Direction = Direction.NorthEast;
		MonoBehaviour.Instantiate(magicbolt3, new Vector2(Player.transform.position.x + 1, Player.transform.position.y + 1), Quaternion.identity);
		yield return new WaitForSeconds(0.001f);

		Player.Direction = Direction.East;
		MonoBehaviour.Instantiate(magicbolt3, new Vector2(Player.transform.position.x + 1, Player.transform.position.y), Quaternion.identity);
		yield return new WaitForSeconds(0.001f);

		Player.Direction = Direction.SouthEast;
		MonoBehaviour.Instantiate(magicbolt3, new Vector2(Player.transform.position.x + 1, Player.transform.position.y - 1), Quaternion.identity);
		yield return new WaitForSeconds(0.001f);

		Player.Direction = Direction.South;
		MonoBehaviour.Instantiate(magicbolt3, new Vector2(Player.transform.position.x, Player.transform.position.y - 1), Quaternion.identity);
		yield return new WaitForSeconds(0.001f);

		Player.Direction = Direction.SouthWest;
		MonoBehaviour.Instantiate(magicbolt3, new Vector2(Player.transform.position.x - 1, Player.transform.position.y - 1), Quaternion.identity);
		yield return new WaitForSeconds(0.001f);

		Player.Direction = Direction.West;
		MonoBehaviour.Instantiate(magicbolt3, new Vector2(Player.transform.position.x - 1, Player.transform.position.y), Quaternion.identity);
		yield return new WaitForSeconds(0.001f);

		Player.Direction = Direction.NorthWest;
		MonoBehaviour.Instantiate(magicbolt3, new Vector2(Player.transform.position.x - 1, Player.transform.position.y + 1), Quaternion.identity);
	}
}
