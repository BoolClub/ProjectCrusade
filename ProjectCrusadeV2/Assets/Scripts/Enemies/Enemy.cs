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
		/// The this renderer.
		/// </summary>
		SpriteRenderer ThisRenderer;

		/// <summary>
		/// The rigid.
		/// </summary>
		Rigidbody2D Rigid;

	#endregion


	/// <summary>
	/// Whether or not this enemy is next to the player.
	/// </summary>
	public bool IsNextToPlayer;

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
	[HideInInspector]
	public bool Burned;
	float burnDelay = 4f;

	/// <summary>
	/// Whether or not the enemy is stunned. If it is, then it should not be able to move for while.
	/// </summary>
	[HideInInspector]
	public bool Stunned;
	[HideInInspector]
	public float stunTime = 5f;

	/// <summary>
	/// Whether or not the enemy is frozen.
	/// </summary>
	public bool Frozen;
	float frozenTime = 500f;

	/// <summary>
	/// Whether or not the enemy is poisoned.
	/// </summary>
	public bool Poisoned;
	float poisonDelay = 4f;

	/// <summary>
	/// The item drop percentage.
	/// </summary>
	[Range(0,100)]
	public float ItemDropPercentage = 50f;

	/// <summary>
	/// The list of items that this enemy could potentially drop.
	/// </summary>
	public GameObject[] ItemDropPrefabs;



	// Use this for initialization
	void Start () {
		Damage.Value = Damage.Random;
		Health.Value = Health.Random;
		MyBoxCollider = GetComponent<BoxCollider2D>();
		Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
		ThisRenderer = GetComponent<SpriteRenderer>();
		Rigid = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		//Constantly change the amount of damage the enemy will do in order to keep it random
		Damage.Value = Damage.Random;

		// Check if touching player
		if(!Player.GM.Paused)
			HurtPlayerOnContact();


		if (Rigid != null)
		{
			if (Player.GM.Paused)
			{
				Rigid.constraints = RigidbodyConstraints2D.FreezeAll;
			}
			else {
				Rigid.constraints = RigidbodyConstraints2D.None;
				Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
			}
		}


		if (Burned)
		{
			ThisRenderer.color = Color.red;
			BurnDamage();
		}
		else {
			ThisRenderer.color = Color.white;
		}


		if (Stunned)
			StunEffects();


		if (Frozen)
			FreezeEffects();


		if (Poisoned)
		{
			ThisRenderer.color = Color.magenta;
			PoisonEffects();
		}
		else {
			ThisRenderer.color = Color.white;
		}

		if (Health.Value <= 0)
			DestroyEnemy();
	}

	/// <summary>
	/// Decreases the health from the enemy.
	/// </summary>
	/// <returns>The health.</returns>
	/// <param name="damage">Damage.</param>
	public void DecreaseHealth(float damage)
	{
		GameObject damagelabel = Resources.Load("DamageLabel") as GameObject;
		damagelabel.GetComponent<DamageLabel>().Text = "" + damage;
		MonoBehaviour.Instantiate(damagelabel.gameObject,
		                          new Vector3(this.transform.position.x, this.transform.position.y, -2),
									Quaternion.identity);
		
		float criticalHitRate = new FloatRange(0, 100).Random;
		bool criticalHit = (!(criticalHitRate % 2).Equals(0) && criticalHitRate > 30 && criticalHitRate < 50) ? true : false;

		if (criticalHit)
			Health.Value -= damage * 1.5f;
		else
			Health.Value -= damage;


		if (criticalHit)
		{
			if (GameObject.Find("DamageLabel(Clone)") != null)
			{
				GameObject.Find("DamageLabel(Clone)").GetComponent<TextMesh>().color = Color.yellow;
			}
		}
	}

	public void DestroyEnemy()
	{
		float randomNum = UnityEngine.Random.Range(0, 100);
		bool willDropItem = false;

		if (randomNum <= ItemDropPercentage)
		{
			willDropItem = true;
		}

		if (willDropItem && ItemDropPrefabs != null && ItemDropPrefabs.Length > 0)
		{
			int item = (int)UnityEngine.Random.Range(0, ItemDropPrefabs.Length);
			Instantiate(ItemDropPrefabs[item], new Vector3(transform.position.x,
			                                               transform.position.y,
			                                              -0.5f), Quaternion.identity);
		}

		Destroy(this.gameObject);
	}

	/// <summary>
	/// Hurts the player on contact.
	/// </summary>
	/// <returns>The player on contac.</returns>
	void HurtPlayerOnContact()
	{
		if (MyBoxCollider.IsTouching(Player.MyBoxCollider))
		{
			IsNextToPlayer = true;

			damageDelay -= 1f;

			if (damageDelay <= 0)
			{
				Healthbar.DecreaseHP(Damage.Value);
				damageDelay = 40f;
			}
		}
		else {
			IsNextToPlayer = false;
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
			ThisRenderer.color = Color.yellow;
			Rigid.constraints = RigidbodyConstraints2D.FreezeAll;
		}

		stunTime -= 0.02f;

		if (stunTime <= 0)
		{
			stunTime = 5f;
			Stunned = false;
			ThisRenderer.color = Color.white;
			Rigid.constraints = RigidbodyConstraints2D.None;
			Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	/// <summary>
	/// Handles the effects of freezing.
	/// </summary>
	/// <returns>The effects.</returns>
	void FreezeEffects()
	{
		if (frozenTime > 0 && Frozen == true)
		{
			ThisRenderer.color = Color.cyan;
			Rigid.constraints = RigidbodyConstraints2D.FreezeAll;
		}

		frozenTime -= 0.02f;

		if (frozenTime <= 0)
		{
			frozenTime = 5f;
			Frozen = false;
			ThisRenderer.color = Color.white;
			Rigid.constraints = RigidbodyConstraints2D.None;
			Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	/// <summary>
	/// Handles the effects of poison.
	/// </summary>
	/// <returns>The effects.</returns>
	void PoisonEffects()
	{
		if (poisonDelay.Equals(4f))
			DecreaseHealth(4f);

		poisonDelay -= 0.05f;

		if (poisonDelay <= 0)
			poisonDelay = 4f;
	}


}