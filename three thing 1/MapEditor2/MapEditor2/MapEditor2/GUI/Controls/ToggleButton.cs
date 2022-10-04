using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MapEditor2.GUI.Controls
{
    //======================================================================================================================
    //
    //======================================================================================================================
    class ToggleButton
    {
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        protected Rectangle BodyRect, ContentRect;
        protected int padding;
        protected String Label;
        protected Editor editor;
        protected Containers.Panel Parent;
        protected Boolean Selected, Triggered;
        public Boolean IsPressed;
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public ToggleButton(int x, int y, int width, int height, String label, Containers.Panel parent, Editor editor)
        {
            this.BodyRect = new Rectangle(x, y, width, height);
            this.padding = 2;
            this.ContentRect = new Rectangle(x + padding, y + padding, width - (padding * 2), height - (padding * 2));
            this.Label = label;
            this.Parent = parent;
            this.editor = editor;
            this.Selected = false;
            this.Triggered = false;
            this.IsPressed = false;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public virtual Boolean CanSelect()
        {
            MouseState state = Mouse.GetState();
            return (state.X >= this.Parent.GetPosition().X + this.BodyRect.X &&
                state.X < this.Parent.GetPosition().X + this.BodyRect.Right &&
                state.Y >= this.Parent.GetPosition().Y + this.BodyRect.Y &&
                state.Y < this.Parent.GetPosition().Y + this.BodyRect.Bottom);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public Boolean IsTriggered()
        {
            return this.Triggered;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public virtual void OnTrigger()
        {
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void Update()
        {
            this.Triggered = false;

            if (!Selected)
            {
                if (CanSelect() && Input.InputHandler.mouseLeftTrigger())
                {
                    Selected = true;
                }
            }
            else
            {
                if (Input.InputHandler.mouseLeftUp())
                {
                    if (this.CanSelect())
                    {
                        this.Selected = false;
                        this.Triggered = true;
                        if (this.IsPressed)
                        {
                            this.IsPressed = false;
                        }
                        else
                        {
                            this.IsPressed = true;
                        }
                    }
                    else
                    {
                        Selected = false;
                    }
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void Draw()
        {
            this.editor.spriteBatch.FillRectangle(this.BodyRect, Color.White);

            Color borderColor = this.IsPressed ? Color.Red : Color.RoyalBlue;

            this.editor.spriteBatch.DrawRectangle(this.BodyRect, 2, borderColor);

        }

    }
}
