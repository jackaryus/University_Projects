// File Author: Daniel Masterson
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.GameManagers
{
    /// <summary>
    /// Handles the camera
    /// </summary>
    public class CameraManager : AbstractManager
    {
        public static RectangleF CameraBounds { get; private set; }
        public static Vector2 CameraZoom { get; private set; }
        public static Camera ActiveCamera { get; private set; }

        public CameraManager()
        {
            ActiveCamera = new Camera();
            CameraBounds = new RectangleF(0, 0, Main.GraphicsDev.PresentationParameters.BackBufferWidth, Main.GraphicsDev.PresentationParameters.BackBufferHeight);
        }

        public static void SetActiveCamera(Camera newCamera)
        {
            ActiveCamera = newCamera;
        }

        public static void OnUpdateResolution()
        {
            if (ActiveCamera != null)
                ActiveCamera.OnResolutionUpdate();
        }

        public override void OnManagerUpdate(float delta)
        {
            if (ActiveCamera != null)
            {
                ActiveCamera.OnUpdate(delta);
                CameraBounds = ActiveCamera.GetBounds();
                CameraZoom = ActiveCamera.GetCameraZoom();
            }
        }
    }
}
