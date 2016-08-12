using UnityEngine;
using System.Collections.Generic;

public class TextBox
{

	//The text to display inside this text box.
	public List<string> Text;

	//The current text slide that is being looked at.
	public int CurrentSlide;

	//Whether or not the text box is open.
	bool open;




	/////////// Constructor /////////////
	public TextBox()
	{
		Text = new List<string>();
	}


	/////////// Setters /////////////

	/** Moves to the next slide in the text box if there is one. Otherwise it will just close the text box.s */
	public void nextSlide()
	{
		if (CurrentSlide >= Text.Count - 1)
		{
			open = false;
			CurrentSlide = 0;
			Object.Destroy(GameObject.Find("TextBox(Clone)"));
		}
		else {
			CurrentSlide++;
		}
	}



	/// <summary>
	/// Adds text for the text box to display.	
	/// </summary>
	/// <returns>The text.</returns>
	/// <param name="t">T.</param>
	public void addText(string t) { Text.Add(t); }


	/// <summary>
	/// Adds text for the text box at the specified index.	
	/// </summary>
	/// <returns>The text.</returns>
	/// <param name="t">T.</param>
	public void addText(string t, int index) { Text.Insert(index, t); }


	/** Toggles the text box. If it was open, it will close. If it was closed, it will open. */
	public void toggle() { open = !open; CurrentSlide = 0; }


	/** Removes all of the text slides from this text box. */
	public void clear()
	{
		Text.Clear();
		CurrentSlide = 0;
	}


	/** Set the current slide of the text box. */
	public void setCurrentSlide(int i) { CurrentSlide = i; }


	/** Sets whether or not the text box is open. */
	public void setOpen(bool b) { open = b; }


	/////////// Getters /////////////

	/** Returns whether or not the text box is currently open. */
	public bool isOpen() { return open; }


	/** Returns a list of all of the text slides that the text box is set to display. */
	public List<string> getTextSlides() { return Text; }


	/** Checks if the text box is currently on its last text slide. */
	public bool onLast()
	{
		if (CurrentSlide == Text.Count - 1) return true;
		return false;
	}



} //End of class
