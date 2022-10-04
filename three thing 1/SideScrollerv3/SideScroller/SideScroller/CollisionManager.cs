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
    public class CollisionManager
    {

        private static List<Collider> Colliders = new List<Collider>();

        public static void AddCollider(Collider collider)
        {
            Colliders.Add(collider);
        }

        public static void RemoveCollider(Collider collider)
        {
            Colliders.Remove(collider);
        }

        public static Boolean TestCollision(Collider collider, ref List<Collider> others)
        {
            Boolean collided = false;
            foreach (var col in Colliders)
            {
                if (col == collider) continue;
                Vector2 distance = new Vector2(col.X - collider.X, col.Y - collider.Y);
                if (distance.X > col.Width && distance.X > collider.Width)
                    continue;
                else if (distance.X < -col.Width && distance.X < -collider.Width)
                    continue;
                else if (distance.Y > col.Height && distance.Y > collider.Height)
                    continue;
                else if (distance.Y < -col.Height && distance.Y < -collider.Height)
                    continue;

                if (collider.Intersects(col))
                {
                    others.Add(col);
                    collided = true;
                }
            }
            return collided;
        }

    }
}
