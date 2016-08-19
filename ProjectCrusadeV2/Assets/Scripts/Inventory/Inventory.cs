using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Inventory : MonoBehaviour
{
	#region References For Simplicity

		GameManagerScript GM;

	#endregion

	/// <summary>
	/// The list of items that the player has.
	/// </summary>
	public Item[] Items;

	/// <summary>
	/// Whether or not the inventory is open.
	/// </summary>
	public bool Open;

	/// <summary>
	/// The first empty space in the inventory.
	/// </summary>
	public int FirstEmpty = 0;

	/// <summary>
	/// The game object that has all the inventory slots.
	/// </summary>
	public GameObject[] InventorySlots;

	/// <summary>
	/// The inventory object.
	/// </summary>
	public GameObject InventoryObject;

	/// <summary>
	/// The currently selected slot.
	/// </summary>
	public int CurrentSlot;




	void Start()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
		Items = new Item[40];
		CurrentSlot = 0;
		for (int i = 0; i < Items.Length; i++)
		{
			Items[i] = GameManagerScript.Items[i];
		}
		InventoryObject = GameObject.Find("Inventory");
	}

	void Awake()
	{
		InventoryObject = GameObject.Find("Inventory");
		InventorySlots = GameObject.FindGameObjectsWithTag("InventorySlot");
		if (Items != null)
		{
			foreach (Item itm in Items)
			{
				itm.HPBar = GameObject.Find("HPBarFill").GetComponent<Healthbar>();
				itm.Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
				itm.TheInventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
			}
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.H))
		{
			AddToInventory(new Item((ItemType)(new IntRange(1,15).Random)));
		}

		//if (Input.GetKeyDown(KeyCode.I))
		//{
		//	Open = !Open;
		//}


		if (InventoryObject != null)
		{
			//Move the selected slot by scrolling
			MoveCurrentSlot();


			// Color the selected slot
			ColorSelectedSlot();


			// If a slot does not have an item, set its image to blank
			ColorBlankSlots();


			// Enable/Disable the quantity label if there is/isn't an item
			EnableDisableQuantityLabel();


			// Draw an item on each slot if that slot has an item.
			DrawItemsOnSlots();


			// Only make certain slots active if the inventory is open/closed.
			ShowSlotsBasedOnOpenOrClosed();


			// Remove an item from the inventory if its quantity is zero
			RemoveItemsIfQuantityZero();
		}


		// Interact with a selected item
		//if (Input.GetKeyDown(KeyCode.U))
		//{
		//	if (Items[CurrentSlot].Type != ItemType.EMPTY)
		//	{
		//		Items[CurrentSlot].PrimaryUse();
		//	}
		//}
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


	public bool Contains(ItemType itm)
	{
		foreach (Item i in Items)
		{
			if (i.Type == itm)
				return true;
		}
		return false;
	}


	void MoveCurrentSlot()
	{
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
	}


	void ColorSelectedSlot()
	{
		foreach (GameObject go in InventorySlots)
		{
			Image img = go.GetComponent<Image>();
			if (go.GetComponent<InventorySlot>().Index == CurrentSlot)
			{
				img.color = Color.green;
			}
			else {
				img.color = Color.white;
			}
		}
	}


	void ColorBlankSlots()
	{
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
					InventorySlots[i].transform.GetChild(0).GetComponentInChildren<Image>().sprite = GM.ItemSprites[(int)Items[i].Type];
					InventorySlots[i].transform.GetChild(1).GetComponentInChildren<Text>().text = "" + Items[i].Quantity;
				}
			}
		}
	}


	void DrawItemsOnSlots()
	{
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
					slot.transform.GetChild(0).GetComponentInChildren<Image>().sprite = GM.ItemSprites[(int)Items[i].Type];
					slot.transform.GetChild(1).GetComponentInChildren<Text>().text = "" + Items[i].Quantity;
				}
			}
		}
	}

	void EnableDisableQuantityLabel()
	{
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
					slot.transform.GetChild(1).GetComponentInChildren<Text>().enabled = false;
				}
			}
			else {
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
					slot.transform.GetChild(1).GetComponentInChildren<Text>().enabled = true;
				}
			}
		}
	}


	void ShowSlotsBasedOnOpenOrClosed()
	{
		if (!Open)
		{
			for (int i = 0; i < Items.Length; i++)
			{
				if (InventorySlots[i].GetComponent<InventorySlot>().Index >= 10)
				{
					InventorySlots[i].SetActive(false);
				}
			}
		}
		else {
			for (int i = 0; i < Items.Length; i++)
			{
				InventorySlots[i].SetActive(true);
			}
		}
	}


	void RemoveItemsIfQuantityZero()
	{
		foreach (Item itm in Items)
		{
			if (itm.Quantity <= 0)
			{
				itm.Type = ItemType.EMPTY;
			}
		}
	}

} //End of inventory class
