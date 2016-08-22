using UnityEngine;
using System.Collections;

public class PriestNPC_1 : MonoBehaviour {
	/// <summary>
	/// The npc script.
	/// </summary>
	NPC npcScript;


	void Start () {
		npcScript = this.gameObject.GetComponent<NPC>();
	}
	
	void Update () {

		if (npcScript.TextBox.onLast())
		{
			// Poof away animation before he gets destroyed

			Destroy(this.gameObject);
			Destroy(GameObject.Find("TextBox(Clone)"));
		}

	}
}
