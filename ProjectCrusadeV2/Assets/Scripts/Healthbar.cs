using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Healthbar : MonoBehaviour {

	/// <summary>
	/// The current amount of health
	/// </summary>
	[Range(0,100)]
	public float health = 100f;


	void Update()
	{
		//Always make sure the fill bar represents the health amount
		GetComponent<Image>().fillAmount = health / 100;

		//Update the color of the health bar.
		if(health > 60) 
		{
			GetComponent<Image>().color = Color.green;
		} 
		else if (health <= 60 && health > 30)
		{
			GetComponent<Image>().color = Color.yellow;
		}
		else {
			GetComponent<Image>().color = Color.red;
		}
	}

	/// <summary>
	/// Takes health away from the player.
	/// </summary>
	/// <returns>The hp.</returns>
	/// <param name="amount">Amount.</param>
	public void DecreaseHP(float amount)
	{
		health -= amount;
	}
}
