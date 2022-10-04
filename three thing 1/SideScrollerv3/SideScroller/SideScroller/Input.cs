using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace SideScroller
{
    public class Input
    {

        public static KeyboardState PreviousKeyState = Keyboard.GetState(), CurrentKeyState = Keyboard.GetState();
        public static MouseState PreviousMouseState = Mouse.GetState(), CurrentMouseState = Mouse.GetState();

        public static void Update()
        {
            PreviousKeyState = CurrentKeyState;
            CurrentKeyState = Keyboard.GetState();
            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }

        public static Boolean KeyDown(Keys key)
        {
            return (CurrentKeyState.IsKeyDown(key));
        }

        public static Boolean KeyTriggered(Keys key)
        {
            return (PreviousKeyState.IsKeyUp(key) && CurrentKeyState.IsKeyDown(key));
        }

        public static Boolean KeyReleased(Keys key)
        {
            return (PreviousKeyState.IsKeyDown(key) && CurrentKeyState.IsKeyUp(key));
        }

        public static Boolean LeftMouseDown()
        {
            return (CurrentMouseState.LeftButton == ButtonState.Pressed);
        }

        public static Boolean LeftMouseTriggered()
        {
            return (PreviousMouseState.LeftButton == ButtonState.Released && CurrentMouseState.LeftButton == ButtonState.Pressed);
        }

        public static Boolean LeftMouseReleased()
        {
            return (PreviousMouseState.LeftButton == ButtonState.Pressed && CurrentMouseState.LeftButton == ButtonState.Released);
        }

        public static Boolean RightMouseDown()
        {
            return (CurrentMouseState.RightButton == ButtonState.Pressed);
        }

        public static Boolean RightMouseTriggered()
        {
            return (PreviousMouseState.RightButton == ButtonState.Released && CurrentMouseState.RightButton == ButtonState.Pressed);
        }

        public static Boolean RightMouseReleased()
        {
            return (PreviousMouseState.RightButton == ButtonState.Pressed && CurrentMouseState.RightButton == ButtonState.Released);
        }

    }
}
