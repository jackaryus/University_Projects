using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Space_Banjo
{
    public class Explosion : Sprite
    {
        public int opacity;

        public Explosion(float x, float y, float scale, int textureid, Game1 game):base(x, y, scale, textureid, game)
        {
            this.opacity = 255;
        }

        public void update()
        {
            //reduces opacity of explosion until it reaches 0 when it deletes
            if (opacity > 0)
            {
                opacity -= 10;
            }
            else
            {
                this.game.removeexplosion(this);  
            }
        }

        public override void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spritebatch)
        {
            // draws the explosion
            Texture2D texture = game.gettexture(textureid);
            spritebatch.Draw(texture, this.rectangle, new Color(255, 255, 255, opacity));
        }


    }
}
