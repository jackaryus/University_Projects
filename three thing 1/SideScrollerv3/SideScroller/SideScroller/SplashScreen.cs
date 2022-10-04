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

namespace SideScroller
{
    public class SplashScreen
    {
        private Game1 game;
        public Vector2 GoatPostion;
        public Texture2D Background, goat;
        public SoundEffect goatSound;
        public Boolean SoundPlaying;
        private SoundEffectInstance instance;

        public SplashScreen(Game1 game)
        {
            this.game = game;
            this.GoatPostion = new Vector2(800, 300);
        }

        public void Update()
        {
            if (!SoundPlaying)
            {
                instance = this.goatSound.CreateInstance();
                instance.Play();
                SoundPlaying = true;
            }
                
            if (this.GoatPostion.X > 440)
            {
                this.GoatPostion.X -= 4;
            }
            else if (this.GoatPostion.Y > -this.goat.Height)
            {
                this.GoatPostion.Y -= 6;
            }
            else
            {
                this.game.scene = Game1.Scene.Title;
                SoundPlaying = false;
                instance.Stop();
            }
        }

        public void Draw(ExtendedSpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Background, Vector2.Zero, Color.White);
            spriteBatch.Draw(this.goat, this.GoatPostion, Color.White);
        }

    }
}
