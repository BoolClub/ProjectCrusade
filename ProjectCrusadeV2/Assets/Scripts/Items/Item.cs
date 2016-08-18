using UnityEngine;
using System.Collections;

public class Item {

	public ItemType Type;

	public bool Stackable;

	public int Quantity = 1;

	public string Name;



	public Item(ItemType type)
	{
		Type = type;
		DetermineNameAndStackable();
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



		if (Type == ItemType.WoodenSword)
		{
			Name = "Wooden Sword";
			Stackable = false;
		}
	}


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


	public void PrimaryUse()
	{
		if (Type == ItemType.Apple)
		{
			Debug.Log("Yum! An apple!");
		}
		if (Type == ItemType.WoodenSword)
		{
			Debug.Log("Take that! *Swings Swords*");
		}
		if (Type == ItemType.Bread)
		{
			Debug.Log("Awesome! Bread");
		}
	}


	public void SecondaryUse()
	{
		if (Type == ItemType.Apple)
		{
			Debug.Log("Wow! That really is a tasty apple!");
		}
		if (Type == ItemType.WoodenSword)
		{
			Debug.Log("This is a really sharp sword.");
		}
		if (Type == ItemType.Bread)
		{
			Debug.Log("This bread is still awesome.");
		}
	}

	public override string ToString()
	{
		return "Name: " + Name + ", Quantity: " + Quantity;
	}
}
