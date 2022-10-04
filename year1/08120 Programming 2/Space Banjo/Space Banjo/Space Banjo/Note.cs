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
    public class Note : Sprite
    {
        int ymovement = 5;
        public Note(float spritex, float spritey, float spritescale, int textureid, Game1 game)
            : base(spritex, spritey, spritescale, textureid, game) 
        {
            health = 1;
            this.spritetype = SpriteType.note;
        }

        // method for when a note hits a mob, delete note and minus health on mob hit and add the mobs score
        public override void oncollision(Sprite other)
        {
            if (other.getspritetype() == SpriteType.mob)
            {
                this.game.removenote(this);
                other.takehit();
                Banjo b = (Banjo)other;
                if (b.isitdead())
                    this.game.increasescore(b.getmobscore());
            }
        }

        public override void update(GameTime gametime)
        {
            base.update(gametime);
            // moves note up when fired and removes it if it hits the top of the screen
            this.position.Y -= ymovement;
            this.rectangle.Y = (int)(this.position.Y - (this.rectangle.Width / 2));
            if (this.rectangle.Bottom < 0)
                this.game.removenote(this);
        }
    }
}
