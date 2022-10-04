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
    class Deadly : Hunter
    {
        public Deadly(float spritex, float spritey, float spritescale, int textureid, Game1 game)
            : base(spritex, spritey, spritescale, textureid, game)
        {
            health = 2;
            this.xspeed = 4;
            this.yspeed = 4;
            // this sets chase time as 0 to begin with so the deadly strummer chases player straight away
            chasetime = 0;
            mobscore = 50;
        }

    }
}
