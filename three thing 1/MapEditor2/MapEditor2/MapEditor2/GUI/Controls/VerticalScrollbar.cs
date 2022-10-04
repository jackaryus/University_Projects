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
    public class VerticalScrollbar
    {
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private Rectangle BodyRect, TrackRect, SliderRect;
        private int padding, ContentHeight, ScrolledY;
        private Boolean Dragging;
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public VerticalScrollbar(int x, int y, int height, int padding)
        {
            this.padding = padding;
            this.BodyRect = new Rectangle(x, y, 20, height);
            this.TrackRect = new Rectangle(x + padding, y + padding, 20 - (padding * 2), 
                height - (padding * 2));
            this.ContentHeight = height - (padding * 2);
            this.ScrolledY = 0;
            this.SliderRect = new Rectangle(x + padding, y + padding + this.ScrolledY, 20 - (padding * 2), 
                GetSliderHeight());
            this.Dragging = false;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private int GetSliderHeight()
        {
            float TrackHeight = this.BodyRect.Height - (this.padding * 2);

            if (this.ContentHeight <= TrackHeight)
                return (int)TrackHeight;

            float percent = TrackHeight / this.ContentHeight;
            float size = percent * TrackHeight;

            return (int)size;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void SetContentHeight(int height)
        {
            this.ScrolledY = 0;
            this.ContentHeight = height;
            this.SliderRect = new Rectangle(this.BodyRect.X + this.padding, this.BodyRect.Y + this.padding + this.ScrolledY,
                this.BodyRect.Width - (this.padding * 2), this.GetSliderHeight());
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public int GetScrolledY()
        {
            //get track height
            float TrackHeight = this.TrackRect.Height;

            //return position relative to position on track compared to scrollable height
            return -(int)(this.ContentHeight * this.ScrolledY / TrackHeight);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private Boolean CanScroll()
        {
            float TrackHeight = this.TrackRect.Height;
            if (this.ContentHeight <= TrackHeight)
                return false;

            //can scroll if mouse hovers slider
            //get current mouse state
            MouseState state = Mouse.GetState();

            //compare with slider position
            return (state.X >= this.TrackRect.X && state.X < this.TrackRect.Right &&
                state.Y >= this.TrackRect.Y + this.ScrolledY && 
                state.Y < this.TrackRect.Y + this.ScrolledY + this.SliderRect.Height);

        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void Update()
        {
            // if dragging slider
            if (this.Dragging)
            {
                // if mouse is down scroll
                if (Input.InputHandler.mouseLeftDown())
                {
                    this.ScrolledY += Input.InputHandler.getMouseScrolledY();
                    if (this.ScrolledY < 0) this.ScrolledY = 0;
                    if (this.ScrolledY > this.TrackRect.Height - this.GetSliderHeight())
                        this.ScrolledY = this.TrackRect.Height - this.GetSliderHeight();
                    this.SliderRect.Y = this.TrackRect.Y + this.ScrolledY;
                }
                // else set dragging to not true 
                else
                {
                    this.Dragging = false;
                }
            }
            // else see if should drag
            else
            {
                if (this.CanScroll() && Input.InputHandler.mouseLeftTrigger())
                {
                    this.Dragging = true;
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void Draw(Graphics.ExtendedSpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // draw background and border of main body
            spriteBatch.FillRectangle(BodyRect, Color.Gray);
            spriteBatch.DrawRectangle(BodyRect, this.padding, Color.Black);

            //draw slider background and border
            spriteBatch.FillRectangle(this.SliderRect, Color.LightBlue);
            Color color = this.Dragging ? Color.Red : this.CanScroll() ? Color.RoyalBlue : Color.DarkGray;
            spriteBatch.DrawRectangle(this.SliderRect, this.padding, color);

            spriteBatch.End();
        }
    }
}
