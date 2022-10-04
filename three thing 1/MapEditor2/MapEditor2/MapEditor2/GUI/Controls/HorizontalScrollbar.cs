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
    public class HorizontalScrollbar
    {
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private Rectangle BodyRect, TrackRect, SliderRect;
        private int padding, ContentWidth, ScrolledX;
        private Boolean Dragging;
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public HorizontalScrollbar(int x, int y, int width, int padding)
        {
            this.padding = padding;
            this.BodyRect = new Rectangle(x, y, width, 20);
            this.TrackRect = new Rectangle(x + padding, y + padding, width - (padding * 2), 
                20 - (padding * 2));
            this.ContentWidth = width - (padding * 2);
            this.ScrolledX = 0;
            this.SliderRect = new Rectangle(x + padding + this.ScrolledX, y + padding, GetSliderWidth(), 
                20 - (padding * 2));
            this.Dragging = false;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private int GetSliderWidth()
        {
            float TrackWidth = this.TrackRect.Width;

            if (this.ContentWidth <= TrackWidth)
                return (int)TrackWidth;

            float percent = TrackWidth / this.ContentWidth;
            float size = percent * TrackWidth;

            //trying to cap min size
            //if (size >= 50)
                return (int)size;
            //return 50;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void SetContentWidth(int width)
        {
            this.ScrolledX = 0;
            this.ContentWidth = width;
            this.SliderRect = new Rectangle(this.BodyRect.X + this.padding + this.ScrolledX, this.BodyRect.Y + this.padding, 
                this.GetSliderWidth(), this.BodyRect.Height - (this.padding * 2));
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public int GetScrolledX()
        {
            //this method is not 100% accurate, when the content size is large it returns a little more than it should???

            //get track Width
            float TrackWidth = this.TrackRect.Width;

            //return position relative to position on track compared to scrollable Width
            return -(int)(this.ContentWidth * this.ScrolledX / TrackWidth);
            
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private Boolean CanScroll()
        {
            float TrackWidth = this.TrackRect.Width;
            if (this.ContentWidth <= TrackWidth)
                return false;

            //can scroll if mouse hovers slider
            //get current mouse state
            MouseState state = Mouse.GetState();

            //compare with slider position
            return (state.X >= this.TrackRect.X + this.ScrolledX &&
                state.X < this.TrackRect.X + this.ScrolledX + this.SliderRect.Width &&
                state.Y >= this.TrackRect.Y && state.Y < this.TrackRect.Bottom);

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
                    this.ScrolledX += Input.InputHandler.getMouseScrolledX();
                    if (this.ScrolledX < 0) this.ScrolledX = 0;
                    if (this.ScrolledX > this.TrackRect.Width - this.GetSliderWidth())
                        this.ScrolledX = this.TrackRect.Width - this.GetSliderWidth();
                    this.SliderRect.X = this.TrackRect.X + this.ScrolledX;
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
