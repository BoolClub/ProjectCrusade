using UnityEngine;
using System.Collections;

public class BadNPCBoss : MonoBehaviour {
	const float WAIT_TIME = 50f;
	const float MOVE_WAIT_TIME = 200f;

	/// <summary>
	/// The player.
	/// </summary>
	GameObject Player;

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
	/// The amount of time to wait before moving again.
	/// </summary>
	float MoveTime = MOVE_WAIT_TIME;
	bool CanMove;

	/// <summary>
	/// The different positions that the boss can teleport himself to.
	/// </summary>
	Vector3[] TeleportPositions = {new Vector3(46,68.11f,-1), new Vector3(58.2f, 68f, -1),
								   new Vector3(60.4f,58f,-1), new Vector3(42.3f,54.9f,-1),
								   new Vector3(50.6f,51f,-1), new Vector3(51.55f,66f,-1) };
	/// <summary>
	/// The index of the current position.
	/// </summary>
	int currentPositionIndex = 0;

	/// <summary>
	/// The random projectile from the list.
	/// </summary>
	GameObject randomProj;



	void Start()
	{
		Direction = Direction.South;
		Player = GameObject.FindWithTag("Player");
		EnemyScript = GetComponent<Enemy>();
	}

	void Update()
	{
		if (Projectiles != null)
		{
			ShootProjectiles();
		}

		MoveBossAroundTheMap();

		if (EnemyScript.Health.Value <= 0)
		{
			EnemyScript.DestroyEnemy();
		}
	}

	void MoveBossAroundTheMap()
	{
		if (MoveTime >= MOVE_WAIT_TIME)
		{
			CanMove = true;
		}

		MoveTime -= 0.5f;

		if (MoveTime <= 0)
		{
			MoveTime = MOVE_WAIT_TIME;
		}


		// Move to next position.
		if (CanMove == true)
		{
			if (currentPositionIndex < TeleportPositions.Length - 1)
			{
				currentPositionIndex++;
				transform.position = TeleportPositions[currentPositionIndex];
				CanMove = false;
			}
			else {
				currentPositionIndex = 0;
				transform.position = TeleportPositions[currentPositionIndex];
				CanMove = false;
			}
		}
	}

	void ShootProjectiles()
	{
		if (WaitTime >= WAIT_TIME)
		{
			// Choose a random projectile from the list
			int RandIndex = new IntRange(0, Projectiles.Length).Random;
			randomProj = Projectiles[RandIndex];
			randomProj.GetComponent<Projectile>().Launcher = this.gameObject;

			// Launch projectiles from the list.
			StartCoroutine(MultiShotAttack());
		}

		WaitTime -= 0.5f;

		if (WaitTime <= 0)
		{
			WaitTime = WAIT_TIME;
		}
	}

	// Has the boss shoot out multiple attacks in different directions, all in less than 1 second.
	IEnumerator MultiShotAttack()
	{
		Direction = Direction.South;
		randomProj.GetComponent<Projectile>().AimAt = Player;
		randomProj.GetComponent<Projectile>().AimAtPosition = Player.transform.position;

		Direction = Direction.West;
		Instantiate(randomProj, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
		yield return new WaitForSeconds(0.2f);

		Direction = Direction.SouthWest;
		Instantiate(randomProj, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
		yield return new WaitForSeconds(0.2f);

		Direction = Direction.South;
		Instantiate(randomProj, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
		yield return new WaitForSeconds(0.2f);

		Direction = Direction.SouthEast;
		Instantiate(randomProj, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
		yield return new WaitForSeconds(0.2f);

		Direction = Direction.East;
		Instantiate(randomProj, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
	}
}
