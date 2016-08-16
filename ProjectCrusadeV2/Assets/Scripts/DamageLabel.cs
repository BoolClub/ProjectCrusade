using UnityEngine;
using System.Collections;

/// <summary>
/// This is waht attaches to the damage label prefab. It displays the amount of damage that is done and then 
/// destroys the game object after a certain amount of time.
/// </summary>
[RequireComponent(typeof(TextMesh))]
public class DamageLabel : MonoBehaviour {

	///The string to display on this label
	public string Text = "0";

	/// <summary>
	/// The text mesh.
	/// </summary>
 	TextMesh TextMesh;

	/// <summary>
	/// An internal timer that ticks so that this game object can destroy itself after a few seconds.
	/// </summary>
	float timer;



	void Start()
	{
		TextMesh = GetComponent(typeof(TextMesh)) as TextMesh;
		timer = 25;
	}

	void Update()
	{
		TextMesh.text = Text;

		timer -= 1f;
		gameObject.transform.Translate(0.05f, 0.05f, 0);

		if (timer <= 0)
		{
			Destroy(this.gameObject);
		}
	}

}
