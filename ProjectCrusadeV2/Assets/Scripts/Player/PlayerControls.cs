using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	/// <summary>
	/// The world.
	/// </summary>
	public World world;

	/// <summary>
	/// The player's rigid body.
	/// </summary>
	Rigidbody2D Rigid;

	/// <summary>
	/// The direction that the player is facing.
	/// </summary>
	public Direction Direction;

	/// <summary>
	/// The direction sprites.
	/// </summary>
	public Sprite[] DirectionSprites;

	/// <summary>
	/// The speed of the player.
	/// </summary>
	public float speed;

	/// <summary>
	/// The player's starting position
	/// </summary>
	public Vector3 StartPosition;


	public void Start () {
		transform.position = StartPosition;
		gameObject.layer = 10;
		Rigid = GetComponent<Rigidbody2D>();
	}

	void Update () {
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		float y = Input.GetAxis("Vertical") * Time.deltaTime * speed;

		//Update the player's sprite based on the button presse.
		UpdatePlayerDirections(x, y);

		//Only move the player when the inventroy is not open.
		if (!Inventory.Open)
			Rigid.MovePosition(new Vector2(this.transform.position.x + x, this.transform.position.y + y));


		//Check for other types of player input
		CheckInput();
	}


	/// <summary>
	/// Checks the input.
	/// </summary>
	/// <returns>The input.</returns>
	void CheckInput()
	{
		KeyCode primaryUseKey = KeyCode.Z;
		KeyCode secondaryUseKey = KeyCode.X;
		KeyCode interactionKey = KeyCode.C;


		if (Input.GetKeyDown(interactionKey))
		{
			NPCInteraction();
			CheckInteraction();
		}
		if (Input.GetKeyDown(primaryUseKey))
		{
			PrimaryUseItems.PrimaryUse(GameObject.Find("Inventory").GetComponent<Inventory>().CurrentSlot.GetComponent<InventorySlot>().Type);
		}
		if (Input.GetKeyDown(secondaryUseKey))
		{
			SecondaryUseItems.SecondaryUse(GameObject.Find("Inventory").GetComponent<Inventory>().CurrentSlot.GetComponent<InventorySlot>().Type);
		}

		//Open the inventory
		if (Input.GetKeyDown(KeyCode.I))
		{
			Inventory.Open = !Inventory.Open;
		}
	}



	/// <summary>
	/// Checks for interaction with things other than NPCs
	/// </summary>
	/// <returns>The interaction.</returns>
	void CheckInteraction()
	{
		foreach (GameObject chest in world.Chests)
		{
			if (chest.GetComponent<Chest>().isNextToPlayer())
			{
				if (chest.GetComponent<Chest>().Type != ItemType.EMPTY)
				{
					//Add item to player's inventory
					GameObject.Find("Inventory").GetComponent<Inventory>().AddToInventory(chest.GetComponent<Chest>().Type);
					chest.GetComponent<Chest>().TakeItem();



					//Open a text box
					if ((chest.GetComponent<Chest>()).TextBox.isOpen())
					{
						(chest.GetComponent<Chest>()).TextBox.nextSlide();
					}
					else {
						(chest.GetComponent<Chest>()).TextBox.toggle();

						GameObject textbox = Resources.Load("TextBox") as GameObject;
						Instantiate(textbox, new Vector3(chest.transform.position.x + 0.75f, chest.transform.position.y + 1.4f, -3), Quaternion.identity);
						break;
					}
				}
			}
		}
	}


	/// <summary>
	/// Private method for dealing with NPC interaction.
	/// </summary>
	/// <returns>The nteraction.</returns>
	void NPCInteraction()
	{
		foreach (GameObject npc in world.Npcs)
		{
			if ((npc.GetComponent<NPC>()).isNextToPlayer())
			{
				//There is already a text box open
				if ((npc.GetComponent<NPC>()).TextBox.isOpen())
				{

					(npc.GetComponent<NPC>()).TextBox.nextSlide();

					//There are no text boxes open already
				}
				else {

					(npc.GetComponent<NPC>()).TextBox.toggle();

					GameObject textbox = Resources.Load("TextBox") as GameObject;
					Instantiate(textbox, new Vector3(npc.transform.position.x + 0.75f, npc.transform.position.y + 1.4f, -3), Quaternion.identity);
					break;

				}
			}
		}
	}

	/// <summary>
	/// Updates the direction.
	/// </summary>
	/// <returns>The direction.</returns>
	void UpdateDirection()
	{
		Projectile[] array = FindObjectsOfType<Projectile>();
		foreach (Projectile proj in array)
		{
			proj.GetComponent<Projectile>().GetDirectionFromPlayer();
		}
	}

	/// <summary>
	/// Updates the player direction based on the input.
	/// </summary>
	/// <returns>The player directions.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	void UpdatePlayerDirections(float x, float y)
	{
		if (x > 0)
		{
			this.GetComponent<SpriteRenderer>().sprite = DirectionSprites[3];
			Direction = Direction.East;
		}
		if (x < 0)
		{
			this.GetComponent<SpriteRenderer>().sprite = DirectionSprites[2];
			Direction = Direction.West;
		}
		if (y > 0)
		{
			this.GetComponent<SpriteRenderer>().sprite = DirectionSprites[1];
			Direction = Direction.North;
		}
		if (y < 0)
		{
			this.GetComponent<SpriteRenderer>().sprite = DirectionSprites[0];
			Direction = Direction.South;
		}
		if (x > 0 && y > 0)
		{
			this.GetComponent<SpriteRenderer>().sprite = DirectionSprites[6];
			Direction = Direction.NorthEast;
		}
		if (x < 0 && y > 0)
		{
			this.GetComponent<SpriteRenderer>().sprite = DirectionSprites[7];
			Direction = Direction.NorthWest;
		}
		if (x < 0 && y < 0)
		{
			this.GetComponent<SpriteRenderer>().sprite = DirectionSprites[4];
			Direction = Direction.SouthWest;
		}
		if (x > 0 && y < 0)
		{
			this.GetComponent<SpriteRenderer>().sprite = DirectionSprites[5];
			Direction = Direction.SouthEast;
		}
	}
}
