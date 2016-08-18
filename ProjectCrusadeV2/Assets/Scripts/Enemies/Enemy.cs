using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	#region Refrences For Simplicity

		/// <summary>
		/// My box collider.
		/// </summary>
		BoxCollider2D MyBoxCollider;

		/// <summary>
		/// The player.
		/// </summary>
		PlayerControls Player;

		/// <summary>
		/// The health bar.
		/// </summary>
		Healthbar TheHealthBar;

	#endregion


	/// <summary>
	/// The amount of damage that this enemy will do to the player.
	/// It is a random number between two values.
	/// </summary>
	public FloatRange Damage;
	float damageDelay = 40f;

	/// <summary>
	/// The amount of health that this enemy has.
	/// This is also a random number so that the game isn't so static; enemies of the same type 
	/// can still have varying amounts of health.
	/// </summary>
	public FloatRange Health;

	/// <summary>
	/// The enemy sprite.
	/// </summary>
	public Sprite EnemySprite;

	/// <summary>
	/// Whether or not the enemy is burned. If it is, it should lose health every few seconds.
	/// </summary>
	public bool Burned;
	float burnDelay = 4f;

	/// <summary>
	/// Whether or not the enemy is stunned. If it is, then it should not be able to move for while.
	/// </summary>
	public bool Stunned;
	public float stunTime = 5f;


	// Use this for initialization
	void Start () {
		Damage.Value = Damage.Random;
		Health.Value = Health.Random;
		MyBoxCollider = GetComponent<BoxCollider2D>();
		Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
		TheHealthBar = GameObject.Find("HPBarFill").GetComponent<Healthbar>();
	}
	
	// Update is called once per frame
	void Update () {
		//Constantly change the amount of damage the enemy will do in order to keep it random
		Damage.Value = Damage.Random;

		// Check if touching player
			HurtPlayerOnContact();


		if (Burned)
			BurnDamage();

		if (Stunned)
			StunEffects();

		if (Health.Value <= 0)
			Destroy(this.gameObject);
	}

	/// <summary>
	/// Decreases the health from the enemy.
	/// </summary>
	/// <returns>The health.</returns>
	/// <param name="damage">Damage.</param>
	public void DecreaseHealth(float damage)
	{
		float criticalHitRate = new FloatRange(0, 100).Random;
		bool criticalHit = (!(criticalHitRate % 2).Equals(0) && criticalHitRate > 30 && criticalHitRate < 50) ? true : false;

		if (criticalHit)
			Health.Value -= damage * 1.5f;
		else
			Health.Value -= damage;
		

		if (criticalHit)
			GameObject.Find("DamageLabel(Clone)").GetComponent<TextMesh>().color = Color.yellow;
	}

	/// <summary>
	/// Hurts the player on contact.
	/// </summary>
	/// <returns>The player on contac.</returns>
	void HurtPlayerOnContact()
	{
		if (MyBoxCollider.IsTouching(Player.MyBoxCollider))
		{
			damageDelay -= 1f;

			if (damageDelay <= 0)
			{
				TheHealthBar.DecreaseHP(Damage.Value);
				damageDelay = 40f;
			}
		}
	}

	/// <summary>
	/// Implements the effects of burning the enemy.
	/// </summary>
	/// <returns>The damage.</returns>
	void BurnDamage()
	{
		if (burnDelay.Equals(4f))
			DecreaseHealth(4f);

		burnDelay -= 0.05f;

		if (burnDelay <= 0)
			burnDelay = 4f;
	}

	/// <summary>
	/// Implements the effects of stunning the enemy.
	/// </summary>
	/// <returns>The effects.</returns>
	void StunEffects()
	{
		if (stunTime > 0 && Stunned == true)
		{
			GetComponent<SpriteRenderer>().color = Color.yellow;
		}

		stunTime -= 0.02f;

		if (stunTime <= 0)
		{
			stunTime = 5f;
			Stunned = false;
			GetComponent<SpriteRenderer>().color = Color.white;
		}
	}
}
