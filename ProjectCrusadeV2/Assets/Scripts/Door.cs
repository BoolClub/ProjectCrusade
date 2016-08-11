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



	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			//Debug.Log(Destination);
			SceneManager.LoadScene(Destination);
		}
	}
}
