﻿using UnityEngine;
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
	/// The inventory slot sprite.
	/// </summary>
	public Sprite InventorySlot_Sprite;

	/// <summary>
	/// The size of the slot.
	/// </summary>
	int SlotSize = 30;


	void Start()
	{
		Slots = new GameObject[Width, Height];

		//Set the position of all of the slots in the opened form.
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				GameObject slot = new GameObject("Inventory Slot");
				slot.AddComponent<InventorySlot>().IndexInInventory = new Vector2(j,i);
				slot.AddComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
				slot.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
				slot.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0.5f);
				slot.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SlotSize);
				slot.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SlotSize);

				slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(-140f + j * (SlotSize + 2), -140f + i * (-SlotSize - 2), -4000);

				slot.AddComponent<CanvasRenderer>();
				slot.AddComponent<Image>().sprite = InventorySlot_Sprite;
				slot.GetComponent<RectTransform>().SetParent(this.transform, false);
				Slots[j, i] = slot;
			}
		}



	}

	void Update()
	{
		CheckInventoryOpen();
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

					}
				}
			}
		}
	}
}
