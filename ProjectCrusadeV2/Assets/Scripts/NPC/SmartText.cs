using UnityEngine;
using System.Collections;

/// <summary>
/// Smart Text represents a piece of text that can resize itself to fit the size of the text box that it is in.
/// In other words, it basically performs the same function as the text wrap feature of certain text editors.
/// </summary>
[RequireComponent(typeof(TextMesh))]
public class SmartText : MonoBehaviour {

	/// <summary>
	/// The text mesh component of this text object.
	/// </summary>
	TextMesh Mesh;


	/// <summary>
	/// The maximum number of characters allowed on one line of text.
	/// </summary>
	int MAX_LINE_LENGTH = 38;




	/// <summary>
	/// Wraps the text to it's container whenever it has been changed.
	/// This method must be called manually.
	/// </summary>
	/// <returns>The text changed.</returns>
	public void OnTextChanged()
	{
		Mesh = GetComponent(typeof(TextMesh)) as TextMesh;

		//If the text is longer than the container can hold
		if (Mesh.text.Length > MAX_LINE_LENGTH)
		{
			string NewString = "";
			int CHARACTER_BOUNDARY = MAX_LINE_LENGTH;

			//Loop through and add a line break in the appropriate areas.
			for (int i = 0; i < Mesh.text.Length; i++)
			{
				NewString += Mesh.text[i];
				if (i == CHARACTER_BOUNDARY)
				{
					NewString += "\n";
					CHARACTER_BOUNDARY += CHARACTER_BOUNDARY;
				}
			}

			//Set the text to the one with the line breaks
			Mesh.text = NewString;
		}
	}

}
