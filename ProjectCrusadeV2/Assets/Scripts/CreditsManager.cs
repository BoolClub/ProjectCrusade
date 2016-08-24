using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsManager : MonoBehaviour {

	TransitionManager TM;

	// Use this for initialization
	void Start () {
		TM = GameObject.Find("TransitionManager").GetComponent<TransitionManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (TM.Finished && TM.Type == FadeType.Fade_Out)
		{
			SceneManager.LoadScene("MainMenu");
		}
	}


	public void LoadMainMenu()
	{
		TM.Reset();
		TM.Type = FadeType.Fade_Out;
		TM.PlayTransition = true;
		TM.BeginFade(-1);
	}
}
