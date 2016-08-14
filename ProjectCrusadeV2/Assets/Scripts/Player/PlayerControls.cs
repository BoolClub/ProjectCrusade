using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	/// <summary>
	/// The world.
	/// </summary>
	public World world;

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
	}

	void Update () {
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		float y = Input.GetAxis("Vertical") * Time.deltaTime * speed;

		//Update the player's sprite based on the button presse.
		if (x > 0) this.GetComponent<SpriteRenderer>().sprite = DirectionSprites[3];
		if (x < 0) this.GetComponent<SpriteRenderer>().sprite = DirectionSprites[2];
		if (y > 0) this.GetComponent<SpriteRenderer>().sprite = DirectionSprites[1];
		if (y < 0) this.GetComponent<SpriteRenderer>().sprite = DirectionSprites[0];


		//Only move the player when the inventroy is not open.
		if (GameObject.Find("Inventory").GetComponent<Inventory>().Open == false)
			transform.Translate(x, y, 0);


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
			//Primary use item
		}
		if (Input.GetKeyDown(secondaryUseKey))
		{
			//Secondary use item
		}

		//Open the inventory
		if (Input.GetKeyDown(KeyCode.I))
		{
			GameObject.Find("Inventory").GetComponent<Inventory>().Open = !GameObject.Find("Inventory").GetComponent<Inventory>().Open;
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
}
