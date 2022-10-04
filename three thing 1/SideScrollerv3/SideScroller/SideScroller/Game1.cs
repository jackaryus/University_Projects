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
    
    //
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum Scene { SplashScreen, Title, instructions, Map, gameOver, fortune, cutscene, credits, win }
        public Scene scene;
        public GraphicsDeviceManager graphics;
        public ExtendedSpriteBatch spriteBatch;

        public SplashScreen splashScreen;
        public TitleScene titleScene;
        public Texture2D instructionsMenu, Credits, winScreen;
        public Level level;
        public WheelOfFortune wheelOfFortune;
        public GameOverScene gameOverScene;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.scene = Scene.SplashScreen;
            this.splashScreen = new SplashScreen(this);
            this.titleScene = new TitleScene(this);
            this.level = new Level(this);
            this.wheelOfFortune = new WheelOfFortune(this);
            this.gameOverScene = new GameOverScene(this);
            

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new ExtendedSpriteBatch(GraphicsDevice);
            spriteBatch.LoadContent(this.Content);

            // TODO: use this.Content to load your game content here
            this.splashScreen.Background = Content.Load<Texture2D>("logobackground");
            this.splashScreen.goat = Content.Load<Texture2D>("goat");
            this.titleScene.background = this.Content.Load<Texture2D>("MainMenu");
            this.instructionsMenu = this.Content.Load<Texture2D>("Instructions");
            this.Credits = this.Content.Load<Texture2D>("credits");
            this.level.Tileset = this.Content.Load<Texture2D>("tileset");
            this.level.playerTexture = this.Content.Load<Texture2D>("player");
            this.level.hud.healthIcon = this.Content.Load<Texture2D>("bannanaicon");
            this.level.crateTexture = this.Content.Load<Texture2D>("crate");
            this.level.pickupTexture = this.Content.Load<Texture2D>("cheeseslice");
            this.level.enemyTextures.Add(this.Content.Load<Texture2D>("bug1"));
            this.level.enemyTextures.Add(this.Content.Load<Texture2D>("bug2"));
            this.level.bossTexture = this.Content.Load<Texture2D>("boss");
            this.level.stoneTexture = this.Content.Load<Texture2D>("stone");
            this.winScreen = this.Content.Load<Texture2D>("Winner");

            this.splashScreen.goatSound = this.Content.Load<SoundEffect>("Goatsound");
            this.titleScene.introSound = this.Content.Load<SoundEffect>("introtune");
            this.level.mainSound = this.Content.Load<SoundEffect>("main");
            this.level.bossSound = this.Content.Load<SoundEffect>("bossman");
            this.level.LandEnemy = this.Content.Load<SoundEffect>("EnemyDamage");
            this.level.Pickup = this.Content.Load<SoundEffect>("PickUp");
            this.level.deathSound = this.Content.Load<SoundEffect>("death");
            this.level.winSound = this.Content.Load<SoundEffect>("end");
            this.level.ThrowSound = this.Content.Load<SoundEffect>("throw");
            this.level.gameOverSound = this.Content.Load<SoundEffect>("game_over");
            this.wheelOfFortune.spinSound = this.Content.Load<SoundEffect>("Wheelspin");

            for (int i = 1; i < 9; i++)
            {
                this.wheelOfFortune.wheelFrames.Add(this.Content.Load<Texture2D>("cheese" + i));
            }
            this.gameOverScene.background = this.Content.Load<Texture2D>("GameOver");
            
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            Input.Update();
            if (this.scene == Scene.SplashScreen)
                this.splashScreen.Update();
            else if (this.scene == Scene.Title)
                this.titleScene.Update();
            else if (this.scene == Scene.instructions || this.scene == Scene.credits)
            {
                if (Input.KeyTriggered(Keys.Escape))
                    this.scene = Scene.Title;
            }
            else if (this.scene == Scene.Map)
                this.level.Update();
            else if (this.scene == Scene.fortune)
                this.wheelOfFortune.Update();
            else if (this.scene == Scene.win)
            {
                if (Input.KeyTriggered(Keys.Enter))
                {
                    this.level.winSoundInstance.Stop();
                    this.scene = Scene.Title;
                }
            }

            else if (this.scene == Scene.gameOver)
                this.gameOverScene.Update();

            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();

            //draw
            if (this.scene == Scene.SplashScreen)
            {
                this.splashScreen.Draw(this.spriteBatch);
            }
            else if (this.scene == Scene.Title)
                this.titleScene.Draw(this.spriteBatch);
            else if (this.scene == Scene.instructions)
                this.spriteBatch.Draw(this.instructionsMenu, Vector2.Zero, Color.White);
            else if (this.scene == Scene.credits)
                this.spriteBatch.Draw(this.Credits, Vector2.Zero, Color.White);
            else if (this.scene == Scene.Map)
            {
                this.level.Draw(this.spriteBatch);
            }
            else if (this.scene == Scene.fortune)
                this.wheelOfFortune.Draw(this.spriteBatch);
            else if (this.scene == Scene.win)
            {
                this.spriteBatch.Draw(this.winScreen, Vector2.Zero, Color.White);
                this.spriteBatch.DrawString(this.level.Score.ToString(), new Vector2(350, 220), Color.Red);
            }
            else if (this.scene == Scene.gameOver)
                this.gameOverScene.Draw(this.spriteBatch);


            this.spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
