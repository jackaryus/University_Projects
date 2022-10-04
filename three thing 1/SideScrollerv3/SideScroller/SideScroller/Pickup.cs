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
    class Pickup : Entity
    {
        public Pickup(int x, int y, int width, int height, Boolean rigid, Game1 game)
            : base(x, y, width, height, rigid, Type.pickup, game)
        {
        }


        public override void OnCollision(Entity other)
        {
            if (other.type == Type.player && !this.destroyed) {
                this.Destroy();
                this.game.level.cheeseSlices += 1;
                this.game.level.gainedSlices += 1;
                this.game.level.Pickup.Play();
            }
        }

        public override void OnLeftCollision(Entity other)
        {


        }

        public override void OnRightCollision(Entity other)
        {


        }

        public override void OnTopCollision(Entity other)
        {
           
        }

        public override void OnBottomCollision(Entity other)
        {
        }


        public override void Update()
        {
            base.Update();

        }

        public override void Draw(ExtendedSpriteBatch spriteBatch, Vector2 offset)
        {
            if(!this.destroyed)
                spriteBatch.Draw(this.texture, new Vector2((int)(this.collider.X + offset.X), (int)(this.collider.Y + offset.Y)), Color.White);
        }
    }
}
