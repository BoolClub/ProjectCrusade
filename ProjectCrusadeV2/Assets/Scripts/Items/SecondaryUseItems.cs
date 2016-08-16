using UnityEngine;
using System;
using System.Collections;


/// <summary>
/// The purpose of this class is to serve as a way for the player to use items. Just call the methods for primary
/// and secondary use of the specified item type.
/// </summary>
public class SecondaryUseItems : MonoBehaviour
{
	/// <summary>
	/// Handles recharging the swords.
	/// </summary>
	static float chargeTimeFlameSword = 200f;
	static float chargeTimeElectricSword = 700f;

	public static void ResetCharges()
	{
		// Reset the charges for the flame sword
		if (GameObject.Find("Inventory").GetComponent<Inventory>().Contains(ItemType.FlamingSword))
		{
			if (PrimaryUseItems.FlameSwordCharge != 5)
			{
				chargeTimeFlameSword -= 1f;
				if (chargeTimeFlameSword <= 0)
				{
					PrimaryUseItems.FlameSwordCharge++;
					chargeTimeFlameSword = 200f;
				}
			}
		}
		// Reset the charges for the electric sword
		if (GameObject.Find("Inventory").GetComponent<Inventory>().Contains(ItemType.ElectricSword))
		{
			chargeTimeElectricSword += 0.5f;
			if (chargeTimeElectricSword >= 700f)
			{
				chargeTimeElectricSword = 700f;
			}
		}
	}


	/// <summary>
	/// Primary use of the specified item.
	/// </summary>
	/// <returns>The use.</returns>
	/// <param name="type">Type.</param>
	public static void SecondaryUse(ItemType type)
	{
		// APPLE
		if (type == ItemType.Apple)
		{
			// NO SECONDARY USE
		}

		// COIN
		if (type == ItemType.Coin)
		{
			// NO SECONDARY USE
		}

		// WOODEN SWORD
		if (type == ItemType.WoodenSword)
		{
			// NO SECONDARY USE
		}

		// BOW AND ARROW
		if (type == ItemType.BowAndArrow)
		{
			// make sure that the player also has arrows in the inventory.
			// shoot a projectile (arrow). Have an arrow script that does damage to an enemy if it is hit.
			// only a secondary use if you have a particular type of arrow.
		}

		// MAGIC WAND
		if (type == ItemType.MagicWand)
		{
			GameObject magicbolt = Resources.Load("Projectiles/Magic Bolt2") as GameObject;
			Instantiate(magicbolt, GameObject.FindWithTag("Player").transform.position, Quaternion.identity);
		}

		// MACE
		if (type == ItemType.Mace)
		{
			// NO SECONDARY USE
		}

		// CURVED SWORD
		if (type == ItemType.CurvedSword)
		{
			// NO SECONDARY USE
		}

		// ARROW
		if (type == ItemType.Arrow)
		{
			// NO SECONDARY USE
		}

		// BREAD
		if (type == ItemType.Bread)
		{
			// NO SECONDARY USE
		}

		// IRON SWORD
		if (type == ItemType.IronSword)
		{
			// NO SECONDARY USE
		}

		// WATER
		if (type == ItemType.Water)
		{
			// NO SECONDARY USE
		}

		// FLAMING SWORD
		if (type == ItemType.FlamingSword)
		{
			// does fire damage in addition to regular damage (secondary use).
			// has the possibility of burning the enemy.
			if (PrimaryUseItems.FlameSwordCharge > 0)
			{
				float number = new FloatRange(0, 100).Random;
				// The flame attack will only burn the enemy if it is an odd number less than 35.
				bool willBurnEnemy = (!(number % 2).Equals(0) && number < 35) ? true : false;

				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
				{
					if (GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>().IsTouching(enemy.GetComponent<BoxCollider2D>()))
					{
						#pragma warning disable
						float damage = (float)Math.Round(new FloatRange(0, PrimaryUseItems.FLAMING_SWORD_DAMAGE).Random, 2);

						// ANIMATE THE SWORD SWING

						// Remove enemy health
						enemy.GetComponent<Enemy>().DecreaseHealth(damage);

						// Chance of burning
						if (willBurnEnemy == true)
							enemy.GetComponent<Enemy>().Burned = true;
					}
				}

				//Shoot fire projectile
				GameObject fire = Resources.Load("Projectiles/Fire Bolt") as GameObject;
				Instantiate(fire, GameObject.FindWithTag("Player").transform.position, Quaternion.identity);

				PrimaryUseItems.FlameSwordCharge--;
			}
		}

		// HEALING SWORD
		if (type == ItemType.HealingSword)
		{
			// take, then store, hp from the damage it does to an enemy.
			// the stored hp can be used to heal the player (secondary use).
			// No recharge time, but if there is no more hp stored in it then it will have no effect.

			GameObject.Find("HPBarFill").GetComponent<Healthbar>().Health += PrimaryUseItems.StoredHP;
			PrimaryUseItems.StoredHP = 0;
		}

		// ELECTRIC SWORD
		if (type == ItemType.ElectricSword)
		{
			// stuns the enemy for a few, short seconds.
			// stuns all of the enemies on the map for 10 seconds.
			// Must recharge after secondary use.

			if (chargeTimeElectricSword.Equals(700f))
			{
				foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
				{
					// Secondary use of the electric sword stuns all enemies on the map
					enemy.GetComponent<Enemy>().Stunned = true;
					enemy.GetComponent<Enemy>().stunTime = 15f;
				}
				chargeTimeElectricSword = 0;
				ResetCharges();
			}
		}

		// LONG SWORD
		if (type == ItemType.LongSword)
		{
			// NO SECONDARY USE
		}

		// STEEL SWORD
		if (type == ItemType.SteelSword)
		{
			// NO SECONDARY USE
		}
	}

} //End of secondary use class