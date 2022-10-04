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
    public class TitleScene
    {
        private Game1 game;
        public Boolean soundPlaying;
        private string[] options;
        private int index;
        public Texture2D background;
        public SoundEffect introSound;
        SoundEffectInstance instance;

        public TitleScene(Game1 game)
        {
            this.game = game;
            this.soundPlaying = false;
            this.index = 0;
            this.options = new string[4] { "Start Game", "Instructions", "Credits", "Exit" };
        }

        public void Update()
        {
            if (!this.soundPlaying)
            {
                instance = this.introSound.CreateInstance();
                instance.IsLooped = true;
                instance.Play();
                this.soundPlaying = true;
            }
            if (Input.KeyTriggered(Keys.S) && this.index < this.options.Length - 1)
                this.index++;
            else if (Input.KeyTriggered(Keys.W) && this.index > 0)
                this.index--;
            if (Input.KeyTriggered(Keys.Enter))
            {
                switch (index)
                {
                    case 0:
                        this.game.scene = Game1.Scene.Map;
                        this.index = 0;
                        this.game.level.Initialize();
                        this.soundPlaying = false;
                        this.instance.Stop();
                        break;
                    case 1:
                        this.game.scene = Game1.Scene.instructions;
                        break;
                    case 2:
                        this.game.scene = Game1.Scene.credits;
                        break;
                    case 3:
                        this.game.Exit();
                        break;
                }
            }
            
        }

        public void Draw(ExtendedSpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.background, Vector2.Zero, Color.White);
            for (int i = 0; i < this.options.Length; i++)
            {
                string text = this.options[i];
                spriteBatch.DrawString(text, new Vector2(400 - (spriteBatch.MeasureString(text).X / 2), 210 + (i * 35)), 
                    i == this.index ? Color.Red : Color.Black);
            }
        }
    }
}
