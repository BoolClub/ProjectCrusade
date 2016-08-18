using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Inventory : MonoBehaviour {

	/// <summary>
	/// The list of items that the player has.
	/// </summary>
	public Item[] Items;

	/// <summary>
	/// The first empty space in the inventory.
	/// </summary>
	public int FirstEmpty = 0;

	/// <summary>
	/// The game object that has all the inventory slots.
	/// </summary>
	public GameObject[] InventorySlots;

	/// <summary>
	/// The currently selected slot.
	/// </summary>
	public int CurrentSlot;




	void Start () {
		Items = new Item[40];
		CurrentSlot = 0;
		InventorySlots = GameObject.FindGameObjectsWithTag("InventorySlot");

		for (int i = 0; i < Items.Length; i++)
			Items[i] = new Item(ItemType.EMPTY);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.H))
		{
			AddToInventory(new Item(ItemType.WoodenSword));
			AddToInventory(new Item(ItemType.Apple));
			AddToInventory(new Item(ItemType.Bread));
		}

		//Move the selected slot
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if (CurrentSlot > 0)
			{
				CurrentSlot--;
			}
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			if (CurrentSlot < Items.Length - 1)
			{
				CurrentSlot++;
			}
		}

		// Color the selected slot
		foreach (GameObject go in InventorySlots)
		{
			if (go.GetComponent<InventorySlot>().Index == CurrentSlot)
			{
				go.GetComponent<Image>().color = Color.green;
			}
			else {
				go.GetComponent<Image>().color = Color.white;
			}
		}

		// If a slot does not have an item, set its image to blank
		for (int i = 0; i < Items.Length; i++)
		{
			if (Items[i].Type == ItemType.EMPTY)
			{
				GameObject slot = null;
				for (int j = 0; j < InventorySlots.Length; j++)
				{
					if (InventorySlots[j].GetComponent<InventorySlot>().Index == i)
					{
						slot = InventorySlots[j];
						break;
					}
				}

				if (slot != null)
				{
					InventorySlots[i].transform.GetChild(0).GetComponentInChildren<Image>().sprite = GameObject.Find("GameManager").GetComponent<GameManagerScript>().ItemSprites[(int)Items[i].Type];
					InventorySlots[i].transform.GetChild(1).GetComponentInChildren<Text>().text = "" + Items[i].Quantity;
				}
			}
		}

		// Draw an item on each slot
		for (int i = 0; i < Items.Length; i++)
		{
			if (Items[i].Type != ItemType.EMPTY)
			{
				GameObject slot = null;
				for (int j = 0; j < InventorySlots.Length; j++)
				{
					if (InventorySlots[j].GetComponent<InventorySlot>().Index == i)
					{
						slot = InventorySlots[j];
						break;
					}
				}

				if (slot != null)
				{
					slot.transform.GetChild(0).GetComponentInChildren<Image>().sprite = GameObject.Find("GameManager").GetComponent<GameManagerScript>().ItemSprites[(int)Items[i].Type];
					slot.transform.GetChild(1).GetComponentInChildren<Text>().text = "" + Items[i].Quantity;
				}
			}
		}

		// Interact with a selected item
		if (Input.GetKeyDown(KeyCode.U))
		{
			if (Items[CurrentSlot].Type != ItemType.EMPTY)
			{
				Items[CurrentSlot].Use();
			}
		}
	}

	public void AddToInventory(Item itm)
	{
		for (int i = 0; i < Items.Length; i++)
		{
			if (Items[i].Type == ItemType.EMPTY || (Items[i].Type == itm.Type && Items[i].Stackable))
			{
				FirstEmpty = i;
				break;
			}
		}

		Items[FirstEmpty].Add(itm);
	}


}
