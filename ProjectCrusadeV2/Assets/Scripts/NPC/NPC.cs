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
		//Draw the appropriate line of text
		if (TextBox.isOpen())
		{
			(GameObject.Find("TextBox(Clone)").GetComponentInChildren<TextMesh>()).text = TextBox.Text[TextBox.CurrentSlide];
		}
	}

	/// <summary>
	/// Returns whether or not this NPC is near the player.
	/// </summary>
	/// <returns>The next to player.</returns>
	public bool isNextToPlayer() { return NextToPlayer; }


	/// <summary>
	/// Ons the trigger enter2 d.
	/// </summary>
	/// <returns>The trigger enter2 d.</returns>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			NextToPlayer = true;
		}
	}


	/// <summary>
	/// Ons the trigger exit2 d.
	/// </summary>
	/// <returns>The trigger exit2 d.</returns>
	/// <param name="other">Other.</param>
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			NextToPlayer = false;
		}
	}
}
