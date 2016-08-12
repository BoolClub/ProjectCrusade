using UnityEngine;
using System.Collections;

public class InventorySlot : MonoBehaviour{

	/// <summary>
	/// The item in this inventory slot.
	/// </summary>
	public ItemType Item;

	/// <summary>
	/// The index in inventory.
	/// </summary>
	public Vector2 IndexInInventory;


	void Start()
	{
		//Default for all inventory slots
		Item = ItemType.EMPTY;
	}
}
