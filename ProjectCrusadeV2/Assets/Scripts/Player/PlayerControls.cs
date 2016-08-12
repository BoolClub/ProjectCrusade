using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	/// <summary>
	/// The world.
	/// </summary>
	public World world;

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
		//You can only move when the inventory is not open.
		if (GameObject.Find("Inventory").GetComponent<Inventory>().Open == false)
		{
			float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
			float y = Input.GetAxis("Vertical") * Time.deltaTime * speed;

			transform.Translate(x, y, 0);
		}

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
				if (chest.GetComponent<Chest>().Item != ItemType.EMPTY)
				{
					//Add item to player's inventory
					chest.GetComponent<Chest>().TakeItem();
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
					Instantiate(textbox, new Vector3(npc.transform.position.x + 0.75f, npc.transform.position.y + 1.4f, 0), Quaternion.identity);
					break;

				}
			}
		}
	}
}
