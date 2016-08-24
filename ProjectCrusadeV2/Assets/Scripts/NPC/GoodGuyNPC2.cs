using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoodGuyNPC2 : MonoBehaviour
{
	#region References for Simplicity

		GameManagerScript GM;

	#endregion

	/// <summary>
	/// The npc script.
	/// </summary>
	NPC npcScript;


	void Start()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
		npcScript = this.gameObject.GetComponent<NPC>();
	}


	void Update()
	{
		if (npcScript.TextBox.onLast())
		{
			GM.Transitions.Reset();
			GM.Transitions.Type = FadeType.Fade_Out;
			GM.Transitions.PlayTransition = true;
			GM.Transitions.BeginFade(-1);

			Destroy(GameObject.Find("TextBox(Clone)"));
		}
	}
}

