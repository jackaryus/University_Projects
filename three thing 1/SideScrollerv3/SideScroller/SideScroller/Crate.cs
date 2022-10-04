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
    class Crate : Entity
    {
        public Crate(int x, int y, int width, int height, Boolean rigid, Game1 game)
            : base(x, y, width, height, rigid, Type.crate, game)
        {
            this.collider.IsRigid = false;
        }


        public override void OnCollision(Entity other)
        {
           
        }

        public override void OnLeftCollision(Entity other)
        {
            
         
        }

        public override void OnRightCollision(Entity other)
        {
            

        }

        public override void OnTopCollision(Entity other)
        {
            if (other.type == Type.player) {

                if (Input.KeyDown(Keys.S) &&( Input.KeyDown(Keys.A)||Input.KeyDown(Keys.D))) {

                    this.Velocity.X = other.Velocity.X * 2f;
                
                }
            }
        }

        public override void OnBottomCollision(Entity other)
        {
        }


        public override void Update()
        {
            this.Velocity.X *= 0.8f;
            base.Update();

            this.collider.translate((int)this.Velocity.X, 0);
        }

        public override void Draw(ExtendedSpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.Draw(this.texture, new Vector2((int)(this.collider.X + offset.X), (int)(this.collider.Y + offset.Y)), Color.White);
        }
    }
}
