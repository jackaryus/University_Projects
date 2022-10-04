using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace MapEditor2.Input
{
    //============================================================================================
    //
    //============================================================================================
    public class InputHandler
    {
        //----------------------------------------------------------------------------------------
        //
        //----------------------------------------------------------------------------------------
        private static MouseState lastMouseState = Mouse.GetState();
        private static MouseState currentMouseState = Mouse.GetState();

        private static int mouseScrolledX = 0;
        private static int mouseScrolledY = 0;
        //----------------------------------------------------------------------------------------
        //
        //----------------------------------------------------------------------------------------
        public static void update()
        {
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            mouseScrolledX = currentMouseState.X - lastMouseState.X;
            mouseScrolledY = currentMouseState.Y - lastMouseState.Y;


        }
        //----------------------------------------------------------------------------------------
        //
        //----------------------------------------------------------------------------------------
        public static MouseState GetMouseState()
        {
            return currentMouseState;
        }
        //----------------------------------------------------------------------------------------
        //
        //----------------------------------------------------------------------------------------
        public static Boolean mouseLeftClicked()
        {
            return (currentMouseState.LeftButton == ButtonState.Released && 
                lastMouseState.LeftButton == ButtonState.Pressed);

        }
        //----------------------------------------------------------------------------------------
        //
        //----------------------------------------------------------------------------------------
        public static Boolean mouseLeftDown()
        {
            return (currentMouseState.LeftButton == ButtonState.Pressed);
        }
        //----------------------------------------------------------------------------------------
        //
        //----------------------------------------------------------------------------------------
        public static Boolean mouseLeftUp()
        {
            return (currentMouseState.LeftButton == ButtonState.Released);
        }
        //----------------------------------------------------------------------------------------
        //
        //----------------------------------------------------------------------------------------
        public static Boolean mouseLeftTrigger()
        {
            return (currentMouseState.LeftButton == ButtonState.Pressed && 
                lastMouseState.LeftButton == ButtonState.Released);
        }
        //----------------------------------------------------------------------------------------
        //
        //----------------------------------------------------------------------------------------
        public static Boolean mouseRightClicked()
        {
            return (currentMouseState.RightButton == ButtonState.Released &&
                lastMouseState.RightButton == ButtonState.Pressed);

        }
        //----------------------------------------------------------------------------------------
        //
        //----------------------------------------------------------------------------------------
        public static Boolean mouseRightDown()
        {
            return (currentMouseState.RightButton == ButtonState.Pressed);
        }
        //----------------------------------------------------------------------------------------
        //
        //----------------------------------------------------------------------------------------
        public static Boolean mouseRightUp()
        {
            return (currentMouseState.RightButton == ButtonState.Released);
        }
        //----------------------------------------------------------------------------------------
        //
        //----------------------------------------------------------------------------------------
        public static Boolean mouseRightTrigger()
        {
            return (currentMouseState.RightButton == ButtonState.Pressed &&
                lastMouseState.RightButton == ButtonState.Released);
        }
        //----------------------------------------------------------------------------------------
        //
        //----------------------------------------------------------------------------------------
        public static int getMouseScrolledX()
        {
            return mouseScrolledX;
        }
        //----------------------------------------------------------------------------------------
        //
        //----------------------------------------------------------------------------------------
        public static int getMouseScrolledY()
        {
            return mouseScrolledY;
        }
    }
}
