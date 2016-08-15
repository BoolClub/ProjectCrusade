using UnityEngine;
using System.Collections;


/// <summary>
/// The purpose of this class is to serve as a way for the player to use items. Just call the methods for primary
/// and secondary use of the specified item type.
/// </summary>
public class SecondaryUseItems
{


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
			// there is no secondary use for an apple. You can only eat it (primary use).
		}

		// COIN
		if (type == ItemType.Coin)
		{
			// trying to use a coin will not do anything
		}

		// WOODEN SWORD
		if (type == ItemType.WoodenSword)
		{
			// attack an enemy if there is one in front of you. Also show a cool animation
		}

		// BOW AND ARROW
		if (type == ItemType.BowAndArrow)
		{
			// make sure that the player also has arrows in the inventory.
			// shoot a projectile (arrow). Have an arrow script that does damage to an enemy if it is hit.
		}

		// MAGIC WAND
		if (type == ItemType.MagicWand)
		{
			// shoot a projectile (magic bolt). Have an arrow script that does damage to an enemy if it is hit.
		}

		// MACE
		if (type == ItemType.Mace)
		{
			// attack an enemy if there is one in front of you. Also show a cool animation
		}

		// CURVED SWORD
		if (type == ItemType.CurvedSword)
		{
			// attack an enemy if there is one in front of you. Also show a cool animation
		}

		// ARROW
		if (type == ItemType.Arrow)
		{
			// arrows alone don't do anything, but if you also have a bow and arrow you can use this to shoot it
		}

		// BREAD
		if (type == ItemType.Bread)
		{
			// there is no secondary use for bread. You can only eat it (primary use).
		}

		// IRON SWORD
		if (type == ItemType.IronSword)
		{
			// attack an enemy if there is one in front of you. Also show a cool animation
		}

		// WATER
		if (type == ItemType.Water)
		{
			// there is no secondary use for water. You can only drink it (primary use).
		}

		// FLAMING SWORD
		if (type == ItemType.FlamingSword)
		{
			// attack an enemy if there is one in front of you. Also show a cool animation
			// does fire damage in addition to regular damage (secondary use).
			// has the possibility of burning the enemy.
		}

		// HEALING SWORD
		if (type == ItemType.HealingSword)
		{
			// attack an enemy if there is one in front of you. Also show a cool animation
			// steal, then store, hp from the damage it does to an enemy.
			// the stored hp can be used to heal the player (secondary use).
			// No recharge time, but if there is no more hp stored in it then it will have no effect.
		}

		// ELECTRIC SWORD
		if (type == ItemType.ElectricSword)
		{
			// attack an enemy if there is one in front of you. Also show a cool animation
			// stuns the enemy for a few, short seconds.
			// stuns all of the enemies on the map for 10 seconds.
			// Must recharge after secondary use.
		}

		// LONG SWORD
		if (type == ItemType.LongSword)
		{
			// attack an enemy if there is one in front of you. Also show a cool animation
		}

		// STEEL SWORD
		if (type == ItemType.SteelSword)
		{
			// attack an enemy if there is one in front of you. Also show a cool animation
		}
	}

} //End of secondary use class