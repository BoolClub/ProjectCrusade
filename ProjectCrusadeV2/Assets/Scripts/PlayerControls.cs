using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	/// <summary>
	/// The speed of the player.
	/// </summary>
	public float speed;

	/// <summary>
	/// The player's position
	/// </summary>
	public Vector3 Position;


	void Start () {
		transform.position = Position;
	}

	void Update () {
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		float y = Input.GetAxis("Vertical") * Time.deltaTime * speed;

		transform.Translate(x,y,0);
	}
}
