using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	KeyCode primaryUseKey = KeyCode.Z;
	KeyCode secondaryUseKey = KeyCode.X;
	KeyCode interactionKey = KeyCode.C;

	#region References For Simplicity

		/// <summary>
		/// The player's rigid body.
		/// </summary>
		public Rigidbody2D Rigid;

		/// <summary>
		/// The sprite render.
		/// </summary>
		public SpriteRenderer SpriteRender;

		/// <summary>
		/// The inventory.
		/// </summary>
		public Inventory inventory;

		/// <summary>
		/// The textbox.
		/// </summary>
		public GameObject textbox;
		TextMesh tbTextMesh;
		SmartText smartText;

		/// <summary>
		/// My box collider.
		/// </summary>
		public BoxCollider2D MyBoxCollider;

		/// <summary>
		/// The gm.
		/// </summary>
		public GameManagerScript GM;

	#endregion

	/// <summary>
	/// The world.
	/// </summary>
	public World world;

	/// <summary>
	/// The text box for the ladder.
	/// </summary>
	TextBox ladderTB;
	bool IsNextToLadder;

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
		SpriteRender = GetComponent<SpriteRenderer>();
		ladderTB = new TextBox();
		ladderTB.addText("Press \"c\" to use the ladder");
		textbox = Resources.Load("TextBox") as GameObject;
		tbTextMesh = textbox.GetComponentInChildren<TextMesh>();
		smartText = textbox.GetComponentInChildren<SmartText>();
		MyBoxCollider = GetComponent<BoxCollider2D>();
		GM = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
		inventory = this.gameObject.GetComponent<Inventory>();
	}

	void Update () {
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		float y = Input.GetAxis("Vertical") * Time.deltaTime * speed;

		//Update the player's sprite based on the button presse.
		UpdatePlayerDirections(x, y);


		//Only move the player when the inventroy is not open.
		if(!GM.Paused)
			Rigid.MovePosition(new Vector2(this.transform.position.x + x, this.transform.position.y + y));


		// Make sure to freeze the position of the player when paused
		if (GM.Paused) {
			Rigid.constraints = RigidbodyConstraints2D.FreezeAll;
		}
		else {
			Rigid.constraints = RigidbodyConstraints2D.None;
			Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
		}

		//Draw the appropriate line of text
		tbTextMesh.text = ladderTB.Text[ladderTB.CurrentSlide];
		smartText.OnTextChanged();
			

		//Check for other types of player input
		CheckInput();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.name == "Ladder(Clone)")
		{
			IsNextToLadder = true;
			Instantiate(textbox, new Vector3(transform.position.x + 0.75f, transform.position.y + 1.4f, -3), Quaternion.identity);
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.name == "Ladder(Clone)")
		{
			IsNextToLadder = false;
			ladderTB.setOpen(false);
			Object.Destroy(GameObject.Find("TextBox(Clone)"));
		}
	}


	/// <summary>
	/// Checks the input.
	/// </summary>
	/// <returns>The input.</returns>
	void CheckInput()
	{
		if (Input.GetKeyDown(interactionKey))
		{
			PickupItemsOffGround(interactionKey);

			if(IsNextToLadder) {
				GM.CurrentFloor++;
				SceneManager.LoadScene(GM.Underground[GM.CurrentFloor - 3]);
			}
			
			if (GM != null)
			{
				if (GM.Npcs.Length > 0)
					NPCInteraction();
				if (GM.Chests.Length > 0)
					CheckInteraction();
			}
		}

		if (Input.GetKeyDown(primaryUseKey))
		{
			if (inventory.Items[inventory.CurrentSlot].Type != ItemType.EMPTY)
			{
				inventory.Items[inventory.CurrentSlot].PrimaryUse();
			}
		}

		if (Input.GetKeyDown(secondaryUseKey))
		{
			if (inventory.Items[inventory.CurrentSlot].Type != ItemType.EMPTY)
			{
				inventory.Items[inventory.CurrentSlot].SecondaryUse();
			}
		}

		// Removes an item from the inventory.
		if (Input.GetKeyDown(KeyCode.V))
		{
			inventory.Items[inventory.CurrentSlot].Remove();
		}

		// Open the inventory
		if (Input.GetKeyDown(KeyCode.I))
		{
			inventory.Open = !inventory.Open;
		}

		// Pause the game
		if (Input.GetKeyDown(KeyCode.P))
		{
			GM.Paused = !GM.Paused;
		}
	}

	/// <summary>
	/// Pickup the items off ground.
	/// </summary>
	/// <returns>The items off ground.</returns>
	/// <param name="k">K.</param>
	void PickupItemsOffGround(KeyCode k)
	{
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("FloorItem"))
		{
			FloorItem fItem = obj.GetComponent<FloorItem>();
 
			if (fItem.IsNextToPlayer)
			{
				inventory.AddToInventory(new Item(fItem.itm, fItem.Quantity.Value));
				fItem.timer = 0;
			}
		}
	}

	/// <summary>
	/// Checks for interaction with things other than NPCs
	/// </summary>
	/// <returns>The interaction.</returns>
	void CheckInteraction()
	{
		foreach (GameObject chest in GM.Chests)
		{
			Chest chestComp = chest.GetComponent<Chest>();

			if (chestComp.isNextToPlayer())
			{
				if (chestComp.Type != ItemType.EMPTY)
				{
					//Add item to player's inventory
					inventory.AddToInventory(new Item(chestComp.Type, chestComp.Quantity));
					chestComp.TakeItem();


					//Open a text box
					if (chestComp.TextBox.isOpen())
					{
						chestComp.TextBox.nextSlide();
					}
					else {
						chestComp.TextBox.toggle();

						Instantiate(textbox, new Vector3(transform.position.x + 0.75f, transform.position.y + 1.4f, -3), Quaternion.identity);
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
		foreach (GameObject npc in GM.Npcs)
		{
			NPC npcComp = npc.GetComponent<NPC>();

			if (npcComp.isNextToPlayer())
			{
				//There is already a text box open
				if (npcComp.TextBox.isOpen())
				{

					npcComp.TextBox.nextSlide();

					//There are no text boxes open already
				}
				else {

					npcComp.TextBox.toggle();

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
			proj.GetComponent<Projectile>().GetDirectionFromLauncher();
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
			SpriteRender.sprite = DirectionSprites[3];
			Direction = Direction.East;
		}
		if (x < 0)
		{
			SpriteRender.sprite = DirectionSprites[2];
			Direction = Direction.West;
		}
		if (y > 0)
		{
			SpriteRender.sprite = DirectionSprites[1];
			Direction = Direction.North;
		}
		if (y < 0)
		{
			SpriteRender.sprite = DirectionSprites[0];
			Direction = Direction.South;
		}
		if (x > 0 && y > 0)
		{
			SpriteRender.sprite = DirectionSprites[6];
			Direction = Direction.NorthEast;
		}
		if (x < 0 && y > 0)
		{
			SpriteRender.sprite = DirectionSprites[7];
			Direction = Direction.NorthWest;
		}
		if (x < 0 && y < 0)
		{
			SpriteRender.sprite = DirectionSprites[4];
			Direction = Direction.SouthWest;
		}
		if (x > 0 && y < 0)
		{
			SpriteRender.sprite = DirectionSprites[5];
			Direction = Direction.SouthEast;
		}
	}
}
