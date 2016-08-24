using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	TransitionManager TM;

	bool start,htp;


	// Use this for initialization
	void Start () {
		TM = GameObject.Find("TransitionManager").GetComponent<TransitionManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (TM.Finished && TM.Type == FadeType.Fade_Out && start == true)
		{
			SceneManager.LoadScene("OverWorld");
		}
		if (TM.Finished && TM.Type == FadeType.Fade_Out && htp == true)
		{
			SceneManager.LoadScene("HowToPlay");
		}
	}


	public void StartGame()
	{
		start = true;
		htp = false;
		TM.Reset();
		TM.Type = FadeType.Fade_Out;
		TM.PlayTransition = true;
		TM.BeginFade(-1);
	}

	public void HowToPlay()
	{
		start = false;
		htp = true;
		TM.Reset();
		TM.Type = FadeType.Fade_Out;
		TM.PlayTransition = true;
		TM.BeginFade(-1);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
