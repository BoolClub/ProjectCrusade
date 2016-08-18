using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Door : MonoBehaviour {

	/// <summary>
	/// The destination.
	/// </summary>
	public string Destination;

	/// <summary>
	/// The position.
	/// </summary>
	public Vector2 Position;

	/// <summary>
	/// The object that manages the entire game.
	/// </summary>
	public GameObject GameManager;

	BoxCollider2D MyBoxColl;


	void Start()
	{
		//Make sure that all the doors have access to the game manager
		GameManager = Resources.Load("GameManager") as GameObject;
		MyBoxColl = GetComponent<BoxCollider2D>() as BoxCollider2D;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Player") && MyBoxColl.IsTouching(other) )
		{
			//Debug.Log(Destination);
			SceneManager.LoadScene(Destination);
		}
	}
}
