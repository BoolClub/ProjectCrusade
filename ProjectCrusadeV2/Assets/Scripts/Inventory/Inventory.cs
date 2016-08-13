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
	int SlotSize = 30;

	///This is the first empty spot found in the inventory
	public Vector2 firstEmpty = new Vector2(0, 0);



	void Start()
	{
		Slots = new GameObject[Width, Height];

		//Set the position of all of the slots in the opened form.
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				//Create the inventory slot
				GameObject slot = new GameObject("Inventory Slot");
				slot.AddComponent<InventorySlot>().IndexInInventory = new Vector2(j,i);
				slot.GetComponent<InventorySlot>().Type = ItemType.EMPTY;
				slot.AddComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
				slot.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
				slot.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
				slot.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SlotSize);
				slot.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SlotSize);

				slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(-140f + j * (SlotSize + 2), -140f + i * (-SlotSize - 2), -4000);

				slot.AddComponent<CanvasRenderer>();
				slot.AddComponent<Image>();
				slot.GetComponent<RectTransform>().SetParent(this.transform, false);
				Slots[j, i] = slot;
			}
		}
	}

	void Update()
	{
		CheckInventoryOpen();
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				Slots[j, i].GetComponent<InventorySlot>().Update();
			}
		}
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
					Slots[j, i].GetComponent<Image>().color = new Color(255, 255, 255, 1f);
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
}
