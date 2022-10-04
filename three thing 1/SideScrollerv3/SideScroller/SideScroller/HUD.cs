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

using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SideScroller
{
    public class HUD
    {
        private Level level;
        public Player player;
        public Texture2D healthIcon;
        public HUD(Level level) {

            this.level = level;

        }

        public void setPlayer(Player player) {

            this.player = player;
        }

        public void Draw(ExtendedSpriteBatch spriteBatch) {
            for (int i = 0; i < player.health; i++) 
            {
                spriteBatch.Draw(this.healthIcon, new Vector2(10 + (i * 40), 10), Color.White);
            }
            spriteBatch.DrawString("Score: " + this.level.Score.ToString() + "\nLives: " + this.level.lives, new Vector2(10, 35), Color.Red);
            for (int i = 0; i < this.level.cheeseSlices; i++) {
                spriteBatch.Draw(this.level.pickupTexture, new Vector2(10 + (i * 40), 450), Color.White);
            }
        }
    }
}
