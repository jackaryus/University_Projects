// File Author: Daniel Masterson
using SpaceGame.GameManagers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Components
{
    /// <summary>
    /// An image
    /// </summary>
    public class ImageComponent : Component
    {
        /// <summary>
        /// An image
        /// </summary>
        /// <param name="texture">The image to render</param>
        public ImageComponent(string texture)
        {
            SetTexture(texture);
        }

        protected override void OnRender(float delta)
        {
            
            Draw(GraphicsManager.GetTexture(Texture), Bounds, null, Tint * Alpha, TrueRotation);
        }
    }
}
