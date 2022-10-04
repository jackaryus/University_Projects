// File Author: Daniel Masterson
using SpaceGame.GameManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    /// <summary>
    /// A camera that can be controlled by the player
    /// Features edge scrolling, mouse dragging and zooming via the scroll wheel
    /// </summary>
    public class EdgeScrollDragCamera : Camera
    {
        const int edgeBorder = 24;
        const float edgeMaxSpeed = 10.0f;

        Vector2 lastMousePosition;
        Vector2 velocity;
        const float velocityFadeTime = 0.9f;

        int lastScrollValue = 0;
        float targetZoom = 1.0f;
        float zoom = 1.0f;

        public override void OnUpdate(float delta)
        {
            if (InputManager.MouseScreenPosition.X <= edgeBorder)
                cameraBounds.X -= (int)((edgeMaxSpeed / edgeBorder) * (edgeBorder - InputManager.MouseScreenPosition.X) * zoom);
            else if (InputManager.MouseScreenPosition.X > Main.Resolution.X - edgeBorder)
                cameraBounds.X += (int)((edgeMaxSpeed / edgeBorder) * (edgeBorder - (Main.Resolution.X - InputManager.MouseScreenPosition.X)) * zoom);

            if (InputManager.MouseScreenPosition.Y < edgeBorder)
                cameraBounds.Y -= (int)((edgeMaxSpeed / edgeBorder) * (edgeBorder - InputManager.MouseScreenPosition.Y) * zoom);
            else if (InputManager.MouseScreenPosition.Y > Main.Resolution.Y - edgeBorder)
                cameraBounds.Y += (int)((edgeMaxSpeed / edgeBorder) * (edgeBorder - (Main.Resolution.Y - InputManager.MouseScreenPosition.Y)) * zoom);

            if (InputManager.MouseState.LeftButton == ButtonState.Pressed)
            {
                velocity = (InputManager.MouseScreenPositionV2 - lastMousePosition) * zoom;
            }

            cameraBounds.X -= velocity.X;
            cameraBounds.Y -= velocity.Y;
            velocity *= (1 - delta) * velocityFadeTime;

            targetZoom = Math.Max(0.1f, Math.Min(8.0f, targetZoom + 0.1f * ((lastScrollValue - InputManager.MouseState.ScrollWheelValue) / 120)));
            if (targetZoom != zoom)
            {
                zoom = UtilityManager.Lerp(zoom, targetZoom, 0.5f);

                float widthChange = cameraBounds.Width - (Main.Resolution.X * zoom);
                float heightChange = cameraBounds.Height - (Main.Resolution.Y * zoom);
                cameraBounds.Width = (int)(Main.Resolution.X * zoom);
                cameraBounds.Height = (int)(Main.Resolution.Y * zoom);
                cameraBounds.X += (int)(widthChange / 2);
                cameraBounds.Y += (int)(heightChange / 2);
            }

            if (cameraBounds.X < -cameraBounds.Width / 2)
                cameraBounds.X = -cameraBounds.Width / 2;
            else if(cameraBounds.X > SceneManager.ActiveScene.Bounds.Width - cameraBounds.Width / 2)
                cameraBounds.X = SceneManager.ActiveScene.Bounds.Width - cameraBounds.Width / 2;

            if (cameraBounds.Y < -cameraBounds.Height / 2)
                cameraBounds.Y = -cameraBounds.Height / 2;
            else if (cameraBounds.Y > SceneManager.ActiveScene.Bounds.Height - cameraBounds.Height / 2)
                cameraBounds.Y = SceneManager.ActiveScene.Bounds.Height - cameraBounds.Height / 2;


            lastScrollValue = InputManager.MouseState.ScrollWheelValue;
            lastMousePosition = InputManager.MouseScreenPositionV2;
        }
    }
}
