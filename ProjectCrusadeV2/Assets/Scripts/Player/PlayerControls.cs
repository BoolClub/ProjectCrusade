using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControls : MonoBehaviour {

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
		/// The textbox.
		/// </summary>
		public GameObject textbox;

		/// <summary>
		/// My box collider.
		/// </summary>
		public BoxCollider2D MyBoxCollider;

		/// <summary>
		/// The gm.
		/// </summary>
		GameManagerScript GM;

	#endregion

	/// <summary>
	/// The world.
	/// </summary>
	public World world;

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
		textbox = Resources.Load("TextBox") as GameObject;
		MyBoxCollider = GetComponent<BoxCollider2D>();
		GM = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
	}

	void Update () {
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		float y = Input.GetAxis("Vertical") * Time.deltaTime * speed;

		//Update the player's sprite based on the button presse.
		UpdatePlayerDirections(x, y);

		//Only move the player when the inventroy is not open.
			Rigid.MovePosition(new Vector2(this.transform.position.x + x, this.transform.position.y + y));


		//Check for other types of player input
		CheckInput();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.name == "Ladder(Clone)")
		{
			GM.CurrentFloor++;
			SceneManager.LoadScene(GM.Underground[GM.CurrentFloor - 3]);
		}
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
			if (world != null)
			{
				if (world.Npcs.Length > 0)
					NPCInteraction();
				if (world.Chests.Length > 0)
					CheckInteraction();
			}
		}
		if (Input.GetKeyDown(primaryUseKey))
		{
		}
		if (Input.GetKeyDown(secondaryUseKey))
		{
		}

		//Open the inventory
		if (Input.GetKeyDown(KeyCode.I))
		{
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
					chest.GetComponent<Chest>().TakeItem();



					//Open a text box
					if ((chest.GetComponent<Chest>()).TextBox.isOpen())
					{
						(chest.GetComponent<Chest>()).TextBox.nextSlide();
					}
					else {
						(chest.GetComponent<Chest>()).TextBox.toggle();

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
