using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoodGuyNPC : MonoBehaviour
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
			npcScript.TextBox.setOpen(false);
			Destroy(GameObject.Find("TextBox(Clone)"));

			// Start transition to next scene.

			if (GM.Transitions.Finished == true && GM.Transitions.Type == FadeType.Fade_In)
			{
				GM.Transitions.Reset();
				GM.Transitions.Type = FadeType.Fade_Out;
				GM.Transitions.PlayTransition = true;
				GM.Transitions.BeginFade(-1);
			}
		}

		if (GM.Transitions.Finished == true && GM.Transitions.Type == FadeType.Fade_Out)
		{
			SceneManager.LoadScene("Underground_Real_Boss");
		}
	}
}

