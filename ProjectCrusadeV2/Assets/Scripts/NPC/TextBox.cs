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

	//The maximum number of characters that can fit on one line in the text box.
	int MAX_LINE_LENGTH = 70;

	//Represents what the text box is showing up to while on a particular text slide.
	int textThrough = 0;



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
			textThrough = 0;
			Object.Destroy(GameObject.Find("TextBox(Clone)"));
		}
		else {
			CurrentSlide++;
			textThrough = 0;
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
	public void toggle() { open = !open; }


	/** Removes all of the text slides from this text box. */
	public void clear()
	{
		Text.Clear();
		CurrentSlide = 0;
		textThrough = 0;
	}


	/** Set the current slide of the text box. */
	public void setCurrentSlide(int i) { CurrentSlide = i; }


	/** Sets whether or not the text box is open. */
	public void setOpen(bool b) { open = b; }


	/** Sets where the text box should display the letters up to. */
	public void setTextThrough(int i) { textThrough = i; }



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



	/////////// Abstract Methods /////////////


	//public void draw(Graphics2D g)
	//{
	//	//If the text box is open...
	//	if (open == true && !text.isEmpty())
	//	{
	//		//Draw the background of the text box
	//		g.setColor(Color.white);
	//		g.fillRoundRect(0, Orbs.HEIGHT - 100, Orbs.WIDTH, 78, 20, 20);

	//		//Set the text attributes
	//		g.setColor(Color.black);
	//		g.setFont(new Font("Arial", 1, 15));


	//		//If the text is longer than one line
	//		if (text.get(currentSlide).length() > MAX_LINE_LENGTH)
	//		{

	//			int y = Orbs.HEIGHT - 80;
	//			string slideText = text.get(currentSlide);

	//			//Loop through each character on the text slide
	//			for (int i = 0; i < text.get(currentSlide).length(); i++)
	//			{
	//				int howFar = MAX_LINE_LENGTH;   //A variable for how far it needs to display the text

	//				//Set howFar based on the length of each line.
	//				if (slideText.length() >= MAX_LINE_LENGTH) { howFar = MAX_LINE_LENGTH; }
	//				else { howFar = slideText.length(); }

	//				if (textThrough < howFar)
	//				{
	//					g.drawstring(slideText.substring(0, textThrough), 5, y);
	//					textThrough++;
	//				}
	//				else {
	//					g.drawstring(slideText.substring(0, howFar), 5, y);
	//				}
	//				slideText = slideText.substring(howFar);
	//				y += 15;

	//				if (textThrough >= howFar) { continue; } else { break; }
	//			}


	//		}
	//		else {

	//			//If the entire slide of text is less than the maximum allowed, then just draw it on one line
	//			//g.drawstring(text.get(currentSlide), 5, Orbs.HEIGHT - 80);

	//			g.drawstring(text.get(currentSlide).substring(0, textThrough), 5, Orbs.HEIGHT - 80);
	//			if (textThrough < text.get(currentSlide).length()) textThrough++;

	//		}

	//	}
	//}



} //End of class
