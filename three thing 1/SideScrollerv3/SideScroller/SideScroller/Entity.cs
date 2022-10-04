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
    public class Entity
    {
        protected Game1 game;
        public enum Type { none, player, enemy, block, flag, ladder, crate, end, kill, pickup, projectile};
        public Type type;
        public Collider collider;
        protected Texture2D texture;
        public Boolean IsGrounded, destroyed;
        public Vector2 Velocity, Gravity;
        public int TexId;

        public Entity(float x, float y, float width, float height, Boolean rigid, Type type, Game1 game)
        {
            this.game = game;
            this.destroyed = false;
            this.type = type;
            this.TexId = 0;
            this.collider = new Collider(x, y, width, height, rigid, this);
            this.Velocity = new Vector2(0, 0);
            this.Gravity = new Vector2(0, 0.2f);
        }

        public void SetTexture(Texture2D tex)
        {
            this.texture = tex;
        }

        public void SetTexId(int id)
        {
            this.TexId = id;
        }

        public virtual void OnCollision(Entity other)
        {
        }

        public virtual void OnLeftCollision(Entity other)
        {
        }

        public virtual void OnRightCollision(Entity other)
        {
        }

        public virtual void OnTopCollision(Entity other)
        {
        }

        public virtual void OnBottomCollision(Entity other)
        {
        }

        public virtual void Update()
        {
            this.IsGrounded = false;
            if (!this.collider.IsRigid)
            {
                this.Velocity.Y += this.Gravity.Y;
                if (!this.collider.translate(0, (int)this.Velocity.Y))
                {
                    if (this.Velocity.Y > 0)
                        this.IsGrounded = true;
                    this.Velocity.Y *= 0.50f;
                }
            }
        }

        public virtual void Draw(ExtendedSpriteBatch spriteBatch, Vector2 offset)
        {
            if (this.texture != null && this.TexId != 0)
            {
                DrawCollider(spriteBatch, offset);
            }
        }

        public void DrawCollider(ExtendedSpriteBatch spriteBatch, Vector2 offset)
        {
            Rectangle container = this.collider.GetRect();
            container.X += (int)offset.X;
            container.Y += (int)offset.Y;
            spriteBatch.DrawRectangle(container, 2, Color.Red);
        }

        public virtual void Destroy()
        {
            CollisionManager.RemoveCollider(this.collider);
            this.destroyed = true;
        }
    }
}
