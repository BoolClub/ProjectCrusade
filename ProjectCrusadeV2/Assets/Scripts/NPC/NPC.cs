using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	/// <summary>
	/// The text box for this NPC.
	/// </summary>
	public TextBox TextBox;

	/// <summary>
	/// This is what the NPC will say to the player.
	/// </summary>
	public string[] Speech;

	/// <summary>
	/// Whether or not this NPC is next to the player
	/// </summary>
	public bool NextToPlayer;


	// Use this for initialization
	void Start () {
		TextBox = new TextBox();

		foreach (string s in Speech)
		{
			TextBox.addText(s);
		}


		//Make a bigger box collider
		(GetComponent(typeof(BoxCollider2D)) as BoxCollider2D).size = new Vector2(0.46f, 0.46f);
	}


	void Update()
	{
		//Check for NPC-Player collisions.
		CheckCollision();

		//Draw the appropriate line of text
		if (GameObject.Find("TextBox(Clone)") != null && TextBox.isOpen())
		{
			(GameObject.Find("TextBox(Clone)").GetComponentInChildren<TextMesh>()).text = TextBox.Text[TextBox.CurrentSlide];
			(GameObject.Find("TextBox(Clone)").GetComponentInChildren<SmartText>()).OnTextChanged();
		}
	}

	/// <summary>
	/// Returns whether or not this NPC is near the player.
	/// </summary>
	/// <returns>The next to player.</returns>
	public bool isNextToPlayer() { return NextToPlayer; }

	/// <summary>
	/// Checks for collision between this NPC and the player.
	/// </summary>
	/// <returns>The collision.</returns>
	public void CheckCollision() { 
		if (GetComponent<BoxCollider2D>().IsTouching(GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>()))
		{
			NextToPlayer = true;
		}
		else {
			NextToPlayer = false;
			if (TextBox.isOpen())
			{
				Object.Destroy(GameObject.Find("TextBox(Clone)"));
				TextBox.toggle();
			}
		}
	}
}
