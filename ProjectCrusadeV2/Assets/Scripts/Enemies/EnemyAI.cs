using System;
using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	const float MOVE_TIME = 50f;
	const float WAIT_TIME = 50f;

	#region Refrences For Simplicity

		/// <summary>
		/// The player. Used for grabbing information without having to keep calling GameObject.Find().
		/// </summary>
		PlayerControls Player;

		/// <summary>
		/// The enemy's rigid body.
		/// </summary>
		Rigidbody2D Rigid;

		/// <summary>
		/// The circle collider for this enemy.
		/// </summary>
		CircleCollider2D CircleColl;

		/// <summary>
		/// The renderer.
		/// </summary>
		#pragma warning disable
		SpriteRenderer renderer;

		/// <summary>
		/// This enemy object's Enemy script
		/// </summary>
		Enemy ThisEnemyObject;

	#endregion

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

	/// <summary>
	/// The sprites for each direction of the enemy
	/// </summary>
	public Sprite[] EnemyDirectionSprites;



	void Start()
	{
		MoveDirection = (Direction)new IntRange(0, 8).Random;
		Rigid = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();
		Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
		CircleColl = GetComponent<CircleCollider2D>();
		ThisEnemyObject = GetComponent<Enemy>();
	}


	void Update()
	{
		if(!Player.GM.Paused)
			MoveEnemy();
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


	void MoveEnemy()
	{
		float movement = Time.deltaTime * speed;

		// If the player is within the circle radius, then have the enemy start following it.
		if (CircleColl.IsTouching(Player.GetComponent<BoxCollider2D>()))
		{
			if (ThisEnemyObject.Stunned == false && ThisEnemyObject.Frozen == false)
			{
				ChangeRotationBasedOnWherePlayerIs();
				Rigid.MovePosition(Vector2.MoveTowards(transform.position, Player.transform.position, movement));
			}
		}
		else {

			// If the player is not close enough to the enemy, then just move the enemy around aimlessly.
			Timer();

			// the enemy can only move if CanMove is true and the enemy is not stunned.
			if (CanMove == true && ThisEnemyObject.Stunned == false)
			{
				Moving = true;

				if (MoveDirection == Direction.North)
				{
					renderer.sprite = EnemyDirectionSprites[0];
					Rigid.MovePosition(new Vector2(transform.position.x, transform.position.y + movement));
				}
				if (MoveDirection == Direction.East)
				{
					renderer.sprite = EnemyDirectionSprites[1];
					Rigid.MovePosition(new Vector2(transform.position.x + movement, transform.position.y));
				}
				if (MoveDirection == Direction.South)
				{
					renderer.sprite = EnemyDirectionSprites[2];
					Rigid.MovePosition(new Vector2(transform.position.x, transform.position.y - movement));
				}
				if (MoveDirection == Direction.West)
				{
					renderer.sprite = EnemyDirectionSprites[3];
					Rigid.MovePosition(new Vector2(transform.position.x - movement, transform.position.y));
				}
				if (MoveDirection == Direction.NorthEast)
				{
					//Vector3 move = Vector3.right * speed * Time.deltaTime;
					//move += Vector3.up * speed * Time.deltaTime;
					//transform.Translate(move);
					renderer.sprite = EnemyDirectionSprites[4];
					Rigid.MovePosition(new Vector2(transform.position.x + movement, transform.position.y + movement));
				}

				if (MoveDirection == Direction.SouthEast)
				{
					//Vector3 move = Vector3.right * speed * Time.deltaTime;
					//move += Vector3.down * speed * Time.deltaTime;
					//transform.Translate(move);
					renderer.sprite = EnemyDirectionSprites[5];
					Rigid.MovePosition(new Vector2(transform.position.x + movement, transform.position.y - movement));
				}

				if (MoveDirection == Direction.SouthWest)
				{
					//Vector3 move = Vector3.left * speed * Time.deltaTime;
					//move += Vector3.down * speed * Time.deltaTime;
					//transform.Translate(move);
					renderer.sprite = EnemyDirectionSprites[6];
					Rigid.MovePosition(new Vector2(transform.position.x - movement, transform.position.y - movement));
				}

				if (MoveDirection == Direction.NorthWest)
				{
					//Vector3 move = Vector3.right * speed * Time.deltaTime;
					//move += Vector3.up * speed * Time.deltaTime;
					//transform.Translate(move);
					renderer.sprite = EnemyDirectionSprites[7];
					Rigid.MovePosition(new Vector2(transform.position.x - movement, transform.position.y + movement));
				}

			}
			else {
				Moving = false;
				ChangeDirection();
			}
		}
	}



	void ChangeRotationBasedOnWherePlayerIs()
	{
		if (PlusMinus(transform.position.y, Player.gameObject.transform.position.y, 0.5f))
		{
			if (Player.gameObject.transform.position.x > this.transform.position.x)
			{
				renderer.sprite = EnemyDirectionSprites[1];
			}
			else if (Player.gameObject.transform.position.x < this.transform.position.x)
			{
				renderer.sprite = EnemyDirectionSprites[3];
			}
		} 
		else if (PlusMinus(transform.position.x, Player.gameObject.transform.position.x, 0.5f))
		{
			if (Player.gameObject.transform.position.y > this.transform.position.y)
			{
				renderer.sprite = EnemyDirectionSprites[0];
			}
			else if (Player.gameObject.transform.position.y < this.transform.position.y)
			{
				renderer.sprite = EnemyDirectionSprites[2];
			}
		}
		else {
			if (Player.gameObject.transform.position.y > this.transform.position.y)
			{
				if (Player.gameObject.transform.position.x > this.transform.position.x)
				{
					renderer.sprite = EnemyDirectionSprites[4];
				}
				else if (Player.gameObject.transform.position.x < this.transform.position.x)
				{
					renderer.sprite = EnemyDirectionSprites[7];
				}
				else {
					renderer.sprite = EnemyDirectionSprites[0];
				}
			}
			else {
				if (Player.gameObject.transform.position.x > this.transform.position.x)
				{
					renderer.sprite = EnemyDirectionSprites[5];
				}
				else if (Player.gameObject.transform.position.x < this.transform.position.x)
				{
					renderer.sprite = EnemyDirectionSprites[6];
				}
				else {
					renderer.sprite = EnemyDirectionSprites[2];
				}
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
