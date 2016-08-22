using UnityEngine;
using System.Collections;

public class GoodNPCBoss : MonoBehaviour {
	const float WAIT_TIME = 150f;

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



	void Start()
	{
		Direction = Direction.South;
	}

	void Update()
	{
		if (WaitTime >= WAIT_TIME)
		{
			// Launch projectiles, one on each side of the boss.
			GameObject randomProj = Projectiles[new IntRange(0, Projectiles.Length).Random];
			randomProj.GetComponent<Projectile>().Launcher = this.gameObject;

			Instantiate(randomProj,
			            new Vector3(transform.position.x - 5, transform.position.y, randomProj.transform.position.z),
						Quaternion.identity);
			Instantiate(randomProj,
			            new Vector3(transform.position.x + 5, transform.position.y, randomProj.transform.position.z),
						Quaternion.identity);
		}

		WaitTime -= 0.5f;

		if (WaitTime <= 0)
		{
			WaitTime = WAIT_TIME;
		}


	}



}
