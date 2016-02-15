using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;

namespace ProjectCrusade
{
	/*
	* Class that handles all of the player input. To use it, just go into the Player class and call this 
	* static method in the update method.
	*/

	public class PlayerInput {
		/*
		 * Booleans for checking which direction the player is moving in. Also an extra boolean
		 * just for checking if the player is moving in general, which we may or may not use later.
		 */
		public static bool Up { get; private set; }
		public static bool Down { get; private set; }
		public static bool Left { get; private set; }
		public static bool Right { get; private set; }
		public static bool Moving { get; private set; }


		public PlayerInput () {}



		//PLAYER INPUT
		public static void CheckInput() { 
			//Up
			if (Keyboard.GetState ().IsKeyDown (Keys.Up)) {
				Up = true;
				Down = false;
				Left = false;
				Right = false;
				Moving = true;
				Console.WriteLine ("Moving Up");
			}

			//Down
			if (Keyboard.GetState ().IsKeyDown (Keys.Down)) {
				Up = false;
				Down = true;
				Left = false;
				Right = false;
				Moving = true;
				Console.WriteLine ("Moving Down");
			}

			//Left
			if (Keyboard.GetState ().IsKeyDown (Keys.Left)) {
				Up = false;
				Down = false;
				Left = true;
				Right = false;
				Moving = true;
				Console.WriteLine ("Moving Left");
			}

			//Right
			if (Keyboard.GetState ().IsKeyDown (Keys.Right)) {
				Up = false;
				Down = false;
				Left = false;
				Right = true;
				Moving = true;
				Console.WriteLine ("Moving Right");
			}

			//Idle
			if ( !(Keyboard.GetState ().IsKeyDown (Keys.Right)) && !(Keyboard.GetState ().IsKeyDown (Keys.Left)
				&& !(Keyboard.GetState ().IsKeyDown (Keys.Up)) && !(Keyboard.GetState ().IsKeyDown (Keys.Down))) ) {
				Up = false;
				Down = false;
				Left = false;
				Right = false;
				Moving = false;
				Console.WriteLine ("Idle");
			}
		}

	} //END OF PLAYERINPUT CLASS



} //END OF NAMESPACE

