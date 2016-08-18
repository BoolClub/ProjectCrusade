using UnityEngine;
using System.Collections;

public class Item {

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
			
		}
		if (Type == ItemType.Arrow)
		{
			
		}
		if (Type == ItemType.BowAndArrow)
		{
			
		}
		if (Type == ItemType.Bread)
		{
			
		}
		if (Type == ItemType.CurvedSword)
		{
			
		}
		if (Type == ItemType.ElectricSword)
		{
			
		}
		if (Type == ItemType.FlamingSword)
		{
			
		}
		if (Type == ItemType.HealingSword)
		{
			
		}
		if (Type == ItemType.IronSword)
		{
			
		}
		if (Type == ItemType.LongSword)
		{
			
		}
		if (Type == ItemType.Mace)
		{
			
		}
		if (Type == ItemType.MagicWand)
		{
			
		}
		if (Type == ItemType.SteelSword)
		{
			
		}
		if (Type == ItemType.Water)
		{
			
		}
		if (Type == ItemType.WoodenSword)
		{
			
		}
	}


	/// <summary>
	/// SECONDARY USE OF ITEMS
	/// </summary>
	/// <returns>The use.</returns>
	public void SecondaryUse()
	{
		if (Type == ItemType.Apple)
		{

		}
		if (Type == ItemType.Arrow)
		{

		}
		if (Type == ItemType.BowAndArrow)
		{

		}
		if (Type == ItemType.Bread)
		{

		}
		if (Type == ItemType.CurvedSword)
		{

		}
		if (Type == ItemType.ElectricSword)
		{

		}
		if (Type == ItemType.FlamingSword)
		{

		}
		if (Type == ItemType.HealingSword)
		{

		}
		if (Type == ItemType.IronSword)
		{

		}
		if (Type == ItemType.LongSword)
		{

		}
		if (Type == ItemType.Mace)
		{

		}
		if (Type == ItemType.MagicWand)
		{

		}
		if (Type == ItemType.SteelSword)
		{

		}
		if (Type == ItemType.Water)
		{

		}
		if (Type == ItemType.WoodenSword)
		{

		}
	}

	public override string ToString()
	{
		return "Name: " + Name + ", Type: " + Type.ToString() + "Quantity: " + Quantity + ", Stackable: " + Stackable;
	}
}
