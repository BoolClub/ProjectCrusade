using UnityEngine;
using System.Collections.Generic;

public class Chest : MonoBehaviour {

	#region References For Simplicity

		/// <summary>
		/// The s renderer.
		/// </summary>
		SpriteRenderer sRenderer;

		/// <summary>
		/// The box collide	r
		/// </summary>
		BoxCollider2D BoxColl;

		/// <summary>
		/// The player.
		/// </summary>
		PlayerControls Player;

	#endregion

	/// <summary>
	/// Whether or not this chest should choose a random item to give the player.
	/// </summary>
	public bool Randomize;

	/// <summary>
	/// A list of items to randomly give the player if Randomize is true.
	/// </summary>
	public ItemType[] RandomItems;

	/// <summary>
	/// This is the item that this chest will give the player.
	/// </summary>
	public ItemType Type;

	/// <summary>
	/// The amount of the item in this chest.
	/// </summary>
	public int Quantity = 1;

	/// <summary>
	/// The temp.
	/// </summary>
	public Item temp;

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

		if (Randomize == true)
		{
			int randomIndex = UnityEngine.Random.Range(0, RandomItems.Length);
			Type = RandomItems[randomIndex];
			Quantity = 1;
		}

		temp = new Item(Type);
		temp.Quantity = Quantity;

		if (Quantity == 1)
		{
			TextBox.addText("You received " + temp.Quantity + " " + temp.Name);
		}
		else if (Quantity > 1)
		{
			TextBox.addText("You received " + temp.Quantity + " " + temp.Name + "s");
		}

		sRenderer = GetComponent<SpriteRenderer>();
		BoxColl = GetComponent<BoxCollider2D>();
		Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		CheckCollision();

		//Draw the appropriate line of text
		foreach (GameObject tb in GameObject.FindGameObjectsWithTag("TextBoxClone"))
		{
			if (tb != null && TextBox.isOpen())
			{
				tb.GetComponentInChildren<TextMesh>().text = TextBox.Text[TextBox.CurrentSlide];
				tb.GetComponentInChildren<SmartText>().OnTextChanged();
			}
		}

		//Change the sprite when the chest has been opened
		if (Type == ItemType.EMPTY)
		{
			sRenderer.sprite = Opened;
		} else {
			sRenderer.sprite = Closed;
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
		if (BoxColl.IsTouching(Player.MyBoxCollider))
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
