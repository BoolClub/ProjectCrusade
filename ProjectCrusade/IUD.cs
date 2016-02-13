using System;
using Microsoft;
using MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrusade
{
	/*
	
	This interface is just an easy way to add the initialize, update, and draw methods in whatever
    game object we need. For example, the Player class will implement this interface to do all the 
    initializing, updating, and drawing.

	*/
	public interface IUD {

		void initialize();
		void update(double time);
		void draw(GraphicsDeviceManager g);

	}
}

