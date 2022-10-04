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
    class Projectile : Entity
    {
        public Entity spawner;
        public int life;
        private static Random random = new Random();

        public Projectile(int x, int y, int width, int height, Boolean rigid, Game1 game, Entity Spawner)
            : base(x, y, width, height, rigid, Type.projectile, game)
        {
            this.spawner = Spawner;
            this.life = 120;
            if (Spawner.type == Type.player)
            {
                Player player = (Player)spawner;
                if (player.direction == 0)
                {
                    this.Velocity.X = -8;
                    this.Velocity.Y = random.Next(-5, -2);
                }
                else
                {
                    this.Velocity.X = 8;
                    this.Velocity.Y = random.Next(-5, -2);
                }
            }
            else if (Spawner.type == Type.enemy)
            {
                Enemy enemy = (Enemy)spawner;
                this.Velocity.X = random.Next(-5, -1);
                this.Velocity.Y = random.Next(-3, -1);
                this.life = 60;
            }
            this.collider.IsSolid = false;
        }


        public override void OnCollision(Entity other)
        {
            if (this.spawner.type == Type.player && other.type == Type.enemy) {
                Enemy enemy = (Enemy)other;
                enemy.health -= 2;
                this.Destroy();
            }
            else if (this.spawner.type == Type.enemy && other.type == Type.player)
            {
                Enemy enemy = (Enemy)this.spawner;
                Player player = (Player)other;
                player.DecreaseHealth(enemy.damage);
                this.Destroy();
            }
            
        }

        public override void OnLeftCollision(Entity other)
        {
            if (other.collider.IsSolid)
                this.Velocity.X = 0;

        }

        public override void OnRightCollision(Entity other)
        {
            if (other.collider.IsSolid)
                this.Velocity.X = 0;

        }

        public override void OnTopCollision(Entity other)
        {

        }

        public override void OnBottomCollision(Entity other)
        {
        }


        public override void Update()
        {
            if ((int)this.Velocity.Y == 0 && (int)this.Velocity.X == 0)
            {
                this.destroyed = true;
                return;
            }
            if (this.destroyed)
                return;
            base.Update();
            this.Velocity.X *= 0.99f;
            this.collider.translate(this.Velocity.X, 0);
            if (this.life > 0)
                this.life -= 1;
            else
                this.Destroy();

        }

        public override void Draw(ExtendedSpriteBatch spriteBatch, Vector2 offset)
        {
            if (!this.destroyed)
                spriteBatch.Draw(this.game.level.stoneTexture, new Vector2((int)this.collider.X + (int)offset.X, (int)this.collider.Y + (int)offset.Y), Color.White);
        }
    }
}
