using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	/// <summary>
	/// The speed of the player.
	/// </summary>
	public float speed;


	void Start () {
		
	}

	void Update () {
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		float y = Input.GetAxis("Vertical") * Time.deltaTime * speed;

		transform.Translate(x,y,0);
	}
}
