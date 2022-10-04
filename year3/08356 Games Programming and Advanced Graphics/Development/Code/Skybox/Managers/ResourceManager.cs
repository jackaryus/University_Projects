using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    static class ResourceManager
    {
        static Dictionary<string, Geometry> geometryDictionary = new Dictionary<string, Geometry>();
        static Dictionary<string, Texture2D> textureDictionary = new Dictionary<string, Texture2D>();
        static Dictionary<string, Effect> effectDictionary = new Dictionary<string, Effect>();

        public static Geometry LoadGeometry(string filename)
        {
            Geometry geometry;
            geometryDictionary.TryGetValue(filename, out geometry);
            if (geometry == null)
            {
                geometry = new Geometry();
                geometry.LoadObject(filename);
                geometryDictionary.Add(filename, geometry);
            }

            return geometry;
        }

        public static Texture2D LoadTexture(string filename)
        {
            Texture2D texture;
            textureDictionary.TryGetValue(filename, out texture);
            if (texture == null)
            {
                texture = MyGame.gameInstance.Content.Load<Texture2D>(filename);
                textureDictionary.Add(filename, texture);
            }

            return texture;
        }

        public static Effect LoadEffect(string filename)
        {
            Effect effect;
            effectDictionary.TryGetValue(filename, out effect);
            if (effect == null)
            {
                effect = MyGame.gameInstance.Content.Load<Effect>(filename);
                effectDictionary.Add(filename, effect);
            }

            return effect;
        }
        
    }
}
