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
    public class Player: Sprite 
    {
        public Player(float spritex, float spritey, float spritescale, int textureid, Game1 game)
        :base(spritex, spritey, spritescale, textureid, game) 
        {
            health = 3;
            this.spritetype = SpriteType.player;
        }

        // moves the player left and right depending on the value used 
        public void moveplayer(float xmovement)
        {
            this.position.X += xmovement;
            this.rectangle.X = (int)(this.position.X - (this.rectangle.Width / 2));
        }

        public override void update(GameTime gametime)
        {
            base.update(gametime);
            KeyboardState keystate = Keyboard.GetState();
            // calls the move method when a button is pressed and if the player is within the limits
            if (keystate.IsKeyDown(Keys.Left) && this.rectangle.X > 0)
            {
                moveplayer(-3);
            }
            else if (keystate.IsKeyDown(Keys.Right) && this.rectangle.Right < 800)
            {
                moveplayer(3);
            }
        }

        
    }
}
