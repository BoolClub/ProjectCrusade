using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	/// <summary>
	/// This is the item that this chest will give the player.
	/// </summary>
	public ItemType Type;

	/// <summary>
	/// The sprites for when the chest is opened and closed.
	/// </summary>
	public Sprite Opened, Closed;

	/// <summary>
	/// The textbox that displays what the user got.
	/// </summary>
	public TextBox TextBox;

	/// <summary>
	/// Whether or not it is next to the player.
	/// </summary>
	public bool NextToPlayer;


	void Start()
	{
		TextBox = new TextBox();
		TextBox.addText("You received a(n) " + Type);
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		CheckCollision();

		//Draw the appropriate line of text
		if (GameObject.Find("TextBox(Clone)") != null && TextBox.isOpen())
		{
			(GameObject.Find("TextBox(Clone)").GetComponentInChildren<TextMesh>()).text = TextBox.Text[TextBox.CurrentSlide];
			(GameObject.Find("TextBox(Clone)").GetComponentInChildren<SmartText>()).OnTextChanged();
		}

		//Change the sprite when the chest has been opened
		if (Type == ItemType.EMPTY)
		{
			GetComponent<SpriteRenderer>().sprite = Opened;
		} else {
			GetComponent<SpriteRenderer>().sprite = Closed;
		}
	}


	/// <summary>
	/// Takes the item from the chest.
	/// </summary>
	/// <returns>The item.</returns>
	public void TakeItem() { Type = ItemType.EMPTY; }


	/// <summary>
	/// Returns whether or not the player is next to the chest.
	/// </summary>
	/// <returns>The next to player.</returns>
	public bool isNextToPlayer()
	{
		return NextToPlayer;
	}


	/// <summary>
	/// Checks for collision between this NPC and the player.
	/// </summary>
	/// <returns>The collision.</returns>
	public void CheckCollision()
	{
		if (GetComponent<BoxCollider2D>().IsTouching(GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>()))
		{
			NextToPlayer = true;
		}
		else {
			NextToPlayer = false;
			if (TextBox.isOpen())
			{
				Object.Destroy(GameObject.Find("TextBox(Clone)"));
				TextBox.toggle();
			}
		}
	}
}
