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
    public class Enemy : Entity
    {
        public Boolean IsDead;
        public int health, damage, ScorePlus, frame, frameTimer, direction;

        public Enemy(int x, int y, Texture2D tex, int id, Game1 game)
            : base(x, y, tex.Width / 2, tex.Height / 2, true, Type.enemy, game)
        {
            this.direction = 0;
            this.IsDead = false;
            this.Velocity.X = 2f;
            if (id == 1)
            {
                this.health = 2;
                this.damage = 1;
                this.ScorePlus = 500;
            }
            else if (id == 2)
            {
                this.health = 4;
                this.damage = 2;
                this.ScorePlus = 1000;
            }
            this.texture = tex;
           
        }

        public override void OnCollision(Entity other)
        {

        }

        public override void OnLeftCollision(Entity other)
        {

            if (other.type == Type.player || other.type == Type.flag || other.type == Type.block)
            {
                this.Velocity.X = -this.Velocity.X;  
            }

        }

        public override void OnRightCollision(Entity other)
        {
            if (other.type == Type.player || other.type == Type.flag || other.type == Type.block)
            {
                this.Velocity.X = -this.Velocity.X;
            }

           
        }

        public override void OnTopCollision(Entity other)
        {
            if (other.type == Type.player)
            {
                health -= 1;
            }
        }

        public override void OnBottomCollision(Entity other)
        {
        }


        public override void Update()
        {
            if (this.IsDead)
                return;

            if (health <= 0)
            {
                this.Destroy();
                game.level.Score += this.ScorePlus;
                return;
            }

            this.IsGrounded = false;
            
            this.Velocity.Y += this.Gravity.Y;
            if (!this.collider.translate(0, (int)this.Velocity.Y))
            {
               if (this.Velocity.Y > 0)
                    this.IsGrounded = true;
                this.Velocity.Y *= 0.50f;
            }
            
            if (this.Velocity.X < 0)
                this.direction = 0;
            else if (this.Velocity.X > 0)
                this.direction = 1;
            if (this.frameTimer > 0)
                frameTimer -= 1;
            else
            {
                this.frame = this.frame == 1 ? 0: frame + 1;
                this.frameTimer = 20;
            }

            this.collider.translate((int)this.Velocity.X, 0);
        }
        public override void Draw(ExtendedSpriteBatch spriteBatch, Vector2 offset)
        {
            if (this.IsDead)
                return;
            Rectangle dest = new Rectangle((int)(this.collider.X + offset.X), (int)(this.collider.Y + offset.Y), (this.texture.Width / 2), (this.texture.Height / 2));
            Rectangle source = new Rectangle(this.frame * (this.texture.Width / 2), this.direction * (this.texture.Height / 2), (this.texture.Width / 2), (this.texture.Height / 2));
            spriteBatch.Draw(this.texture, dest, source, Color.White);
        }

        public override void Destroy()
        {
            base.Destroy();
            
            this.IsDead = true;

        }
    }
}
