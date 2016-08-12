using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	/// <summary>
	/// This is the item that this chest will give the player.
	/// </summary>
	public ItemType Item;

	/// <summary>
	/// The sprites for when the chest is opened and closed.
	/// </summary>
	public Sprite Opened, Closed;



	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		//Change the sprite when the chest has been opened
		if (Item == ItemType.EMPTY)
		{
			GetComponent<SpriteRenderer>().sprite = Opened;
		} else {
			GetComponent<SpriteRenderer>().sprite = Closed;
		}
	}


	/// <summary>
	/// Takes the item from the chest.
	/// </summary>
	/// <returns>The item.</returns>
	public void TakeItem() { Item = ItemType.EMPTY; }


	/// <summary>
	/// Returns whether or not the player is next to the chest.
	/// </summary>
	/// <returns>The next to player.</returns>
	public bool isNextToPlayer()
	{
		if (GetComponent<BoxCollider2D>().IsTouching(GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>()))
		{
			return true;
		} else {
			return false;
		}
	}
}
