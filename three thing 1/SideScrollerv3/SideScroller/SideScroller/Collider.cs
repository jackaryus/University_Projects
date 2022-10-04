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
    public class Collider
    {
        public Boolean IsRigid, IsSolid;
        public float X {get; private set;}
        public float Y { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Left { get; private set; }
        public float Right { get; private set; }
        public float Top { get; private set; }
        public float Bottom { get; private set; }
        public Entity entity { get; private set; }

        public Collider(float x, float y, float width, float height, Boolean rigid, Entity entity)
        {
            this.IsRigid = rigid;
            this.IsSolid = true;
            this.SetDimensions(width, height);
            this.SetPosition(x, y);
            this.entity = entity;
            CollisionManager.AddCollider(this);
        }

        public void HandleCollision(Collider other, float x, float y)
        {
            this.entity.OnCollision(other.entity);
            other.entity.OnCollision(this.entity);
            if (x > 0)
            {
                this.entity.OnRightCollision(other.entity);
                other.entity.OnLeftCollision(this.entity);
            }
            else if (x < 0)
            {
                this.entity.OnLeftCollision(other.entity);
                other.entity.OnRightCollision(this.entity);
            }
            if (y > 0)
            {
                this.entity.OnBottomCollision(other.entity);
                other.entity.OnTopCollision(this.entity);
            }
            else if (y < 0)
            {
                this.entity.OnTopCollision(other.entity);
                other.entity.OnBottomCollision(this.entity);
            }
        }

        public Boolean translate(float x, float y)
        {
            Boolean translated = true;
            this.SetPosition(this.X + x, this.Y + y);
            List<Collider> others = new List<Collider>();
            if (CollisionManager.TestCollision(this, ref others))
            {
                for (int i = 0; i < others.Count; i++)
                {
                    if (!others[i].IsSolid)
                    {
                        HandleCollision(others[i], x, y);
                    }
                    else if (others[i].IsRigid)
                    {
                        HandleCollision(others[i], x, y);
                        this.SetPosition(this.X - x, this.Y - y);
                        translated = false;
                        break;
                    }
                    else
                    {
                        HandleCollision(others[i], x, y);
                        if (!others[i].translate(x, y))
                        {
                            this.SetPosition(this.X - x, this.Y - y);
                            translated = false;
                            break;
                        }
                    }
                }
            }
            return translated;
        }

        private void SetPosition(float x, float y)
        {
            this.X = this.Left = x;
            this.Y = this.Top = y;
            this.Right = this.X + this.Width;
            this.Bottom = this.Y + this.Height;
        }

        private void SetDimensions(float width, float height)
        {
            this.Width = width; 
            this.Height = height;
        }

        public void Resize(int width, int height)
        {
            this.SetDimensions(width, height);
            this.SetPosition(this.X, this.Y);
        }

        public Boolean Intersects(Collider target)
        {
            return (target.X < this.Right && target.Right > this.X && target.Y < this.Bottom && target.Bottom  > this.Y);
        }
        
        public Rectangle GetRect()
        {
            return new Rectangle((int)this.X, (int)this.Y, (int)this.Width, (int)this.Height);
        }
    }
}
