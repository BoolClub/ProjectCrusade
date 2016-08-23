using UnityEngine;
using System.Collections;

public class PriestNPC_2 : MonoBehaviour {
	
	/// <summary>
	/// The npc script.
	/// </summary>
	NPC npcScript;

	/// <summary>
	/// The good npc.
	/// </summary>
	GameObject badNPC;

	/// <summary>
	/// The good npc boss
	/// </summary>
	GameObject badNPCBoss;



	void Start () {
		npcScript = this.gameObject.GetComponent<NPC>();
		badNPC = GameObject.Find("PriestNPC");
		badNPCBoss = Resources.Load("Enemies/BadNPCBoss") as GameObject;
	}
	
	void Update () {

		if (npcScript.TextBox.onLast())
		{
			// Instantiate the boss
			Instantiate(badNPCBoss, badNPC.transform.position, Quaternion.identity);


			// Play animation of good npc transforming into the boss version, then destroy it
			Destroy(badNPC);


			// Poof away animation before he gets destroyed
			Destroy(GameObject.Find("TextBox(Clone)"));
			Destroy(this.gameObject);
		}

	}
}
