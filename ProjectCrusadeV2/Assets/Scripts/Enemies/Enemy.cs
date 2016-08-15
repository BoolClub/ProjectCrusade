using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	/// <summary>
	/// The amount of damage that this enemy will do to the player.
	/// It is a random number between two values.
	/// </summary>
	public FloatRange Damage;
	public float damageValue;

	/// <summary>
	/// The amount of health that this enemy has.
	/// This is also a random number so that the game isn't so static; enemies of the same type 
	/// can still have varying amounts of health.
	/// </summary>
	public FloatRange Health;
	public float healthValue;


	/// <summary>
	/// The enemy sprite.
	/// </summary>
	public Sprite EnemySprite;




	// Use this for initialization
	void Start () {
		Damage.Value = Damage.Random;
		Health.Value = Health.Random;
		healthValue = Health.Value;
		damageValue = Damage.Value;
	}
	
	// Update is called once per frame
	void Update () {
		//Constantly change the amount of damage the enemy will do in order to keep it random
		Damage.Value = Damage.Random;
		damageValue = Damage.Value;
	}
}
