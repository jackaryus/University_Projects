// File Author: Daniel Masterson
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Components
{
    /// <summary>
    /// A solid color
    /// </summary>
    public class SolidColorComponent : Component
    {
        /// <summary>
        /// A solid color
        /// </summary>
        /// <param name="color">This component's color</param>
        public SolidColorComponent(Color color)
        {
            SetTint(color);
        }

        protected override void OnRender(float delta)
        {
            Draw(Pixel, Bounds, null, Tint * Alpha, Rotation);
        }
    }
}
