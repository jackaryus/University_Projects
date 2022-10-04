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
    [Serializable]
    class Banjo : Sprite
    {
        protected float xmovement;
        protected float ymovement = 0;
        protected float xspeed = 3;
        protected int mobscore;

        public Banjo(float spritex, float spritey, float spritescale, int textureid, Game1 game)
            : base(spritex, spritey, spritescale, textureid, game) 
        {
            health = 1;
            xmovement = 3;
            this.spritetype = SpriteType.mob;
            mobscore = 10;
        }

        public override void oncollision(Sprite other)
        {
            if (other.getspritetype() == SpriteType.player)
            {
                other.takehit();
                this.health = 0;
                this.isdead = true;
            }
        }

        public int getmobscore()
        {
            return this.mobscore;
        }

        public override void update(GameTime gametime)
        {
            base.update(gametime);
            ymovement = 0;
            // moves mob down if it is touches a side and reverses the direction
            if (this.rectangle.Left <= 0)
            {
                ymovement = 10;
                xmovement = xspeed;
            }
            if (this.rectangle.Right >= 800)
            {
                ymovement = 10;
                xmovement = -xspeed;
            }
            //moves the normal banjo
            this.position.X += xmovement;
            this.rectangle.X = (int)(this.position.X - (this.rectangle.Width / 2));
            this.position.Y += ymovement;
            this.rectangle.Y = (int)(this.position.Y - (this.rectangle.Width / 2));

        }
    }
}
