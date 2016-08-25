using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class GameManagerScript : MonoBehaviour {

	/// <summary>
	/// The transitions.
	/// </summary>
	public TransitionManager Transitions;

	/// <summary>
	/// The player.
	/// </summary>
	PlayerControls Player;
	public bool UsedStaff;

	/// <summary>
	/// The item sprites.
	/// </summary>
	public static List<Item> Items = new List<Item>();

	/// <summary>
	/// An array of sprites for each item.
	/// </summary>
	public Sprite[] ItemSprites;

	/// <summary>
	/// A list of all of the different types of enemies that could be spawned.
	/// </summary>
	public GameObject[] Enemies;

	/// <summary>
	/// The current floor that the player is on by its index in the array above.
	/// </summary>
	public static int CurrentFloor = 0;

	/// <summary>
	/// Floor items holder.
	/// </summary>
	public GameObject FloorItemsHolder;

	/// <summary>
	/// All of the NPCs in the game world.
	/// </summary>
	public List<GameObject> Npcs;

	/// <summary>
	/// The treasure chests that are in the game world.
	/// </summary>
	public GameObject[] Chests;

	/// <summary>
	/// Whether or not the game is paused.
	/// </summary>
	public bool Paused;

	/// <summary>
	/// The pause object
	/// </summary>
	public GameObject PauseOverlay;

	/// <summary>
	/// The priest npc.
	/// </summary>
	public GameObject PriestNPC;

	/// <summary>
	/// The good npc.
	/// </summary>
	public GameObject goodNPC;



	// Use this for initialization
	public void Start () {
		Transitions.Type = FadeType.Fade_In;
		Transitions.PlayTransition = true;
		Healthbar.Health = 100;
		Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();

		if (SceneManager.GetActiveScene().buildIndex == 15)
		{
			PriestNPC = GameObject.Find("PriestNPC");
			goodNPC = GameObject.Find("GoodGuyNPC");
			Npcs.Add(goodNPC);
			goodNPC.SetActive(false);
		}

		for (int i = 0; i < 40; i++)
			Items.Add(new Item(ItemType.EMPTY));
	}

 	void Awake()
	{
		if (Items.Count < 40)
		{
			for (int i = 0; i < 40; i++)
				Items.Add(new Item(ItemType.EMPTY));
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("FloorItem"))
		{
			obj.transform.SetParent(FloorItemsHolder.transform);
		}


		PauseOverlay.transform.position = new Vector3(Player.gameObject.transform.position.x,
		                                              Player.gameObject.transform.position.y,
													  PauseOverlay.transform.position.z);

		if (Paused == true)
		{
			PauseOverlay.SetActive(true);
			PauseOverlay.transform.GetChild(0).gameObject.SetActive(true);
		}
		else {
			PauseOverlay.SetActive(false);
			PauseOverlay.transform.GetChild(0).gameObject.SetActive(false);
		}


		// Moving to church scene after beating the first boss.
		if (Transitions.Finished && Transitions.Type == FadeType.Fade_Out && Player.inventory.Contains(ItemType.Staff))
		{
			if (SceneManager.GetActiveScene().buildIndex == 14)
			{
				SceneManager.LoadScene(15);
			}
		}

		if (Transitions.Finished && Transitions.Type == FadeType.Fade_Out)
		{
			if (SceneManager.GetActiveScene().buildIndex == 15)
			{
				SceneManager.LoadScene(16);
			}
		}

		if (Transitions.Finished && Transitions.Type == FadeType.Fade_Out)
		{
			if (Healthbar.Health <= 0)
			{
				SceneManager.LoadScene("PlayerLoseScene");
			}
		}
	}

}
