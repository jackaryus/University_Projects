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
    public class GameOverScene
    {

        private int timer;
        private Game1 game;
        public Texture2D background;

        public GameOverScene(Game1 game)
        {
            this.game = game;
            this.timer = 60 * 8;
        }

        public void Update()
        {
            if (timer > 0)
                timer -= 1;
            else
            {
                this.game.scene = Game1.Scene.Title;
                this.timer = 60 * 8;
                this.game.level.gameOverSoundInstance.Stop();
            }

        }

        public void Draw(ExtendedSpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.background, Vector2.Zero, Color.White);
        }

    }
}
