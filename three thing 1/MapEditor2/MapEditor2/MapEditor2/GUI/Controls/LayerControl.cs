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
    public class LayerControl
    {
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private Rectangle ContentRect, BorderRect;
        private int Padding;
        private Editor editor;
        private Button MinusButton, PlusButton;
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public LayerControl(int x, int y, Containers.Panel parent, Editor editor)
        {
            this.ContentRect = new Rectangle(x + 2, y + 2, 20, 20);
            this.BorderRect = new Rectangle(x, y, 34, 24);
            this.Padding = 2;
            this.editor = editor;
            this.MinusButton = new Button(x + 2 + 20, y + 2+ 10, 10, 10, "▼", parent, editor);
            this.PlusButton = new Button(x + 2 + 20, y + 2, 10, 10, "▲", parent, editor);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void Update()
        {
            this.MinusButton.Update();
            this.PlusButton.Update();
            if (MinusButton.IsTriggered())
            {
                this.editor.MapPanel.DecreaseLayer();
            }
            if (PlusButton.IsTriggered())
            {
                this.editor.MapPanel.IncreaseLayer();
            }

        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void Draw()
        {
            this.MinusButton.Draw();
            this.PlusButton.Draw();
            this.editor.spriteBatch.FillRectangle(this.ContentRect, Color.LightGray);

            String layer = (this.editor.MapPanel.GetLayer() + 1).ToString();
            Vector2 size = this.editor.spriteBatch.MeasureString(layer);
            int X = (int)(this.ContentRect.X + ((this.ContentRect.Width / 2) - (size.X / 2)));
            int Y = (int)(this.ContentRect.Y + ((this.ContentRect.Height / 2) - (size.Y / 2)));
            this.editor.spriteBatch.DrawString(layer, new Vector2(X, Y), Color.Black);

            this.editor.spriteBatch.DrawRectangle(this.BorderRect, this.Padding, Color.RoyalBlue);
        }
    }
}
