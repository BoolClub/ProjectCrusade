using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Inventory : MonoBehaviour {

	/// <summary>
	/// The width and height of the inventory
	/// </summary>
	public int Width, Height;

	/// <summary>
	/// The inventory slots.
	/// </summary>
	public GameObject[,] Slots;

	/// <summary>
	/// Whether or not the full inventory is open. If not, only the first 10 slots wills show.
	/// </summary>
	public bool Open;

	/// <summary>
	/// The size of the slot.
	/// </summary>
	int SlotSize = 32;

	///This is the first empty spot found in the inventory
	public Vector2 firstEmpty = new Vector2(0, 0);

	/// <summary>
	/// The currently selected slot.
	/// </summary>
	[HideInInspector]
	public Vector2 CurrentSlotIndex;

	/// <summary>
	/// The current slot game object.
	/// </summary>
	[HideInInspector]
	public GameObject CurrentSlot;



	void Start()
	{
		Slots = new GameObject[Width, Height];
		CurrentSlotIndex = new Vector2();

		//Set the position of all of the slots in the opened form.
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				//Create the inventory slot
				GameObject slot = new GameObject("Inventory Slot");
				slot.AddComponent<InventorySlot>().IndexInInventory = new Vector2(j,i);
				slot.GetComponent<InventorySlot>().Type = ItemType.EMPTY;
				slot.GetComponent<InventorySlot>().InvSlot = slot;
				slot.AddComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
				slot.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
				slot.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
				slot.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SlotSize);
				slot.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SlotSize);

				slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(-140f + j * (SlotSize + 2), -140f + i * (-SlotSize - 2), -4000);

				slot.AddComponent<CanvasRenderer>();
				slot.AddComponent<Image>().sprite = GameObject.Find("GameManager").GetComponent<GameManagerScript>().ItemSprites[0]; ;
				slot.GetComponent<RectTransform>().SetParent(this.transform, false);
				Slots[j, i] = slot;
			}
		}

		//Set the initial current slot
		CurrentSlot = Slots[(int)CurrentSlotIndex.x, (int)CurrentSlotIndex.y];

		//An an overlay image for each inventory slot. This is the item image.
		AddInventorySlotItemImage();
	}

	void Update()
	{
		CheckInventoryOpen();

		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				//Slots[j, i].GetComponent<InventorySlot>().Update();
				if (!Slots[j, i].GetComponent<InventorySlot>().Type.Equals(ItemType.EMPTY))
				{
					Slots[j,i].transform.GetChild(0).GetComponent<Image>().enabled = true;
					Slots[j, i].transform.GetChild(0).GetComponent<Image>().sprite = GameObject.Find("GameManager").GetComponent<GameManagerScript>().ItemSprites[(int)Slots[j,i].GetComponent<InventorySlot>().Type];
				}
				else {
					Slots[j, i].transform.GetChild(0).GetComponent<Image>().enabled = false;
				}
			}
		}

		//Handles input for moving between different slots.
		CurrentSlotInput();
	}


	/// <summary>
	/// Adds an item to the inventory.
	/// </summary>
	/// <returns>The to inventory.</returns>
	/// <param name="item">Item.</param>
	public void AddToInventory(ItemType item)
	{
		//Find the first empty spot
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				//If there is an empty spot or a spot with the same item and the item is stackable
				if (Slots[Width - j - 1, Height - i - 1].GetComponent<InventorySlot>().Type == ItemType.EMPTY)
				{
					//The first empty slot is the slot's index in the inventory.
					firstEmpty = new Vector2(Slots[j, i].GetComponent<InventorySlot>().IndexInInventory.x, Slots[j, i].GetComponent<InventorySlot>().IndexInInventory.y);
				}
			}
		}

		Slots[Width - (int)firstEmpty.x - 1, Height - (int)firstEmpty.y - 1].GetComponent<InventorySlot>().Type = item;

	}

	/// <summary>
	/// Shows the full inventory when it is open and only shows the first ten items when it is closed.
	/// </summary>
	/// <returns>The inventory open.</returns>
	void CheckInventoryOpen()
	{
		if (Open)
		{
			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					Slots[j, i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-140f + j * (SlotSize + 2), -140f + i * (-SlotSize - 2), -4000);
					Slots[j, i].SetActive(true);
					Slots[j, i].GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
				}
			}
		}
		else {
			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					//It isn't on the first line of the inventory
					if (Slots[j, i].GetComponent<InventorySlot>().IndexInInventory.y > 0)
					{
						Slots[j, i].SetActive(false);
					}
					else {
						Slots[j,i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-140f + j * (SlotSize + 2), -25f + i * (-SlotSize - 2), -4000);
						Slots[j, i].GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
					}
				}
			}
		}
	}


	/// <summary>
	/// Adds another image as a child of every inventory slot.
	/// </summary>
	/// <returns>The inventory slot image.</returns>
	void AddInventorySlotItemImage()
	{
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				GameObject itemImg = new GameObject("Item Image");
				itemImg.AddComponent<RectTransform>();
				itemImg.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
				itemImg.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
				itemImg.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
				itemImg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 30);
				itemImg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);
				itemImg.transform.position = new Vector3(0,-15,0);

				itemImg.AddComponent<CanvasRenderer>();
				itemImg.AddComponent<Image>().sprite = GameObject.Find("GameManager").GetComponent<GameManagerScript>().ItemSprites[(int)Slots[j,i].GetComponent<InventorySlot>().Type];

				itemImg.transform.SetParent(Slots[j,i].transform, false);
			}
		}
		return;
	}


	/// <summary>
	/// Checks for input that changes which inventory slot is which.
	/// </summary>
	/// <returns>The slot input.</returns>
	void CurrentSlotInput()
	{
		//Set the current slot color
		Slots[(int)CurrentSlotIndex.x, (int)CurrentSlotIndex.y].GetComponent<Image>().color = Color.red;
		CurrentSlot = Slots[(int)CurrentSlotIndex.x, (int)CurrentSlotIndex.y];

		//Change the selected slot when you scroll through the mouse
		//If the inventory is open you can move through every slot.
		if (Open)
		{
			if (Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				if (CurrentSlotIndex.x < Width - 1)
					CurrentSlotIndex.Set(CurrentSlotIndex.x + 1, CurrentSlotIndex.y);
				else
					if (CurrentSlotIndex.y < Height - 1)
					CurrentSlotIndex.Set(0, CurrentSlotIndex.y + 1);
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0)
			{
				if (CurrentSlotIndex.x > 0)
					CurrentSlotIndex.Set(CurrentSlotIndex.x - 1, CurrentSlotIndex.y);
				else
					if (CurrentSlotIndex.y > 0)
					CurrentSlotIndex.Set(Width - 1, CurrentSlotIndex.y - 1);
			}
		}
		//If the inventory is not open you can only move through the first row
		else {
			if (Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				if (CurrentSlotIndex.x < Width - 1)
					CurrentSlotIndex.Set(CurrentSlotIndex.x + 1, CurrentSlotIndex.y);
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0)
			{
				if (CurrentSlotIndex.x > 0)
					CurrentSlotIndex.Set(CurrentSlotIndex.x - 1, CurrentSlotIndex.y);
			}
		}
	}


} //End of inventory class.
