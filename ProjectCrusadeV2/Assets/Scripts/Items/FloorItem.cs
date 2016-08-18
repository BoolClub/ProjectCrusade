﻿using UnityEngine;
using System.Collections;

/// <summary>
/// This represents items that are on the floor.
/// </summary>
public class FloorItem : MonoBehaviour {

	/// <summary>
	/// The type of item.
	/// </summary>
	public ItemType itm;

	/// <summary>
	/// The quantity of the item.
	/// </summary>
	public IntRange Quantity;

	/// <summary>
	/// The textbox that displays what the user got.
	/// </summary>
	public TextBox TextBox;

	/// <summary>
	/// The textbox object.
	/// </summary>
	GameObject textboxObj;

	/// <summary>
	/// The timer.
	/// </summary>
	float timer = 500f;



	// Use this for initialization
	void Start () {
		TextBox = new TextBox();
		Item i = new Item(itm);
		TextBox.addText("Press \"c\" to pick up the " + i.Name);
		textboxObj = Resources.Load("TextBox") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {

		//Draw the appropriate line of text
		foreach (GameObject tb in GameObject.FindGameObjectsWithTag("TextBoxClone"))
		{
			if (tb != null && TextBox.isOpen())
			{
				tb.GetComponentInChildren<TextMesh>().text = TextBox.Text[TextBox.CurrentSlide];
				tb.GetComponentInChildren<SmartText>().OnTextChanged();
			}
		}

		// Destroy the item after some time.
		timer -= 0.5f;
		if (timer <= 0)
		{
			Destroy(this.gameObject);
			foreach (GameObject tb in GameObject.FindGameObjectsWithTag("TextBoxClone"))
			{
				if (tb != null && TextBox.isOpen())
				{
					Destroy(tb);
					break;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			if (TextBox.isOpen() == false)
			{
				TextBox.setOpen(true);
				Instantiate(textboxObj, new Vector3(transform.position.x + 0.75f, transform.position.y + 1.4f, -3), Quaternion.identity);
			}

			if (Input.GetKeyDown(KeyCode.C))
			{
				GameObject.FindWithTag("Player").GetComponent<Inventory>().AddToInventory(new Item(itm, Quantity.Value));
				Destroy(this.gameObject);
			}
		}

	}


	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			if (TextBox.isOpen()) {
				TextBox.setOpen(false);
				Object.Destroy(GameObject.Find("TextBox(Clone)"));
			}
		}
	}

}
