using UnityEngine;
using System.Collections;

public class Item {

	/// <summary>
	/// The type of item this is.
	/// </summary>
	public ItemType Type;

	/// <summary>
	/// The quantity of this item.
	/// </summary>
	public int Quantity = 1;


	/// <summary>
	/// Whether or not this item can be stacked.
	/// </summary>
	public bool Stackable = false;


	void Start () {
		Type = ItemType.EMPTY;	//Default
	}

}
