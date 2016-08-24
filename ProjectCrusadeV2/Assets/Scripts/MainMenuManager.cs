using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	TransitionManager TM;

	bool start,htp,back;


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
		if (TM.Finished && TM.Type == FadeType.Fade_Out && back == true)
		{
			SceneManager.LoadScene("MainMenu");
		}
	}


	public void StartGame()
	{
		start = true;
		htp = false;
		back = false;
		TM.Reset();
		TM.Type = FadeType.Fade_Out;
		TM.PlayTransition = true;
		TM.BeginFade(-1);
	}

	public void HowToPlay()
	{
		start = false;
		htp = true;
		back = false;
		TM.Reset();
		TM.Type = FadeType.Fade_Out;
		TM.PlayTransition = true;
		TM.BeginFade(-1);
	}

	public void BackToMainMenu()
	{
		start = false;
		htp = false;
		back = true;
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
