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


	void Start()
	{
		Type = ItemType.EMPTY;
	}

	public void Update()
	{
		//Some random number that would never be an item index.
		if (!Type.Equals(ItemType.EMPTY))
		{
			GetComponentInParent<Image>().sprite = GameObject.Find("GameManager").GetComponent<GameManagerScript>().ItemSprites[(int)Type];
		}
	}
}
