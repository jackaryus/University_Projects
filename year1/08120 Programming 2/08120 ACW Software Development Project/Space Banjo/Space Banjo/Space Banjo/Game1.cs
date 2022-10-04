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
using System.Runtime.Serialization.Formatters.Binary;


namespace Space_Banjo
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // declaring all the items needed
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        public Player player;
        List<Note> notes;
        List<Explosion> explosions;
        Banjo[] mobs;
        Random random = new Random();
        int[] seeds = { 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 2, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0};
        int score = 0;
        int highscore = 0;
        Texture2D background;
        int screenWidth;
        int screenHeight;
        List<Texture2D> textures;

        // declaring game states
        public enum GameState
        {
            Title, Running, GameOver
        }

        public GameState gameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // creating declared items and loading highscore
        protected override void Initialize()
        {
            this.gameState = GameState.Title;
            this.notes = new List<Note>();
            this.explosions = new List<Explosion>();
            this.mobs = new Banjo[100];
            base.Initialize();
            if (File.Exists("highscore.dat"))
            {
                this.loadhighscore();
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // loading all textures and creating objects
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.textures = new List<Texture2D>();
            this.textures.Add( this.Content.Load<Texture2D>("accordian"));
            this.textures.Add( this.Content.Load<Texture2D>("PlainBanjo"));
            this.textures.Add( this.Content.Load<Texture2D>("AttackerBanjo"));
            this.textures.Add( this.Content.Load<Texture2D>("DeadlyStrummer"));
            this.textures.Add(this.Content.Load<Texture2D>("note"));
            this.textures.Add(this.Content.Load<Texture2D>("explosion"));
            this.background = this.Content.Load<Texture2D>("background");
            screenWidth = this.GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenHeight = this.GraphicsDevice.PresentationParameters.BackBufferHeight;
            ResetGame();
            this.font = this.Content.Load<SpriteFont>("SpriteFont1");
        }

        // method to get textures of each object using id saved
        public Texture2D gettexture(int textureid)
        {
            return this.textures[textureid];
        }

        // if first time run then creates highscore file and saves the highscore if not it just saves the new highscore if it occurs
        protected void savehighscore()
        {
            FileStream stream = new FileStream("highscore.dat", FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this.highscore);
            stream.Close();
        }

        // loads highscore from saved file
        protected void loadhighscore()
        {
            FileStream stream = new FileStream("highscore.dat", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            this.highscore = (int)formatter.Deserialize(stream);
            stream.Close();
        }

        // saves all the required data 
        protected void SaveGame()
        {
            FileStream stream = new FileStream("save.dat", FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this.player);
            formatter.Serialize(stream, this.mobs);
            formatter.Serialize(stream, this.notes);
            formatter.Serialize(stream, this.score);
            stream.Close();
            this.savehighscore();
        }

        // loads the game from the saved file
        protected void LoadGame()
        {
            FileStream stream = new FileStream("save.dat", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            this.player = (Player)formatter.Deserialize(stream);
            this.mobs = (Banjo[])formatter.Deserialize(stream);
            this.notes = (List<Note>)formatter.Deserialize(stream);
            this.score = (int)formatter.Deserialize(stream);
            stream.Close();
            this.player.setgame(this);
            for (int i = 0; i < this.mobs.Length; i++)
            {
                if (this.mobs[i] != null)
                    this.mobs[i].setgame(this);
            }
            for (int i = 0; i < this.notes.Count; i++)
            {
                this.notes[i].setgame(this);
            }
        }

        //resets the player and mobs to the original spawn and removes any notes left over
        private void ResetGame()
        {
            this.player = new Player(400, 400, 0.2f, 0, this);
            for (int i = 0; i < 100; i++)
            {
                int type = seeds[random.Next(0, this.seeds.Length)];
                if (type == 0)
                {
                    this.mobs[i] = new Banjo(random.Next(0, 600), random.Next(0, 50), 0.15f, 1, this);
                }
                else if (type == 1)
                {
                    this.mobs[i] = new Hunter(random.Next(0, 600), random.Next(0, 50), 0.10f, 2, this);
                }
                else
                {
                    this.mobs[i] = new Deadly(random.Next(0, 600), random.Next(0, 50), 0.2f, 3, this);
                }
            }
            for (int i = 0; i < this.notes.Count; i++)
            {
                this.notes.RemoveAt(0);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        // method to remove notes when called
        public void removenote(Note note)
        {
            this.notes.Remove(note);
        }

        // mothod to remove mobs when they die
        public void removemob(int index)
        {
            this.mobs[index] = null;
        }

        // method to remove explosion when opacity = 0
        public void removeexplosion(Explosion explosion)
        {
            this.explosions.Remove(explosion);
        }

        // method to add explosion when a mob is destroyed
        public void addexplosion(Explosion explosion)
        {
            this.explosions.Add(explosion);
        }

        // method to increase score depending on the mob destroyed
        public void increasescore(int mobscore)
        {
            this.score += mobscore;
            if (this.score > this.highscore)
                this.highscore = this.score;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gametime)
        {
            KeyboardState keystate = Keyboard.GetState();
            // title state 
            if (this.gameState == GameState.Title)
            {
                // this loads an existing save when first started
                if (File.Exists("save.dat"))
                {
                    this.gameState = GameState.Running;
                    LoadGame();
                }
                // this is used to spawn mobs for attract mode when the menu is up
                for (int i = 0; i < 100; i++)
                {
                    if (this.mobs[i] != null)
                    {
                        this.mobs[i].update(gametime);
                    }
                    else
                    {
                        int type = seeds[random.Next(0, this.seeds.Length)];
                        if (type == 0)
                        {
                            this.mobs[i] = new Banjo(random.Next(0, 600), random.Next(0, 50), 0.15f, 1, this);
                        }
                        else if (type == 1)
                        {
                            this.mobs[i] = new Hunter(random.Next(0, 600), random.Next(0, 50), 0.10f, 2, this);
                        }
                        else
                        {
                            this.mobs[i] = new Deadly(random.Next(0, 600), random.Next(0, 50), 0.2f, 3, this);
                        }
                    }
                }
                // this changes the state to running when enter is pressed in the menu
                if (keystate.IsKeyDown(Keys.Enter))
                {
                    this.gameState = GameState.Running;
                    ResetGame();
                }

            }
            // running state
            else if (this.gameState == GameState.Running)
            {
                // this calls the save game method when s is pressed
                if (keystate.IsKeyDown(Keys.S))
                {
                    this.SaveGame();
                }
                this.player.update(gametime);
                for (int i = 0; i < 100; i++)
                {
                    if (this.mobs[i] != null)
                    {
                        // calls the update method for each mob alive and checks if a mob is off the bottom of the screen if so the player loses a life and the mob is destroyed
                        this.mobs[i].update(gametime);
                        if (this.mobs[i].getposition().Y > screenHeight)
                        {
                            this.player.takehit();
                            this.mobs[i] = null;
                        }
                    }
                    else
                    {
                        // spawns mobs randomly when others are destroyed
                        int type = seeds[random.Next(0, this.seeds.Length)];
                        if (type == 0)
                        {
                            this.mobs[i] = new Banjo(random.Next(0, 600), random.Next(0, 50), 0.15f, 1, this);
                        }
                        else if (type == 1)
                        {
                            this.mobs[i] = new Hunter(random.Next(0, 600), random.Next(0, 50), 0.10f, 2, this);
                        }
                        else
                        {
                            this.mobs[i] = new Deadly(random.Next(0, 600), random.Next(0, 50), 0.2f, 3, this);
                        }
                    }
                }

                // this is where the notes are created if space is pressed adding them to a list and also calling the update of each note created.
                if (keystate.IsKeyDown(Keys.Space))
                    this.notes.Add(new Note(player.getposition().X, player.getposition().Y, 0.1f, 4, this));
                for (int i = 0; i < notes.Count; i++)
                {
                    this.notes[i].update(gametime);
                }

                for (int i = 0; i < 100; i++)
                {
                    // this is where the collision checker is run between notes and mobs
                    this.mobs[i].checkcollision(this.player);
                    for (int j = 0; j < this.notes.Count; j++)
                    {
                        this.notes[j].checkcollision(this.mobs[i]);
                            
                    }
                }

                for (int i = 0; i < explosions.Count; i++)
                {
                    this.explosions[i].update();
                }

                // removes a mob if it is dead and changes to explosion 
                for (int i = 0; i < 100; i++)
                    if (this.mobs[i].isitdead())
                    {
                        this.addexplosion(new Explosion(this.mobs[i].getposition().X, this.mobs[i].getposition().Y, 0.2f, 5, this));
                        this.removemob(i);                        
                    }

            }
            // if the player dies this takes them to the game over screen and if a save game exists then it deletes it and saves the highscore
            if (player.isitdead())
            {
                this.gameState = GameState.GameOver;
                if (File.Exists("save.dat"))
                {
                    File.Delete("save.dat");
                }
                this.savehighscore();
            }
            if (this.gameState == GameState.GameOver)
            {
                // this takes you to the menu if you press escape while on the game over screen and also resets the game for attract mode in the menu
                if (keystate.IsKeyDown(Keys.Escape))
                {
                    this.gameState = GameState.Title;
                    ResetGame();

                }   
            }

            base.Update(gametime);
        }

        // this method draws the background texture 
        public void drawBackground()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(background, screenRectangle, Color.White);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();
            drawBackground();
            // draws the menu text and mob textures in attract mode
            if (this.gameState == GameState.Title)
            {
                for (int i = 0; i < 100; i++)
                {
                    if (this.mobs[i] != null)
                    {
                        this.mobs[i].draw(this.spriteBatch);
                    }
                }
                this.spriteBatch.DrawString(this.font, "Alien Banjo Attackers from Space", new Vector2(150, 200 ), Color.Red);
                this.spriteBatch.DrawString(this.font, "press enter to start", new Vector2(150, 250), Color.Red);
                this.spriteBatch.DrawString(this.font, "Highscore : " + highscore, new Vector2(225, 350), Color.Red);

            }
            // draws the game while playing and the text in the bottom right corner while updating when required
            if (this.gameState == GameState.Running)
            {
                this.player.draw(this.spriteBatch);
                for (int i = 0; i < 100; i++)
                {
                    if (this.mobs[i] != null)
                    {
                        this.mobs[i].draw(this.spriteBatch);
                    }
                }
                //draws notes that have been shot
                for (int i = 0; i < notes.Count; i++)
                {
                    this.notes[i].draw(this.spriteBatch);
                }
                // draws explosions which are active
                for (int i = 0; i < explosions.Count; i++)
                {
                    this.explosions[i].draw(this.spriteBatch);
                }
                this.spriteBatch.DrawString(this.font, "Score : " + score +" lives : " + player.gethealth(), new Vector2(20, 430), Color.Red);
                this.spriteBatch.DrawString(this.font, "Highscore : " + highscore , new Vector2(20, 400), Color.Red);
            }
            // draws the text on the game over screen
            if (this.gameState == GameState.GameOver)
            {
                
                this.spriteBatch.DrawString(this.font, "Game Over. Press escape to return to menu.", new Vector2(75, 250), Color.Red);
                this.spriteBatch.DrawString(this.font, " Your Score : " + score, new Vector2(225, 300), Color.Red);
            } 

            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
