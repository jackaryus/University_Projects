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
    public class Level
    {
        private Game1 game;
        public Boolean LoadNextLevel;
        public int levelCount;
        public int lives, Score, cheeseSlices, gainedSlices;
        public Player player;
        public HUD hud;
        public Texture2D Tileset, playerTexture, crateTexture, pickupTexture, bossTexture, stoneTexture;
        public List<Texture2D> enemyTextures;
        public static List<Entity> entities, entitiesToAdd;
        public Vector2 DisplayPosition;

        public int[][,] MapData;
        public int[,] MapFlags;

        public SoundEffect mainSound, bossSound, LandEnemy, Pickup, deathSound, winSound, ThrowSound, gameOverSound;
        public SoundEffectInstance mainSoundInstance, bossSoundInstance, winSoundInstance, gameOverSoundInstance;

        public Level(Game1 game)
        {
            this.LoadNextLevel = false;
            this.enemyTextures = new List<Texture2D>();
            this.game = game;
            this.levelCount = 1;
            this.lives = 2;
            this.Score = 0;
            this.cheeseSlices = this.gainedSlices = 0;
            this.hud = new HUD(this);
            this.DisplayPosition = new Vector2(0, 0);
        }

        public void Initialize()
        {
            this.mainSoundInstance = this.mainSound.CreateInstance();
            this.mainSoundInstance.IsLooped = true;
            this.bossSoundInstance = bossSound.CreateInstance();
            this.bossSoundInstance.IsLooped = true;
            this.winSoundInstance = this.winSound.CreateInstance();
            this.gameOverSoundInstance = this.gameOverSound.CreateInstance();
            this.levelCount = 1;
            this.lives = 2;
            this.Score = 0;
            this.cheeseSlices = 0;
            this.gainedSlices = 0;
            this.LoadLevel();
        }

        public void LoadLevel()
        {
            if (this.levelCount == 3)
            {
                this.mainSoundInstance.Stop();
                this.bossSoundInstance.Play();
            }
            else
            {
                this.mainSoundInstance.Play();
            }
            this.gainedSlices = 0;
            this.LoadNextLevel = false;
            if (entitiesToAdd != null)
            {
                for (int i = 0; i < entitiesToAdd.Count; i++)
                {
                    entitiesToAdd[i].Destroy();
                }
            }
            if (entities != null)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    entities[i].Destroy();
                }
                this.player = null;
            }
            entities = new List<Entity>();
            entitiesToAdd = new List<Entity>();

            System.IO.FileStream stream = File.Open("map" + this.levelCount.ToString() + ".TM", FileMode.OpenOrCreate, FileAccess.Read);
            BinaryFormatter serializer = new BinaryFormatter();
            this.MapData = (int[][,])serializer.Deserialize(stream);
            this.MapFlags = (int[,])serializer.Deserialize(stream);
            stream.Close();

            for (int x = 0; x < this.MapData[1].GetLength(0); x++)
            {
                for (int y = 0; y < this.MapData[1].GetLength(1); y++)
                {
                    int flag = MapFlags[x, y];
                    if (MapData[1][x, y] != 0)
                        entities.Add(new Entity(x * 30, y * 30, 30, 30, true, Entity.Type.block, this.game));

                    if (flag == 10)
                    {
                        entities.Add(new Entity(x * 30, y * 30, 30, 30, true, Entity.Type.flag, this.game));
                        entities[entities.Count - 1].collider.IsSolid = false;
                    }
                    if (flag >= 1 && flag <= 9)
                    {
                        entities.Add(new Enemy(x * 30, y * 30, this.enemyTextures[flag - 1], flag, this.game));
                    }
                    if (flag == 11)
                    {
                        entities.Add(this.player = new Player(x * 30, y * 30, 30, 30, false, this.game));
                        player.SetTexture(this.playerTexture);
                        this.hud.setPlayer(this.player);
                    }
                    if (flag == 12) {
                        entities.Add(new Entity(x * 30, y * 30, 30, 30, true, Entity.Type.ladder, this.game));
                        entities[entities.Count - 1].collider.IsSolid = false;
                    }

                    if (flag == 13)
                    {
                        entities.Add(new Crate(x * 30, y * 30, 30, 30, false, this.game));
                        entities[entities.Count - 1].SetTexture(this.crateTexture);
                    }

                    if (flag == 14)
                    {
                        entities.Add(new Entity(x * 30, y * 30, 30, 30, true, Entity.Type.end, this.game));
                        entities[entities.Count - 1].collider.IsSolid = false;
                    }

                    if (flag == 15)
                    {
                        entities.Add(new Entity(x * 30, y * 30, 30, 30, true, Entity.Type.kill, this.game));
                        entities[entities.Count - 1].collider.IsSolid = true;
                    }
                    if (flag == 16)
                    {
                        entities.Add(new Pickup(x * 30, y * 30, 15, 15, false, this.game));
                        entities[entities.Count - 1].SetTexture(this.pickupTexture);
                    }
                    if (flag == 20)
                    {
                        entities.Add(new BossEnemy(x * 30, y * 30, this.bossTexture, this.game));
                    }
                }
            }
            this.CenterMap();
        }

        public void CenterMap()
        {
            this.DisplayPosition.X = 400 - this.player.collider.X;
            this.DisplayPosition.Y = 240 - this.player.collider.Y;
            if (this.DisplayPosition.X > 0)
                this.DisplayPosition.X = 0;
            else if (this.DisplayPosition.X < -((this.MapData[0].GetLength(0) * 30) - 800))
                this.DisplayPosition.X = -((this.MapData[0].GetLength(0) * 30) - 800);
            if (this.DisplayPosition.Y > 0)
                this.DisplayPosition.Y = 0;
            else
            if (this.DisplayPosition.Y < -((this.MapData[0].GetLength(1) * 30) - 480))
                this.DisplayPosition.Y = -((this.MapData[0].GetLength(1) * 30) - 480);
        }

        public void Update()
        {
            if (this.LoadNextLevel)
                this.LoadLevel();
            if (entitiesToAdd.Count > 0)
            {
                for (int i = 0; i < entitiesToAdd.Count; i++)
                {
                    entities.Add(entitiesToAdd[i]);
                }
                entitiesToAdd = new List<Entity>();
            }
            player.Update();
            foreach (var entity in entities)
                entity.Update();

            this.CenterMap();
        }

        public void Draw(ExtendedSpriteBatch spriteBatch)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int x = 0; x < this.MapData[i].GetLength(0); x++)
                {
                    int TX = (int)this.DisplayPosition.X + (x * 30);
                    for (int y = 0; y < this.MapData[i].GetLength(1); y++)
                    {
                        int TY = (int)this.DisplayPosition.Y + (y * 30);
                        int index = this.MapData[i][x, y];
                        if (index == 0)
                            continue;
                        Rectangle source = new Rectangle(index % 8 * 30, index / 8 * 30, 30, 30);
                        spriteBatch.Draw(this.Tileset, new Rectangle(TX, TY, 30, 30), source, Color.White);
                    }
                }
            }

            for (int i = 0; i < entities.Count; i++)
                entities[i].Draw(spriteBatch, this.DisplayPosition);

            for (int x = 0; x < this.MapData[2].GetLength(0); x++)
            {
                int TX = (int)this.DisplayPosition.X + (x * 30);
                for (int y = 0; y < this.MapData[2].GetLength(1); y++)
                {
                    int TY = (int)this.DisplayPosition.Y + (y * 30);
                    int index = this.MapData[2][x, y];
                    if (index == 0)
                        continue;
                    Rectangle source = new Rectangle(index % 8 * 30, index / 8 * 30, 30, 30);
                    spriteBatch.Draw(this.Tileset, new Rectangle(TX, TY, 30, 30), source, Color.White);
                }
            }

            this.hud.Draw(spriteBatch);
        }
    }
}
