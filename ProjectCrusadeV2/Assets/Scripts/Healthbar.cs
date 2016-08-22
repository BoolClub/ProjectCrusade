using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Healthbar : MonoBehaviour {

	/// <summary>
	/// The current amount of health
	/// </summary>
	[Range(0,100)]
	public static float Health = 100f;

	/// <summary>
	/// The hp image.
	/// </summary>
	Image hpImage;


	void Start()
	{
		hpImage = GetComponent<Image>();
	}


	void Update()
	{
		//Always make sure the fill bar represents the health amount
		hpImage.fillAmount = Health / 100;

		//Update the color of the health bar.
		if(Health > 60) 
		{
			hpImage.color = Color.green;
		} 
		else if (Health <= 60 && Health > 30)
		{
			hpImage.color = Color.yellow;
		}
		else {
			hpImage.color = Color.red;
		}
	}

	/// <summary>
	/// Takes health away from the player.
	/// </summary>
	/// <returns>The hp.</returns>
	/// <param name="amount">Amount.</param>
	public static void DecreaseHP(float amount)
	{
		Health -= amount;
	}

	/// <summary>
	/// Adds to the hp.
	/// </summary>
	/// <returns>The hp.</returns>
	/// <param name="amount">Amount.</param>
	public static void AddHP(float amount)
	{
		if (Health + amount <= 100)
		{
			Health += amount;
		}
		else {
			Health = 100;
		}
	}
}
