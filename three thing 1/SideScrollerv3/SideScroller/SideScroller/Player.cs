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
    public class Player : Entity
    {
        public int cooldown = 60, cooldown2 = 120;
        private int frame, frameTimer;
        public int direction;
        public int health {get; private set;}
        private int maxHealth = 6;

        public Player(int x, int y, int width, int height, Boolean rigid, Game1 game)
            : base(x, y, width, height, rigid, Type.player, game)
        {
            health = this.maxHealth;
            frame = direction = 0;
            frameTimer = 20;
        }

        public override void OnCollision(Entity other)
        {
            if (other.type == Type.ladder && Input.KeyDown(Keys.W)) {
                this.Velocity.Y *= 0.45f;
                this.IsGrounded = true;
            }
            else if (other.type == Type.end) {
                other.Destroy();
                this.game.scene = Game1.Scene.fortune;
            }
            else if (other.type == Type.kill) {
                if (this.game.level.lives <= 0)
                {
                    this.game.level.mainSoundInstance.Stop();
                    this.game.level.bossSoundInstance.Stop();
                    this.game.scene = Game1.Scene.gameOver;
                    this.game.level.gameOverSoundInstance.Play();
            }
                else
                {
                    this.game.level.lives -= 1;
                    this.game.level.cheeseSlices -= this.game.level.gainedSlices;
                    this.game.level.LoadLevel();
                    this.game.level.deathSound.Play();
                }
            }
        }

        public override void OnLeftCollision(Entity other)
        {
            if (cooldown == 0)
            {
                if (other.type == Type.enemy)
                {
                    Enemy enemy = (Enemy)other;
                    DecreaseHealth(enemy.damage);
                    cooldown = 120;
                }
            }

        }

        public override void OnRightCollision(Entity other)
        {
            if (cooldown == 0)
            {
                if (other.type == Type.enemy)
                {
                    Enemy enemy = (Enemy)other;
                    DecreaseHealth(enemy.damage);
                    cooldown = 60;
                }
            }
        }

        public override void OnTopCollision(Entity other)
        {
        }

        public override void OnBottomCollision(Entity other)
        {
            if (other.type == Type.enemy)
            {
                this.Velocity.Y = -10f;
                this.game.level.LandEnemy.Play();
            }
        }

        public void DecreaseHealth(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                health = 0;
                if (this.game.level.lives == 0)
                {
                    this.game.level.mainSoundInstance.Stop();
                    this.game.level.bossSoundInstance.Stop();
                    this.game.scene = Game1.Scene.gameOver;
                    this.game.level.gameOverSoundInstance.Play();
                }
                else
                {
                    this.game.level.lives -= 1;
                    this.game.level.cheeseSlices -= this.game.level.gainedSlices;
                    this.game.level.LoadLevel();
                    this.game.level.deathSound.Play();
                }
            }
        }

        public void IncreaseHealth(int amount)
        {
            this.health += amount;
            if (this.health > this.maxHealth)
                this.health = this.maxHealth;
        }

        public override void Update()
        {
            this.Velocity.X *= 0.8f;

            base.Update();

            if (cooldown > 0)
            {
                cooldown -= 1;
            }
            if (cooldown2 > 0)
                cooldown2 -= 1;

            if(Input.KeyDown(Keys.Space) && cooldown2 == 0)
            {
                int spawnX = this.direction == 0 ? -15 : 35;
                Level.entitiesToAdd.Add(new Projectile((int)this.collider.X + spawnX, (int)this.collider.Y, 10, 10, false, this.game, this));
                cooldown2 = 120;
                this.game.level.ThrowSound.Play();
            }

           // Console.WriteLine(cooldown);
            if (this.IsGrounded && Input.KeyDown(Keys.W))
            {
                this.Velocity.Y = -7;
            }

            if (Input.KeyDown(Keys.A))
            {
                this.direction = 0;
                this.Velocity.X -= 0.5f;
            }
            else if (Input.KeyDown(Keys.D))
            {
                this.direction = 1;
                this.Velocity.X += 0.5f;
            }
            if (!this.collider.translate((int)this.Velocity.X, 0))
                this.Velocity.X = 0;

            if ((int)this.Velocity.X != 0)
            {
                if (this.frameTimer > 0)
                    this.frameTimer--;
                else
                {
                    this.frameTimer = 20;
                    this.frame = this.frame == 3 ? 0 : this.frame + 1;
                }
            }
            else
            {
                this.frameTimer = 20;
                this.frame = 0;
            }
        }

        public override void Draw(ExtendedSpriteBatch spriteBatch, Vector2 offset)
        {
            Rectangle dest = new Rectangle((int)(this.collider.X + offset.X) - 4, (int)(this.collider.Y + offset.Y), 38, 30);
            Rectangle source = new Rectangle(this.frame * 38, this.direction * 30, 38, 30);
            spriteBatch.Draw(this.texture, dest, source, Color.White);
        }
    }
}
