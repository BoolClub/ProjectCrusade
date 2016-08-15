using UnityEngine;
using System.Collections;

/// <summary>
/// Represents all types of projectiles (arrows, magic bolts, etc.)
/// </summary>
public class Projectile : MonoBehaviour {

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



	// Use this for initialization
	void Start () {
		GetDirectionFromPlayer();
	}

	/// <summary>
	/// Makes sure the projectile is facing in the same direction as the player.
	/// </summary>
	/// <returns>The direction from player.</returns>
	public void GetDirectionFromPlayer()
	{
		direction = GameObject.FindWithTag("Player").GetComponent<PlayerControls>().Direction;

		if (direction == Direction.North)
			transform.Rotate(new Vector3(0,0,90));
		if (direction == Direction.East)
			transform.Rotate(new Vector3(0, 0, 0));
		if (direction == Direction.South)
			transform.Rotate(new Vector3(0, 0, 270));
		if (direction == Direction.West)
			transform.Rotate(new Vector3(0, 0, 180));
	}
	
	// Update is called once per frame
	void Update () {

		//Always moving forward, but accounting for its current rotation angle.
		if (direction == Direction.North)
			transform.Translate(Time.deltaTime * Speed, 0, 0);
		if (direction == Direction.East)
			transform.Translate(Time.deltaTime * Speed, 0, 0);
		if (direction == Direction.South)
			transform.Translate(Time.deltaTime * Speed, 0, 0);
		if (direction == Direction.West)
			transform.Translate(Time.deltaTime * Speed, 0, 0);
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Wall"))
		{
			Destroy(this.gameObject);
		}

		if (other.tag.Equals("Enemy"))
		{
			// Do damage to the enemy
			Destroy(this.gameObject);
			other.GetComponent<Enemy>().Health.Value -= Damage.Value;
			Debug.Log(other.GetComponent<Enemy>().Health.Value);
		}
	}
}
