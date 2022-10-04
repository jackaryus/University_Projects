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

namespace Space_Banjo
{
    [Serializable]
    public class Sprite 
    {
        protected int health;
        protected bool isdead;
        protected SpriteType spritetype;
        protected int textureid;
        protected Vector2 position;
        protected Rectangle rectangle;
        protected float scale;
        [NonSerialized]
        protected Game1 game;

        // declaring different sprite types
        public enum SpriteType
        {
            player, mob, note
        }

        // setting everything needed when a sprite is created
        public Sprite(float x, float y, float scale, int textureid, Game1 game)
        {
            this.isdead = false;
            this.game = game;
            this.textureid = textureid;
            this.position = new Vector2(x, y);
            this.scale = scale;
            Texture2D texture = game.gettexture(textureid);
            int height = (int)(texture.Height * this.scale);
            int width = (int)(texture.Width * this.scale);
            this.rectangle = new Rectangle((int)(this.position.X - (width / 2)), (int)(this.position.Y - (height / 2)), width, height);
        }

        public void setgame(Game1 game)
        {
            this.game = game;
        }
        
        // general collision checker that is used to check all collisons between sprites
        public void checkcollision(Sprite other)
        {
            if (this.spritetype == other.spritetype)
                return;
            if (this.rectangle.Intersects(other.rectangle))
                oncollision(other);

        }

        public virtual void oncollision(Sprite other)
        {
            
        }

        // method to deduct health and check if its dead from that hit
        public void takehit()
        {
            this.health--;
            if (health == 0)
                this.isdead = true;
        }

        public Boolean isitdead()
        {
            return this.isdead;
        }

        public SpriteType getspritetype()
        {
            return this.spritetype;
        }

        public Vector2 getposition()
        {
            return this.position;
        }

        public virtual void update(GameTime gametime)
        {
            if (isdead)
                return;
        }

        public int gethealth()
        {
            return this.health;
        }

        public virtual void draw(SpriteBatch spritebatch)
        {
            // if the sprite is dead it wont draw if it is alive it will draw
            if (isdead)
                return;
            Texture2D texture = game.gettexture(textureid);
            spritebatch.Draw(texture, this.rectangle, Color.White);

        }

    }
}
