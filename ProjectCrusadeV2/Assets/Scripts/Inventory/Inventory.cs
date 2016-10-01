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
				itm.Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
				itm.TheInventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
			}
		}
	}

	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.H))
		//{
		//	AddToInventory(new Item((ItemType)(new IntRange(1,20).Random)));
		//}
		//if (Input.GetKeyDown(KeyCode.I))
		//{
		//	Open = !Open;
		//}


		if (InventoryObject != null && !GM.Paused)
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


			// Calculate the recharge time for certain items.
			CalculateRechargeTime();
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
		if (!IsFull())
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
		else {
			// Debug.Log("Inventory is full.");
		}
	}


	public void Clear()
	{
		foreach (Item itm in GameManagerScript.Items)
		{
			itm.Remove();
		}
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


	public Item Find(ItemType type)
	{
		foreach (Item itm in Items)
		{
			if (itm.Type == type)
			{
				return itm;
			}
		}
		return null;
	}


	public bool IsFull()
	{
		foreach (Item itm in Items)
		{
			if (itm.Type == ItemType.EMPTY)
			{
				return false;
			}
		}
		return true;
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

		if (Input.GetKeyDown(KeyCode.Alpha1))
			CurrentSlot = 0;
		if (Input.GetKeyDown(KeyCode.Alpha2))
			CurrentSlot = 1;
		if (Input.GetKeyDown(KeyCode.Alpha3))
			CurrentSlot = 2;
		if (Input.GetKeyDown(KeyCode.Alpha4))
			CurrentSlot = 3;
		if (Input.GetKeyDown(KeyCode.Alpha5))
			CurrentSlot = 4;
		if (Input.GetKeyDown(KeyCode.Alpha6))
			CurrentSlot = 5;
		if (Input.GetKeyDown(KeyCode.Alpha7))
			CurrentSlot = 6;
		if (Input.GetKeyDown(KeyCode.Alpha8))
			CurrentSlot = 7;
		if (Input.GetKeyDown(KeyCode.Alpha9))
			CurrentSlot = 8;
		if (Input.GetKeyDown(KeyCode.Alpha0))
			CurrentSlot = 9;
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


	/// <summary>
	/// Calculates the recharge time for different weapons.
	/// </summary>
	/// <returns>The recharge time.</returns>
	public void CalculateRechargeTime()
	{
		foreach (Item itm in Items)
		{
			// Recharge electric sword
			if (itm.Type == ItemType.ElectricSword)
			{
				if (itm.chargeTimeElectricSword < 700)
				{
					itm.chargeTimeElectricSword += 1;
				}
				else {
					itm.chargeTimeElectricSword = 700;
				}
			}

			// Recharge flaming sword
			if (itm.Type == ItemType.FlamingSword)
			{
				if (itm.FlameSwordCharge < 5)
				{
					if (itm.RechargeTime_FlameSword < 350)
					{
						itm.RechargeTime_FlameSword += 1;
					}
					else {
						itm.FlameSwordCharge += 1;
						itm.RechargeTime_FlameSword = 0;
					}
				}
			}
		}
	}

} //End of inventory class
