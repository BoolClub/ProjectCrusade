using UnityEngine;
using System.Collections;

public class PriestNPC_2 : MonoBehaviour {

	/// <summary>
	/// The gm.
	/// </summary>
	GameManagerScript GM;

	/// <summary>
	/// The npc script.
	/// </summary>
	NPC npcScript;

	/// <summary>
	/// The good npc boss
	/// </summary>
	GameObject badNPCBoss;



	void Start () {
		GM = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
		npcScript = this.gameObject.GetComponent<NPC>();
		badNPCBoss = Resources.Load("Enemies/BadNPCBoss") as GameObject;
	}
	
	void Update () {

		if (npcScript.TextBox.onLast())
		{
			// Instantiate the boss
			if (!npcScript.TextBox.getTextSlides()[0].Equals("Hm... It's seems you have beaten me..."))
			{
				Instantiate(badNPCBoss, transform.position, Quaternion.identity);
			}
			// Poof away animation before he gets destroyed
			Destroy(GameObject.Find("TextBox(Clone)"));

			if (!npcScript.TextBox.getTextSlides()[0].Equals("Hm... It's seems you have beaten me..."))
			{
				GM.PriestNPC.SetActive(false);
			}

			if (npcScript.TextBox.getTextSlides()[0].Equals("Hm... It's seems you have beaten me..."))
			{
				GM.goodNPC.SetActive(true);
				GM.goodNPC.AddComponent<GoodGuyNPC2>();

				GM.goodNPC.GetComponent<NPC>().TextBox.clear();
				GM.goodNPC.GetComponent<NPC>().TextBox.addText("You did it! You saved me, our town, and the whole world! You are forever in our debt!");
				GM.goodNPC.GetComponent<NPC>().TextBox.addText("And now that we have the sacred key back, we can all live in peace once again");
				GM.goodNPC.GetComponent<NPC>().TextBox.addText("The storm has passed, and it's all thanks to you!");
				GM.goodNPC.GetComponent<NPC>().TextBox.addText("");

				GM.Npcs.Remove(this.gameObject);
				Destroy(this.gameObject);
			}
		}

	}
}
