using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MapEditor2.Graphics
{
    //======================================================================================================================
    //
    //======================================================================================================================
    public class ExtendedSpriteBatch : SpriteBatch
    {
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        //blank texture used to draw shapes
        private Texture2D WhiteTexture;
        private SpriteFont DefaultFont;
        private SpriteFont CurrentFont;
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public ExtendedSpriteBatch(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.WhiteTexture = new Texture2D(this.GraphicsDevice, 1, 1);
            this.WhiteTexture.SetData(new Color[] { Color.White });
            
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void LoadContent(ContentManager content)
        {
            this.DefaultFont = content.Load<SpriteFont>("DefaultFont");
            this.CurrentFont = this.DefaultFont;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void DrawString(string text, Vector2 position, Color color)
        {
            this.DrawString(this.CurrentFont, text, position, color);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void DrawOutlinedString(string text, Vector2 position, Color BorderColor, Color FillColor)
        {
            this.DrawString(this.CurrentFont, text, new Vector2(position.X - 1, position.Y - 1), BorderColor);
            this.DrawString(this.CurrentFont, text, new Vector2(position.X + 1, position.Y - 1), BorderColor);
            this.DrawString(this.CurrentFont, text, new Vector2(position.X - 1, position.Y + 1), BorderColor);
            this.DrawString(this.CurrentFont, text, new Vector2(position.X + 1, position.Y + 1), BorderColor);
            this.DrawString(this.CurrentFont, text, position, FillColor);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public Vector2 MeasureString(string text)
        {
            return this.CurrentFont.MeasureString(text);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            float length = (end - start).Length();
            float rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            this.Draw(this.WhiteTexture, start, null, color, rotation, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void DrawRectangle(Rectangle rectangle, int padding, Color color)
        {
            this.Draw(this.WhiteTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, padding), color);
            this.Draw(this.WhiteTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - padding, 
                rectangle.Width, padding), color);
            this.Draw(this.WhiteTexture, new Rectangle(rectangle.X, rectangle.Top, padding, rectangle.Height), color);
            this.Draw(this.WhiteTexture, new Rectangle(rectangle.X + rectangle.Width - padding, rectangle.Y, padding, 
                rectangle.Height), color);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void FillRectangle(Rectangle rectangle, Color color)
        {
            this.Draw(this.WhiteTexture, rectangle, color);
        }
    }
}
