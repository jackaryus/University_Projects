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
    public class WheelOfFortune
    {
        private Game1 game;
        private Random random;
        private string text;
        private Boolean running, spinning, DrawText;
        private int wheelFrame = 0;
        private int playerFrame, playerFrameTimer, wheelframeTimer, speed, Loops;
        private Vector2 playerPosition;
        public List<Texture2D> wheelFrames;
        public SoundEffect spinSound;

        public WheelOfFortune(Game1 game)
        {
            this.text = "Welcome to the Wheel of\nfortune, press enter to\nspend cheese tokens for a \nchance to win a great prize,\n or escape if you dont want\n to risk it!";
            this.random = new Random();
            random.Next();
            this.game = game;
            this.wheelFrames = new List<Texture2D>();
            Initialize();
        }

        private void Initialize()
        {
            this.running = true;
            this.spinning = false;
            this.DrawText = false;
            this.playerFrame = 0;
            this.speed = random.Next(3, 6);
            this.Loops = random.Next(10, 30);
            this.playerFrameTimer = 20;
            this.wheelframeTimer = 20 / speed;
            this.playerPosition = new Vector2(-38, 417);
        }

        public void Update()
        {
            if (this.running)
            {
                if (playerPosition.X < 480)
                {
                    playerPosition.X += 4;
                    if (this.playerFrameTimer > 0)
                        this.playerFrameTimer -= 1;
                    else
                    {
                        this.playerFrameTimer = 20;
                        this.playerFrame = this.playerFrame == 3 ? 0 : this.playerFrame + 1;
                    }
                }
                else if (!this.spinning)
                {
                    this.DrawText = true;
                    if (Input.KeyTriggered(Keys.Enter))
                    {
                        if (this.game.level.cheeseSlices > 0)
                        {
                            this.game.level.cheeseSlices -= 1;
                            this.spinning = true;
                        }
                        else
                        {
                            this.running = false;
                        }
                    }
                    else if (Input.KeyTriggered(Keys.Escape))
                        this.running = false;
                }
                else
                {
                    if (wheelframeTimer > 0)
                    {
                        this.wheelframeTimer -= 1;
                    }
                    else
                    {
                        this.spinSound.Play();
                        this.speed = this.speed > 1 ? this.speed - 1 : 1;
                        this.wheelframeTimer = 20 / speed;
                        this.wheelFrame = this.wheelFrame < 7 ? this.wheelFrame + 1 : 0;
                        if (wheelFrame > 7)
                            wheelFrame -= 8;
                        this.Loops -= 1;
                        if (Loops == 0)
                            GetPrize();
                    }
                }
            }
            else
            {
                if (Input.KeyTriggered(Keys.Enter) && this.spinning)
                {
                    this.game.level.levelCount += 1;
                    this.game.level.LoadNextLevel = true;
                    this.game.scene = Game1.Scene.Map;
                    this.Initialize();
                }
                else if (!this.spinning)
                {
                    this.game.level.levelCount += 1;
                    this.game.level.LoadNextLevel = true;
                    this.game.scene = Game1.Scene.Map;
                    this.Initialize();
                }
            }

        }

        public void GetPrize()
        {
            this.spinning = false;
            switch (this.wheelFrame)
            {

                case 0:
                    this.game.level.lives += 3;
                    break;
                case 1:
                    this.game.level.Score += 1000;
                    break;
                case 2:
                    this.game.level.Score += 500;
                    break;
                case 3:
                    this.game.level.Score -= 1000;
                    break;
                case 4:
                    this.game.level.lives -= 1;
                    break;
                case 5:
                    this.game.level.lives += 1;
                    break;
                case 6:
                    break;
                case 7:
                    this.game.level.Score -= 500;
                    break;
            }
            this.speed = random.Next(3, 6);
            this.Loops = random.Next(10, 30);
            this.wheelframeTimer = 20 / speed;
            
        }

        public void Draw(ExtendedSpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.wheelFrames[this.wheelFrame], Vector2.Zero, Color.White);

            Rectangle source = new Rectangle(this.playerFrame * 38, 30, 38, 30);
            spriteBatch.Draw(this.game.level.playerTexture, this.playerPosition, source, Color.White);

            if (this.DrawText)
            {
                
                spriteBatch.DrawString(text, new Vector2(30, 70), Color.Black);
                spriteBatch.DrawString("tokens: " + this.game.level.cheeseSlices, new Vector2(10, 390), Color.Black);
            }
        }
    }
}
