// File Author: Daniel Masterson
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.GameManagers
{
    /// <summary>
    /// Handles misc things
    /// </summary>
    public class UtilityManager : AbstractManager
    {
        public static Texture2D Pixel { get; private set; }
        public static Random Random { get; private set; }

        public UtilityManager()
        {
            Pixel = new Texture2D(Main.GraphicsDev, 1, 1);
            Pixel.SetData<Color>(new Color[] { Color.White });

            Random = new Random();
        }

        public static void SeedRandom(int seed)
        {
            Random = new Random(seed);
        }

        public static float Lerp(float A, float B, float Alpha)
        {
            return A + (B - A) * Alpha;
        }
    }
}
