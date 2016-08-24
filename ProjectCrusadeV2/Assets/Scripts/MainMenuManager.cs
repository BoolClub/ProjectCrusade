using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void StartGame()
	{
		SceneManager.LoadScene("OverWorld");
	}

	public void HowToPlay()
	{
		SceneManager.LoadScene("HowToPlay");
	}

	public void Quit()
	{
		Application.Quit();
	}
}
