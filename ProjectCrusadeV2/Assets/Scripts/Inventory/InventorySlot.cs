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

		if (Type != ItemType.EMPTY)
		{
			GetComponentInParent<Image>().sprite = GameObject.Find("GameManager").GetComponent<GameManagerScript>().ItemSprites[(int)Type];
		}
	}
}
