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
    public class BossEnemy : Enemy
    {

        public int projectileCooldown;

        public BossEnemy(int x, int y, Texture2D tex, Game1 game)
            : base(x - 20, y, tex, 20, game)
        {
            this.collider.Resize(texture.Width / 3, this.texture.Height);
            this.Velocity.X = 0f;
           
            this.health = 15;
            this.damage = 3;
            this.ScorePlus = 10000;
            this.projectileCooldown = 30;
            
            this.texture = tex;
           
        }

        public override void Update()
        {
            if (this.health <= 0)
            {
                this.Destroy();
                this.game.scene = Game1.Scene.win;
                this.game.level.Score += this.ScorePlus;
                this.game.level.bossSoundInstance.Stop();
                this.game.level.winSoundInstance.Play();
            }

            if (this.destroyed)
                return;
            this.Velocity.Y += this.Gravity.Y;
            if (!this.collider.translate(0, (int)this.Velocity.Y))
            {
                if (this.Velocity.Y > 0)
                    this.IsGrounded = true;
                this.Velocity.Y *= 0.50f;
            }

            if (projectileCooldown > 0)
                projectileCooldown -= 1;

            if (projectileCooldown == 0)
            {
                int spawnX = -30;
                Level.entitiesToAdd.Add(new Projectile((int)this.collider.X + spawnX, (int)this.collider.Y, 10, 10, false, this.game, this));
                projectileCooldown = 30;
            }
        }

        public override void Draw(ExtendedSpriteBatch spriteBatch, Vector2 offset)
        {
            if (this.destroyed)
                return;
            Rectangle Dest = new Rectangle((int)this.collider.X + (int)offset.X, (int)this.collider.Y + (int)offset.Y, this.texture.Width / 3, this.texture.Height);
            Rectangle source = new Rectangle(this.frame * (this.texture.Width / 3), 0, this.texture.Width / 3, this.texture.Height);
            spriteBatch.Draw(this.texture, Dest, source, Color.White);
        }

    }
}
