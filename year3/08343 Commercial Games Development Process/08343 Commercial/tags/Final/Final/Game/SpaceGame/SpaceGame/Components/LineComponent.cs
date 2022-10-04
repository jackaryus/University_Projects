// File Author: Daniel Masterson
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Components
{
    /// <summary>
    /// A line
    /// </summary>
    public class LineComponent : Component
    {
        public Vector2 A { get; set; }
        public Vector2 B { get; set; }
        public int Width { get; set; }
        public Vector2 Midpoint { get; private set; }
        public float Length { get; private set; }

        private Rectangle renderRect = new Rectangle(0, 0, 1, 20);

        /// <summary>
        /// A line
        /// </summary>
        /// <param name="a">Starting point for the line</param>
        /// <param name="b">Ending point for the line</param>
        /// <param name="color">Color of the line</param>
        /// <param name="width">Width of the line</param>
        public LineComponent(Vector2 a, Vector2 b, Color color, int width = 1)
        {
            Width = width;
            SetTint(color);
            SetEndpoints(a, b);
        }

        protected override void OnRender(float delta)
        {
            Draw(Pixel, renderRect, null, Tint, Rotation, false);
        }

        /// <summary>
        /// Set the line end points
        /// </summary>
        /// <param name="a">Starting point for the line</param>
        /// <param name="b">Ending point for the line</param>
        /// <returns>This line component</returns>
        public LineComponent SetEndpoints(Vector2 a, Vector2 b)
        {
            A = a;
            B = b;
            Midpoint = Vector2.Lerp(a, b, 0.5f);
            Length = Vector2.Distance(a, b);
            SetRotation((float)Math.Atan2(A.Y - B.Y, A.X - B.X));

            renderRect = new Rectangle((int)(B.X), (int)(B.Y), (int)Length, Width);
            return this;
        }
    }
}
