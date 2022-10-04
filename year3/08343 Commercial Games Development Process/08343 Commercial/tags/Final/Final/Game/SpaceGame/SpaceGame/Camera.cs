// File Author: Daniel Masterson
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    /// <summary>
    /// A basic camera
    /// </summary>
    public class Camera
    {
        protected RectangleF cameraBounds = new RectangleF(0, 0, 800, 600);
        protected Vector2 cameraZoom { get { return new Vector2((float)Main.Resolution.X / cameraBounds.Width, (float)Main.Resolution.Y / cameraBounds.Height); } }

        public Camera()
        {
            OnResolutionUpdate();
        }

        public virtual void OnUpdate(float delta) { }

        public virtual void OnResolutionUpdate()
        {
            cameraBounds = new RectangleF(0, 0, Main.Resolution.X, Main.Resolution.Y);
        }

        public RectangleF GetBounds()
        {
            return cameraBounds;
        }

        public Vector2 GetCameraZoom()
        {
            return cameraZoom;
        }

        public void SetCameraPosition(Vector2 newPosition)
        {
            cameraBounds.X = newPosition.X;
            cameraBounds.Y = newPosition.Y;
        }

        public void SetCameraBounds(RectangleF newBounds)
        {
            cameraBounds = newBounds;
        }
    }
}
