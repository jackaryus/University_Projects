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
    class Hunter : Banjo
    {
        protected float yspeed;
        protected double chasetime;
        public Hunter(float spritex, float spritey, float spritescale, int textureid, Game1 game)
            : base(spritex, spritey, spritescale, textureid, game)
        {
            chasetime = 5.0;
            mobscore = 20;
        }

        public override void update(GameTime gametime)
        {
            yspeed = 3;
            // overides original movement pattern after 5 seconds
            if (chasetime > 0)
            {
                base.update(gametime);
                chasetime -= gametime.ElapsedGameTime.TotalSeconds;

            }
            else
            {
                // original banjo movement before 5 secs pass
                ymovement = yspeed;
                xmovement = 0;
                float playerX = this.game.player.getposition().X;
                if (this.position.X < playerX)
                {
                    xmovement = xspeed;
                }
                else if (this.position.X > playerX)
                {
                    xmovement = -xspeed;
                }
                this.position.X += xmovement;
                this.rectangle.X = (int)(this.position.X - (this.rectangle.Width / 2));
                this.position.Y += ymovement;
                this.rectangle.Y = (int)(this.position.Y - (this.rectangle.Width / 2));
            }

        }
    }
}
