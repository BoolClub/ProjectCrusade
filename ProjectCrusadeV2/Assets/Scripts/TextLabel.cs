using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class TextLabel : MonoBehaviour
{
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



	public void Start()
	{
		TextMesh = GetComponent(typeof(TextMesh)) as TextMesh;
		timer = 40;
	}

	void Update()
	{
		if (!Text.Equals(""))
		{
			TextMesh.text = Text;
			timer -= 1f;
			if (timer <= 0)
			{
				Text = "";
				TextMesh.text = "";
				this.enabled = false;
				timer = 40f;
			}
		}
	}

}
