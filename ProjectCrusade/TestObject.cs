using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Shadows2D
{
    class TestObject
    {
        Texture2D texture;       //sprite-sheet containing the animation
        Animation animation;     //animation object used for animating
        Vector2 origin;          //origin of the image

        public Vector2 Position { get; set; }   //position on the screen

        /// <summary>
        /// Creates a new object
        /// </summary>
        /// <param name="texture">texture to use for the animation</param>
        /// <param name="frameCount">how many frames are in the texture</param>
        /// <param name="origin">origin to use for drawing individual sprites</param>
        public TestObject(Texture2D texture, int frameCount, Vector2 origin)
        {
            this.texture = texture;
            //create a new animation object
            animation = new Animation(texture.Width, texture.Height, frameCount, 0, 0);

            //tweak the FramesPerSecond and movementSpeed values until you're satisfied with how things move
            animation.FramesPerSecond = 14*2;

            //reset position
            Position = Vector2.Zero;

            this.origin = origin;
        }

        /// <summary>
        /// Update position of the object, and the animation
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            animation.Update(elapsed);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(texture, position, animation.CurrentFrame,
                            color, 0.0f, origin, 1.0f, SpriteEffects.None, 1.0f);
        }

    }
}
