using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITest
{
    public class Sprite
    {
        public Rectangle spriteRectangle;
        Sprite[] blocks;
        Texture2D spriteTexture;

        public Sprite(Rectangle inRect, Texture2D inTexture, Sprite [] inBlocks)
        {
            spriteRectangle = inRect;
            spriteTexture = inTexture;
            blocks = inBlocks;
        }
        public Sprite(Rectangle inRect, Texture2D inTexture)
        {
            spriteRectangle = inRect;
            spriteTexture = inTexture;
        }

        public virtual void Update()
        {

        }

        public virtual void Update(Sprite player)
        {

        }

        public void Draw(SpriteBatch spriteBatch, Color colour)
        {
            spriteBatch.Draw(spriteTexture, spriteRectangle, colour);
        }

        protected bool CollidesWithBlocks()
        {
            bool returnValue = false;
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].spriteRectangle.Intersects(spriteRectangle))
                {
                    returnValue = true;
                    break;
                }
            }
            return returnValue;
        }
    }
}
