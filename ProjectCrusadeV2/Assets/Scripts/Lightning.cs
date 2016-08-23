using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour {

	float Delay;

	/// <summary>
	/// The camera to follow.
	/// </summary>
	Camera c;

	/// <summary>
	/// The renderer.
	/// </summary>
	#pragma warning disable
	SpriteRenderer renderer;

	/// <summary>
	/// The lightning stike delay1.
	/// </summary>
	public FloatRange LightningStikeDelay1;





	void Start () {
		c = GameObject.Find("Main Camera").GetComponent<Camera>();
		renderer = GetComponent<SpriteRenderer>();

		LightningStikeDelay1.Value = LightningStikeDelay1.Random;

		Delay = LightningStikeDelay1.Value;
	}
	
	void Update () {
		this.gameObject.transform.position = new Vector3(c.transform.position.x,
		                                                 c.transform.position.y,
		                                                 this.gameObject.transform.position.z);



		// Simulate a lightning strike every so often.

		if (LightningStikeDelay1.Value >= Delay)
		{
			renderer.color = new Color(0, 0, 0, 0.35f);
		}
		else if(LightningStikeDelay1.Value >= 0 && LightningStikeDelay1.Value < 5f) {
			renderer.color = new Color(255, 255, 255, 0.15f);
		}

		LightningStikeDelay1.Value -= 1;

		if (LightningStikeDelay1.Value <= 0)
		{
			LightningStikeDelay1.Value = Delay;
		}

	}
}
