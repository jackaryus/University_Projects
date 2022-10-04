using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITest
{
    public class Player : Sprite
    {
        private int velocity;
        private Sprite[] blockArray;

        public Player(Rectangle inRect, Texture2D inTexture, int inVelocity, Sprite [] inArray) : base(inRect, inTexture, inArray)
        {
            velocity = inVelocity;
            blockArray = inArray;
        }

        public override void Update()
        {
            KeyboardState keystate = Keyboard.GetState();

            Rectangle previousPosition = spriteRectangle;

            if (keystate.IsKeyDown(Keys.Up))
                spriteRectangle.Y -= velocity;
            if (keystate.IsKeyDown(Keys.Down))
                spriteRectangle.Y += velocity;
            if (CollidesWithBlocks())
                spriteRectangle = previousPosition;
            else
                previousPosition = spriteRectangle;
            if (keystate.IsKeyDown(Keys.Left))
                spriteRectangle.X -= velocity;
            if (keystate.IsKeyDown(Keys.Right))
                spriteRectangle.X += velocity;
            if (CollidesWithBlocks())
                spriteRectangle = previousPosition;
            else
                previousPosition = spriteRectangle;
        }
    }
}
