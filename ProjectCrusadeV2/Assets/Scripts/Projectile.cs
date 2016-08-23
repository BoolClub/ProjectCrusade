using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Represents all types of projectiles (arrows, magic bolts, etc.)
/// </summary>
public class Projectile : MonoBehaviour {

	/// <summary>
	/// The rigidbody 2D.
	/// </summary>
	Rigidbody2D Rigid;

	/// <summary>
	/// The game object that is launching the projectile.
	/// </summary>
	public GameObject Launcher;

	/// <summary>
	/// The direction.
	/// </summary>
	public Direction direction;

	/// <summary>
	/// The speed.
	/// </summary>
	public float Speed;

	/// <summary>
	/// The amount of damage that this projectile will do to an enemy upon contact.
	/// </summary>
	public FloatRange Damage;

	/// <summary>
	/// The game object to aim this projectile at.
	/// </summary>
	public GameObject AimAt;

	/// <summary>
	/// The position of the object that it should aim at.
	/// </summary>
	public Vector3 AimAtPosition;


	void Start () {
		Rigid = GetComponent<Rigidbody2D>();
		if (AimAt != null)
		{
			AimAtPosition = AimAt.transform.position;
		}
		GetDirectionFromLauncher();
		Damage.Value = (float)Math.Round(Damage.Random, 2);
	}

	/// <summary>
	/// Makes sure the projectile is facing in the same direction as the player.
	/// </summary>
	/// <returns>The direction from player.</returns>
	public void GetDirectionFromLauncher()
	{
		if (Launcher.GetComponent<PlayerControls>() != null)
		{
			direction = Launcher.GetComponent<PlayerControls>().Direction;
		}
		else if (Launcher.GetComponent<GoodNPCBoss>() != null)
		{
			direction = Launcher.GetComponent<GoodNPCBoss>().Direction;
		}


		if (direction == Direction.North)
			transform.Rotate(new Vector3(0, 0, 90));
		if (direction == Direction.East)
			transform.Rotate(new Vector3(0, 0, 0));
		if (direction == Direction.South)
			transform.Rotate(new Vector3(0, 0, 270));
		if (direction == Direction.West)
			transform.Rotate(new Vector3(0, 0, 180));
		if (direction == Direction.NorthEast)
			transform.Rotate(new Vector3(0, 0, 45));
		if (direction == Direction.SouthEast)
			transform.Rotate(new Vector3(0, 0, 315));
		if (direction == Direction.SouthWest)
			transform.Rotate(new Vector3(0, 0, 225));
		if (direction == Direction.NorthWest)
			transform.Rotate(new Vector3(0, 0, 135));
	}
	
	// Update is called once per frame
	void Update () {
		//Always moving forward, but accounting for its current rotation angle.
		if (AimAt == null)
		{
			transform.Translate(Time.deltaTime * Speed, 0, 0);
		}
		else {
			// Move towards the aimed at position.
			Rigid.MovePosition(Vector3.MoveTowards(transform.position,
			                                       AimAtPosition,
												   Time.deltaTime * Speed));
		}

		// Just destroy the object if it reaches the player but the player moves out of the way.
		if (PlusMinus(transform.position.x, AimAtPosition.x, 0.5f) 
		    && PlusMinus(transform.position.y, AimAtPosition.y, 0.5f))
		{
			// Play fade out animation.
			Destroy(this.gameObject);
		}
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Wall"))
		{
			Destroy(this.gameObject);
		}

		// Launched by player
		if (!Launcher.tag.Equals("Enemy"))
		{
			if (other.tag.Equals("Enemy") && other is BoxCollider2D)
			{
				// Do damage to the enemy
				other.GetComponent<Enemy>().Health.Value -= Damage.Value;

				// Create a damage label object to display how much damage was done to the enemy.
				GameObject damagelabel = Resources.Load("DamageLabel") as GameObject;
				damagelabel.GetComponent<DamageLabel>().Text = "" + Damage.Value;
				Instantiate(damagelabel,
							new Vector3(other.transform.position.x, other.transform.position.y, -2),
							Quaternion.identity);

				Destroy(this.gameObject);
			}
		}
		// Launched by enemy or boss
		if(Launcher.tag.Equals("Enemy") || Launcher.tag.Equals("Boss")) {
			if (other.tag.Equals("Player") && other is BoxCollider2D)
			{
				// Do damage to the enemy
				Healthbar.DecreaseHP(Damage.Value);

				Destroy(this.gameObject);
			}
		}
	}


	/// <summary>
	/// Returns whether or not the first number is equal to the second number, give or take plusOrMinus.
	/// </summary>
	/// <returns>The minus.</returns>
	/// <param name="num1">Num1.</param>
	/// <param name="num2">Num2.</param>
	/// <param name="plusOrMinus">Plusminus.</param>
	public bool PlusMinus(float num1, float num2, float plusOrMinus)
	{
		if (num1 <= num2 + plusOrMinus && num1 >= num2 - plusOrMinus)
		{
			return true;
		}
		return false;
	}
}
