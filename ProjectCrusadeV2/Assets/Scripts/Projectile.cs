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
		Damage.Value = Mathf.Ceil(Damage.Random);
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
}
