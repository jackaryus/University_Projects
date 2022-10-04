// File Author: Daniel Masterson
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.GameManagers
{
    public enum EMouseState
    {
        None,
        Over,
        Down,
    }

    /// <summary>
    /// Manages player input
    /// </summary>
    public class InputManager : AbstractManager
    {
        public static KeyboardState KeyboardState { get; private set; }
        public static MouseState MouseState { get; private set; }

        public static Point MouseScreenPosition { get; private set; }
        public static Point MouseWorldPosition { get; private set; }
        public static Vector2 MouseScreenPositionV2 { get; private set; }
        public static Vector2 MouseWorldPositionV2 { get; private set; }

        public override void OnManagerUpdate(float delta)
        {
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();

            MouseScreenPositionV2 = new Vector2((float)Math.Max(0.0, Math.Min(Main.Resolution.X, MouseState.X)), (float)Math.Max(0.0, Math.Min(Main.Resolution.Y, MouseState.Y)));
            MouseWorldPositionV2 = new Vector2(CameraManager.CameraBounds.X + MouseScreenPositionV2.X * (1 / CameraManager.CameraZoom.X), CameraManager.CameraBounds.Y + MouseScreenPositionV2.Y * (1 / CameraManager.CameraZoom.Y));

            MouseScreenPosition = new Point((int)MouseScreenPositionV2.X, (int)MouseScreenPositionV2.Y);
            MouseWorldPosition = new Point((int)MouseWorldPositionV2.X, (int)MouseWorldPositionV2.Y);
        }

        public override void OnManagerRender(float delta)
        {
            SpriteBatch.Draw(UtilityManager.Pixel, new Rectangle(MouseScreenPosition.X - 2, MouseScreenPosition.Y - 2, 2, 8), Color.White);
            SpriteBatch.Draw(UtilityManager.Pixel, new Rectangle(MouseScreenPosition.X - 2, MouseScreenPosition.Y - 2, 8, 2), Color.White);
        }

        public static Point GetMousePosition(ERenderSpace space)
        {
            switch (space)
            {
                default:
                case ERenderSpace.ScreenSpace:
                    return MouseScreenPosition;
                case ERenderSpace.WorldSpace:
                    return MouseWorldPosition;
            }
        }

        public static Vector2 GetMousePositionV2(ERenderSpace space)
        {
            switch (space)
            {
                default:
                case ERenderSpace.ScreenSpace:
                    return MouseScreenPositionV2;
                case ERenderSpace.WorldSpace:
                    return MouseWorldPositionV2;
            }
        }
    }

    public class MouseEvent
    {
        public bool Handled = false;
    }
}
