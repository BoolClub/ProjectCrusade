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
}
