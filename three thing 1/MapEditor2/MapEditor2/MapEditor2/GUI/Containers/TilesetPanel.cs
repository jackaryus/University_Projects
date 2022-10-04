using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MapEditor2.GUI.Containers
{
    //======================================================================================================================
    //
    //======================================================================================================================
    public class TilesetPanel : Panel
    {
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private int SelectedTile, tilesetId;
        private Boolean Selected;
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public TilesetPanel(int x, int y, int width, int height, Editor editor)
            : base(x, y, width, height, editor)
        {
            this.SelectedTile = 0;
            this.tilesetId = -1;
            this.Selected = false;
            this.CreateVerticalBar();
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void SetTileset()
        {
            this.tilesetId = 0;
            this.VerticalBar.SetContentHeight(this.editor.Tileset.Height);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public int GetTile()
        {
            return this.SelectedTile;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public override Boolean CanSelect()
        {
            if (this.tilesetId < 0)
                return false;
            return base.CanSelect();
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();
            if (this.Selected)
            {
                if (Input.InputHandler.mouseLeftUp())
                {
                    if (this.CanSelect() && this.GetMouseRelativePosition().Y < this.editor.Tileset.Height)
                    {
                        int x = (int)this.GetMouseRelativePosition().X / 30;
                        int y = (int)this.GetMouseRelativePosition().Y / 30;
                        this.SelectedTile = (x + y * 8);

                    }
                    this.Selected = false;
                }
                    
            }
            else
            {
                if (this.CanSelect() && Input.InputHandler.mouseLeftTrigger())
                    this.Selected = true;
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public override void DrawContent(Graphics.ExtendedSpriteBatch spriteBatch)
        {
            base.DrawContent(spriteBatch);
            if (this.tilesetId >= 0)
            {
                spriteBatch.FillRectangle(new Rectangle((int)this.ContentPosition.X + this.padding, 
                    (int)this.ContentPosition.Y + this.padding, this.BodyRect.Width - (this.padding * 2), 
                    this.editor.Tileset.Height), Color.White);

                spriteBatch.Draw(this.editor.Tileset, new Vector2(this.ContentPosition.X + this.padding, 
                    this.ContentPosition.Y + this.padding), Color.White);

                //draw selected tile
                spriteBatch.DrawRectangle(new Rectangle(this.padding + (this.SelectedTile % 8 * 30),
                    (this.padding + (int)this.ContentPosition.Y) + (this.SelectedTile / 8 * 30), 30, 30), 2, Color.Black);
                spriteBatch.DrawRectangle(new Rectangle(this.padding + 2 + (this.SelectedTile % 8 * 30),
                    (this.padding + 2 + (int)this.ContentPosition.Y) + (this.SelectedTile / 8 * 30), 26, 26), 2, Color.White);
                spriteBatch.DrawRectangle(new Rectangle(this.padding + 4 + (this.SelectedTile % 8 * 30),
                    (this.padding + 4 + (int)this.ContentPosition.Y) + (this.SelectedTile / 8 * 30), 22, 22), 2, Color.Black);
            }
        }
    }
}
