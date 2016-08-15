using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySlot : MonoBehaviour {


	/// <summary>
	/// The item type.
	/// </summary>
	public ItemType Type;

	/// <summary>
	/// The index in inventory.
	/// </summary>
	public Vector2 IndexInInventory;

	/// <summary>
	/// The inventory slot for this item.
	/// </summary>
	public GameObject InvSlot;


	void Start()
	{
		Type = ItemType.EMPTY;
	}

}
