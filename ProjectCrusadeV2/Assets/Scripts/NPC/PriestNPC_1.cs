using UnityEngine;
using System.Collections;

public class PriestNPC_1 : MonoBehaviour {
	
	/// <summary>
	/// The npc script.
	/// </summary>
	NPC npcScript;

	/// <summary>
	/// The good npc.
	/// </summary>
	GameObject goodNPC;

	/// <summary>
	/// The good npc boss
	/// </summary>
	GameObject goodNPCBoss;



	void Start () {
		npcScript = this.gameObject.GetComponent<NPC>();
		goodNPC = GameObject.Find("GoodGuyNPC");
		goodNPCBoss = Resources.Load("Enemies/GoodNPCBoss") as GameObject;
	}
	
	void Update () {

		if (npcScript.TextBox.onLast())
		{
			// Instantiate the boss
			Instantiate(goodNPCBoss, goodNPC.transform.position, Quaternion.identity);


			// Play animation of good npc transforming into the boss version, then destroy it
			Destroy(goodNPC);


			// Poof away animation before he gets destroyed
			Destroy(GameObject.Find("TextBox(Clone)"));
			Destroy(this.gameObject);
		}

	}
}
