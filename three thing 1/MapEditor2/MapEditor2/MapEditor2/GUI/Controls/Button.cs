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
    public class Button
    {
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        protected Rectangle BodyRect;
        protected int Padding;
        protected String Label;
        protected Boolean Selected, Triggered;
        protected Containers.Panel Parent;
        protected Editor editor;
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public Button(int x, int y, int width, int height, string label, Containers.Panel parent, Editor editor)
        {
            this.BodyRect = new Rectangle(x, y, width, height);
            this.Padding = 2;
            this.Label = label;
            this.Selected = false;
            this.Triggered = false;
            this.Parent = parent;
            this.editor = editor;
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
                    Selected = true;
            }
            else
            {
                if (Input.InputHandler.mouseLeftUp())
                {
                    if (this.CanSelect())
                    {
                        Selected = false;
                        this.Triggered = true;
                        OnTrigger();
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
            this.editor.spriteBatch.FillRectangle(this.BodyRect, Color.LightBlue);

            //content begin

            Vector2 size = this.editor.spriteBatch.MeasureString(this.Label);

            int X = (int)(this.BodyRect.X + ((this.BodyRect.Width / 2) - (size.X / 2)));
            int Y = (int)(this.BodyRect.Y + ((this.BodyRect.Height / 2) - (size.Y / 2)));

            this.editor.spriteBatch.DrawString(this.Label, new Vector2(X, Y), Color.Black);

            //content end

            Color color = this.Selected ? Color.Red : CanSelect() ? Color.RoyalBlue : Color.DarkGray;
            this.editor.spriteBatch.DrawRectangle(this.BodyRect, this.Padding, color);
        }
    }
}
