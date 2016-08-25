using UnityEngine;
using System.Collections;

public class GoodNPCBoss : MonoBehaviour {
	const float WAIT_TIME = 150f;

	/// <summary>
	/// The player.
	/// </summary>
	GameObject Player;

	/// <summary>
	/// The gm.
	/// </summary>
	GameManagerScript GM;

	/// <summary>
	/// sprite renderer.
	/// </summary>
	#pragma warning disable
	SpriteRenderer renderer;

	/// <summary>
	/// The sprite.
	/// </summary>
	public Sprite firstSprite;

	/// <summary>
	/// The sprite.
	/// </summary>
	public Sprite secondSprite;

	/// <summary>
	/// The enemy script attached to this boss.
	/// </summary>
	Enemy EnemyScript;

	/// <summary>
	/// The direction that the boss is facing in.
	/// </summary>
	public Direction Direction;

	/// <summary>
	/// The different types of projectiles that the boss can launch.
	/// </summary>
	public GameObject[] Projectiles;

	/// <summary>
	/// The amount of time to wait before launching another ranged attack.
	/// </summary>
	float WaitTime = WAIT_TIME;

	/// <summary>
	/// The random projectile from the list.
	/// </summary>
	GameObject randomProj;



	void Start()
	{
		Direction = Direction.South;
		Player = GameObject.FindWithTag("Player");
		EnemyScript = GetComponent<Enemy>();
		GM = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
		renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = firstSprite;
	}

	void Update()
	{
		if (!GM.Paused)
		{
			if (Projectiles != null)
			{
				if (WaitTime >= WAIT_TIME)
				{
					// Choose a random projectile from the list
					int RandIndex = new IntRange(0, Projectiles.Length).Random;
					randomProj = Projectiles[RandIndex];
					randomProj.GetComponent<Projectile>().Launcher = this.gameObject;

					// Launch projectiles from the list.
					if (RandIndex == 0)
						StartCoroutine(FistThrowAttack());
					if (RandIndex == 1)
						StartCoroutine(MultiShotAttack());
				}

				WaitTime -= 0.5f;

				if (WaitTime <= 0)
				{
					WaitTime = WAIT_TIME;
				}
			}
		}

		// Handle sequence for beating the boss
		if (EnemyScript.Health.Value <= 0)
		{
			StartCoroutine(BeatBossSequence());
		}

	}

	// Plays through particular events after the boss looses.
	IEnumerator BeatBossSequence()
	{
		// Play animation of monster being blown up.
		yield return new WaitForSeconds(10);
		EnemyScript.DestroyEnemy();
	}


	// Has the boss throw his fists at the player.
	IEnumerator FistThrowAttack()
	{
		renderer.sprite = secondSprite;

		Direction = Direction.South;
		randomProj.GetComponent<Projectile>().AimAt = Player;
		randomProj.GetComponent<Projectile>().AimAtPosition = Player.transform.position;
		Instantiate(randomProj, new Vector2(transform.position.x - 5, transform.position.y), Quaternion.identity);
		yield return new WaitForSeconds(0.3f);

		Direction = Direction.South;
		randomProj.GetComponent<Projectile>().AimAt = Player;
		randomProj.GetComponent<Projectile>().AimAtPosition = Player.transform.position;
		Instantiate(randomProj, new Vector2(transform.position.x + 5, transform.position.y), Quaternion.identity);

		renderer.sprite = firstSprite;
	}

	// Has the boss shoot out multiple attacks in different directions, all in less than 1 second.
	IEnumerator MultiShotAttack()
	{
		renderer.sprite = secondSprite;

		Direction = Direction.West;
		Instantiate(randomProj, new Vector2(transform.position.x - 1, transform.position.y), Quaternion.identity);
		yield return new WaitForSeconds(0.2f);

		Direction = Direction.SouthWest;
		Instantiate(randomProj, new Vector2(transform.position.x - 1, transform.position.y - 1), Quaternion.identity);
		yield return new WaitForSeconds(0.2f);

		Direction = Direction.South;
		Instantiate(randomProj, new Vector2(transform.position.x, transform.position.y - 1), Quaternion.identity);
		yield return new WaitForSeconds(0.2f);

		Direction = Direction.SouthEast;
		Instantiate(randomProj, new Vector2(transform.position.x + 1, transform.position.y - 1), Quaternion.identity);
		yield return new WaitForSeconds(0.2f);

		Direction = Direction.East;
		Instantiate(randomProj, new Vector2(transform.position.x + 1, transform.position.y), Quaternion.identity);

		renderer.sprite = firstSprite;
	}
}
