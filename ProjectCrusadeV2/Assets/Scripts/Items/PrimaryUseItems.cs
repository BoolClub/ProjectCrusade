using System;
using UnityEngine;

/// <summary>
/// The purpose of this class is to serve as a way for the player to use items. Just call the methods for primary
/// and secondary use of the specified item type.
/// </summary>
public class PrimaryUseItems : MonoBehaviour
{
	public const float WOODEN_SWORD_DAMAGE = 8;
	public const float MACE_DAMAGE = 13;
	public const float CURVED_SWORD_DAMAGE = 10;
	public const float LONG_SWORD_DAMAGE = 7;
	public const float IRON_SWORD_DAMAGE = 11;
	public const float STEEL_SWORD_DAMAGE = 12;
	public const float FLAMING_SWORD_DAMAGE = 20;
	public const float HEALING_SWORD_DAMAGE = 15;
	public const float ELECTRIC_SWORD_DAMAGE = 17;

	// The hp that is stored by the healing sword.
	public static float StoredHP = 0;

	// The amount of charge on swords
	public static int FlameSwordCharge = 5;



	/// <summary>
	/// Primary use of the specified item.
	/// </summary>
	/// <returns>The use.</returns>
	/// <param name="type">Type.</param>
	public static void PrimaryUse(ItemType type)
	{
		// APPLE
		if (type == ItemType.Apple)
		{
			if (GameObject.Find("HPBarFill").GetComponent<Healthbar>().Health < 100f)
			{
				GameObject.Find("HPBarFill").GetComponent<Healthbar>().Health += 7f;
			}
			GameObject.Find("Inventory").GetComponent<Inventory>().CurrentSlot.GetComponent<InventorySlot>().Type = ItemType.EMPTY;
		}

		// COIN
		if (type == ItemType.Coin)
		{
			// trying to use a coin will not do anything
		}

		// WOODEN SWORD
		if (type == ItemType.WoodenSword)
		{
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			{
				if (GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>().IsTouching(enemy.GetComponent<BoxCollider2D>()))
				{
					float damage = (float)Math.Round(new FloatRange(0, WOODEN_SWORD_DAMAGE).Random, 2);

					// ANIMATE THE SWORD SWING

					// Decrease enemy health and create damage label.
					enemy.GetComponent<Enemy>().DecreaseHealth(damage);
				}
			}
		}

		// BOW AND ARROW
		if (type == ItemType.BowAndArrow)
		{
			if (GameObject.Find("Inventory").GetComponent<Inventory>().Contains(ItemType.Arrow))
			{
				GameObject arrow = Resources.Load("Projectiles/Arrow") as GameObject;
				Instantiate(arrow, GameObject.FindWithTag("Player").transform.position, Quaternion.identity);
			}
		}

		// MAGIC WAND
		if (type == ItemType.MagicWand)
		{
			GameObject magicbolt = Resources.Load("Projectiles/Magic Bolt") as GameObject;
			Instantiate(magicbolt, GameObject.FindWithTag("Player").transform.position, Quaternion.identity);
		}

		// MACE
		if (type == ItemType.Mace)
		{
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			{
				if (GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>().IsTouching(enemy.GetComponent<BoxCollider2D>()))
				{
					float damage = (float)Math.Round(new FloatRange(0, MACE_DAMAGE).Random, 2);

					// ANIMATE THE SWORD SWING

					// Decrease enemy health and create damage label.
					enemy.GetComponent<Enemy>().DecreaseHealth(damage);
				}
			}
		}

		// CURVED SWORD
		if (type == ItemType.CurvedSword)
		{
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			{
				if (GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>().IsTouching(enemy.GetComponent<BoxCollider2D>()))
				{
					float damage = (float)Math.Round(new FloatRange(0, CURVED_SWORD_DAMAGE).Random, 2);

					// ANIMATE THE SWORD SWING

					// Decrease enemy health and create damage label.
					enemy.GetComponent<Enemy>().DecreaseHealth(damage);
				}
			}
		}

		// ARROW
		if (type == ItemType.Arrow)
		{
			if (GameObject.Find("Inventory").GetComponent<Inventory>().Contains(ItemType.BowAndArrow))
			{
				GameObject arrow = Resources.Load("Projectiles/Arrow") as GameObject;
				Instantiate(arrow, GameObject.FindWithTag("Player").transform.position, Quaternion.identity);
			}
		}

		// BREAD
		if (type == ItemType.Bread)
		{
			if (GameObject.Find("HPBarFill").GetComponent<Healthbar>().Health < 100f)
			{
				GameObject.Find("HPBarFill").GetComponent<Healthbar>().Health += 15f;
			}
			GameObject.Find("Inventory").GetComponent<Inventory>().CurrentSlot.GetComponent<InventorySlot>().Type = ItemType.EMPTY;
		}

		// IRON SWORD
		if (type == ItemType.IronSword)
		{
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			{
				if (GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>().IsTouching(enemy.GetComponent<BoxCollider2D>()))
				{
					float damage = (float)Math.Round(new FloatRange(0, IRON_SWORD_DAMAGE).Random, 2);

					// ANIMATE THE SWORD SWING

					// Decrease enemy health and create damage label.
					enemy.GetComponent<Enemy>().DecreaseHealth(damage);
				}
			}
		}

		// WATER
		if (type == ItemType.Water)
		{
			if (GameObject.Find("HPBarFill").GetComponent<Healthbar>().Health < 100f)
			{
				GameObject.Find("HPBarFill").GetComponent<Healthbar>().Health += 5f;
			}
			GameObject.Find("Inventory").GetComponent<Inventory>().CurrentSlot.GetComponent<InventorySlot>().Type = ItemType.EMPTY;
		}

		// FLAMING SWORD
		if (type == ItemType.FlamingSword)
		{
			// attack an enemy if there is one in front of you. Also show a cool animation
			// does fire damage in addition to regular damage (secondary use).
			// has the possibility of burning the enemy.

			float number = new FloatRange(0, 100).Random;
			// The regular use will only burn the enemy if it is an odd number less than 20.
			bool willBurnEnemy = (!(number % 2).Equals(0) && number < 20) ? true : false;

			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			{
				if (GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>().IsTouching(enemy.GetComponent<BoxCollider2D>()))
				{
					float damage = (float)Math.Round(new FloatRange(0, FLAMING_SWORD_DAMAGE).Random, 2);

					// ANIMATE THE SWORD SWING

					// Decrease enemy health and create damage label.
					enemy.GetComponent<Enemy>().DecreaseHealth(damage);

					// Chance of burning
					if (willBurnEnemy == true)
						enemy.GetComponent<Enemy>().Burned = true;
				}
			}
		}

		// HEALING SWORD
		if (type == ItemType.HealingSword)
		{
			// attack an enemy if there is one in front of you. Also show a cool animation
			// steal, then store, hp from the damage it does to an enemy.
			// the stored hp can be used to heal the player (secondary use).
			// No recharge time, but if there is no more hp stored in it then it will have no effect.
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			{
				if (GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>().IsTouching(enemy.GetComponent<BoxCollider2D>()))
				{
					float damage = (float)Math.Round(new FloatRange(0, HEALING_SWORD_DAMAGE).Random, 2);

					//Store one fourth of the damage you do
					StoredHP += damage / 4;

					// ANIMATE THE SWORD SWING

					// Decrease enemy health and create damage label.
					enemy.GetComponent<Enemy>().DecreaseHealth(damage);
				}
			}
		}

		// ELECTRIC SWORD
		if (type == ItemType.ElectricSword)
		{
			// attack an enemy if there is one in front of you. Also show a cool animation
			// stuns the enemy for a few, short seconds.
			// stuns all of the enemies on the map for 10 seconds.
			// Must recharge after secondary use.

			float number = new FloatRange(0, 100).Random;
			// The regular use will only stun the enemy if it is an odd number less than 20.
			bool willStunEnemy = (!(number % 2).Equals(0) && number < 20) ? true : false;

			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			{
				if (GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>().IsTouching(enemy.GetComponent<BoxCollider2D>()))
				{
					float damage = (float)Math.Round(new FloatRange(0, ELECTRIC_SWORD_DAMAGE).Random, 2);

					// ANIMATE THE SWORD SWING

					// Decrease enemy health and create damage label.
					enemy.GetComponent<Enemy>().DecreaseHealth(damage);

					if (willStunEnemy == true)
					{
						enemy.GetComponent<Enemy>().Stunned = true;
						enemy.GetComponent<Enemy>().stunTime = 5f;
					}
				}
			}
		}

		// LONG SWORD
		if (type == ItemType.LongSword)
		{
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			{
				if (GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>().IsTouching(enemy.GetComponent<BoxCollider2D>()))
				{
					float damage = (float)Math.Round(new FloatRange(0, LONG_SWORD_DAMAGE).Random, 2);

					// ANIMATE THE SWORD SWING

					// Decrease enemy health and create damage label.
					enemy.GetComponent<Enemy>().DecreaseHealth(damage);
				}
			}
		}

		// STEEL SWORD
		if (type == ItemType.SteelSword)
		{
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			{
				if (GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>().IsTouching(enemy.GetComponent<BoxCollider2D>()))
				{
					float damage = (float)Math.Round(new FloatRange(0, STEEL_SWORD_DAMAGE).Random, 2);

					// ANIMATE THE SWORD SWING

					// Decrease enemy health and create damage label.
					enemy.GetComponent<Enemy>().DecreaseHealth(damage);
				}
			}
		}

	}


	public static void InstantiateDamageLabel(GameObject onTopOff, float damage)
	{
		GameObject damagelabel = Resources.Load("DamageLabel") as GameObject;
		damagelabel.GetComponent<DamageLabel>().Text = "" + damage;
		Instantiate(damagelabel,
		            new Vector3(onTopOff.transform.position.x, onTopOff.transform.position.y, -2),
					Quaternion.identity);
	}


} //End of primary use class