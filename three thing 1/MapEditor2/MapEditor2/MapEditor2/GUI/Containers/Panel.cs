using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MapEditor2.GUI.Containers
{
    //======================================================================================================================
    //
    //======================================================================================================================
    public class Panel
    {
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        protected Rectangle BodyRect;
        protected int padding;
        protected Editor editor;
        protected Vector2 ContentPosition;
        protected GUI.Controls.HorizontalScrollbar HorizontalBar;
        protected GUI.Controls.VerticalScrollbar VerticalBar;
        private Color BackColor;
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public Panel(int x, int y, int width, int height, Editor editor)
        {
            this.BodyRect = new Rectangle(x, y, width, height);
            this.padding = 2;
            this.editor = editor;
            this.ContentPosition = new Vector2(0, 0);
            this.HorizontalBar = null;
            this.VerticalBar = null;
            this.BackColor = Color.White;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void CreateHorizontalBar()
        {
            this.HorizontalBar = new Controls.HorizontalScrollbar(this.BodyRect.X, this.BodyRect.Bottom, this.BodyRect.Width,
                this.padding);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void CreateVerticalBar()
        {
            this.VerticalBar = new Controls.VerticalScrollbar(this.BodyRect.Right, this.BodyRect.Y, this.BodyRect.Height, 
                this.padding);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public virtual Boolean CanSelect()
        {
            MouseState state = Mouse.GetState();
            return (state.X >= this.BodyRect.X + this.padding &&
                state.X < this.BodyRect.Right - this.padding &&
                state.Y >= this.BodyRect.Y + this.padding &&
                state.Y < this.BodyRect.Bottom - this.padding);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public Vector2 GetMouseRelativePosition()
        {
            MouseState state = Mouse.GetState();
            float x = state.X - (this.BodyRect.X + this.padding + this.ContentPosition.X);
            float y = state.Y - (this.BodyRect.Y + this.padding + this.ContentPosition.Y);
            return new Vector2(x, y);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public Vector2 GetPosition()
        {
            return new Vector2(this.BodyRect.X, this.BodyRect.Y);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void SetBackColor(Color color)
        {
            this.BackColor = color;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public virtual void Update()
        {
            if (this.HorizontalBar != null)
            {
                this.HorizontalBar.Update();
                this.ContentPosition.X = this.HorizontalBar.GetScrolledX();
            }

            if (this.VerticalBar != null)
            {
                this.VerticalBar.Update();
                this.ContentPosition.Y = this.VerticalBar.GetScrolledY();
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void Draw()
        {
            Viewport PreviousView = this.editor.GraphicsDevice.Viewport;
            this.editor.GraphicsDevice.Viewport = new Viewport(this.BodyRect);

            this.editor.spriteBatch.Begin();

            this.editor.spriteBatch.FillRectangle(new Rectangle(0, 0, this.BodyRect.Width, this.BodyRect.Height), 
                this.BackColor);

            this.DrawContent(this.editor.spriteBatch);

            this.editor.spriteBatch.DrawRectangle(new Rectangle(0, 0, this.BodyRect.Width, this.BodyRect.Height),
                this.padding, Color.DarkBlue);

            this.editor.spriteBatch.End();

            this.editor.GraphicsDevice.Viewport = PreviousView;

            if (this.HorizontalBar != null)
                this.HorizontalBar.Draw(this.editor.spriteBatch);

            if (this.VerticalBar != null)
                this.VerticalBar.Draw(this.editor.spriteBatch);

        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public virtual void DrawContent(Graphics.ExtendedSpriteBatch spriteBatch)
        {
        }
    }
}
