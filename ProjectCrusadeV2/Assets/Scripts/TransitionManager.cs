using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public enum FadeType { Fade_In, Fade_Out }

public class TransitionManager : MonoBehaviour {

	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.8f;
	public FadeType Type;
	public bool PlayTransition;
	public bool Finished;

	private int drawDepth = -4000;
	private float alpha = 1.0f;
	private int fadeDir = -1;


	public void OnGUI()
	{
		if (PlayTransition == true)
		{
			alpha += fadeDir * fadeSpeed * Time.deltaTime;

			alpha = Mathf.Clamp01(alpha);

			if (Type == FadeType.Fade_In)
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
			else if (Type == FadeType.Fade_Out)
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1 - alpha);

			GUI.depth = drawDepth;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
		}
	}

	public float BeginFade(int direction)
	{
		fadeDir = direction;
		return (fadeSpeed);
	}

	public void Reset()
	{
		Finished = false;
		PlayTransition = false;
		alpha = 1.0f;
	}

	void Start()
	{
		BeginFade(-1);
	}

	void Update()
	{
		if (Type == FadeType.Fade_In)
		{
			if (alpha >= 1)
			{
				Finished = true;
			}
		}
		if (Type == FadeType.Fade_Out)
		{
			if (alpha <= 0)
			{
				Finished = true;
			}
		}
	}
}
