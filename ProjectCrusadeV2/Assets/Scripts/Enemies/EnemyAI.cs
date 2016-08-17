using System;
using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	const float MOVE_TIME = 50f;
	const float WAIT_TIME = 50f;

	/// <summary>
	/// The speed of the game object.
	/// </summary>
	public float speed = 1;

	/// <summary>
	/// The amount of time to move for.
	/// </summary>
	float MoveTime = MOVE_TIME;

	/// <summary>
	/// The amount of time to wait before moving again.
	/// </summary>
	float WaitTime = WAIT_TIME;

	/// <summary>
	/// Whether or not the enemy can move.
	/// </summary>
	bool CanMove;

	/// <summary>
	/// Whether or not the enemy is moving.
	/// </summary>
	#pragma warning disable
	bool Moving;

	/// <summary>
	/// The random direction to move in.
	/// </summary>
	Direction MoveDirection;

	/// <summary>
	/// Whether or not the direction should be changed.
	/// </summary>
	bool ShouldChangeDirection;



	void Start()
	{
		MoveDirection = (Direction)new IntRange(0, 8).Random;
	}


	void Update()
	{
		// If the player is within the circle radius, then have the enemy start following it.
		if (GetComponent<CircleCollider2D>().IsTouching(GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>()))
		{
			if(gameObject.GetComponent<Enemy>().Stunned == false)
				transform.position = Vector3.MoveTowards(transform.position, GameObject.FindWithTag("Player").transform.position, Time.deltaTime * speed);
		}
		else {

			// If the player is not close enough to the enemy, then just move the enemy around aimlessly.
			Timer();

			// the enemy can only move if CanMove is true and the enemy is not stunned.
			if (CanMove == true && gameObject.GetComponent<Enemy>().Stunned == false)
			{
				Moving = true;

				if (MoveDirection == Direction.North)
					transform.Translate(0, Time.deltaTime * speed, 0);

				if (MoveDirection == Direction.East)
					transform.Translate(Time.deltaTime * speed, 0, 0);

				if (MoveDirection == Direction.South)
					transform.Translate(0, -(Time.deltaTime * speed), 0);

				if (MoveDirection == Direction.West)
					transform.Translate(-(Time.deltaTime * speed), 0, 0);

				if (MoveDirection == Direction.NorthEast)
				{
					Vector3 move = Vector3.right * speed * Time.deltaTime;
					move += Vector3.up * speed * Time.deltaTime;
					transform.Translate(move);
				}

				if (MoveDirection == Direction.SouthEast)
				{
					Vector3 move = Vector3.right * speed * Time.deltaTime;
					move += Vector3.down * speed * Time.deltaTime;
					transform.Translate(move);
				}

				if (MoveDirection == Direction.SouthWest)
				{
					Vector3 move = Vector3.left * speed * Time.deltaTime;
					move += Vector3.down * speed * Time.deltaTime;
					transform.Translate(move);
				}

				if (MoveDirection == Direction.NorthWest)
				{
					Vector3 move = Vector3.right * speed * Time.deltaTime;
					move += Vector3.up * speed * Time.deltaTime;
					transform.Translate(move);
				}

			}
			else {
				Moving = false;
				ChangeDirection();
			}

		}
	}

	/// <summary>
	/// Handles the amount of time for when the enemy can move on its own.
	/// </summary>
	void Timer()
	{
		if (MoveTime > 0)
		{
			MoveTime -= 0.5f;
		}

		if (MoveTime <= 0)
		{
			CanMove = false;
			WaitTime -= 1f;

			if (WaitTime <= 0)
			{
				MoveTime = MOVE_TIME;
				CanMove = true;
				WaitTime = WAIT_TIME;
			}
		}
	}


	void ChangeDirection()
	{
		MoveDirection = (Direction)new IntRange(0, 8).Random;
	}
}
