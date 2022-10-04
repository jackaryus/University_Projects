using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MapEditor2.GUI.Containers
{
    //======================================================================================================================
    //
    //======================================================================================================================
    public class MapPanel : Panel
    {
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private int CurrentLayer, MapWidth, MapHeight;
        private int[][,] Layers;
        private int[,] flags;
        private Boolean Selected;
        private Color GridColor;
        private Tools.MapCreator mapCreator;
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public MapPanel(int x, int y, int width, int height, Editor editor)
            : base(x, y, width, height, editor)
        {
            this.CreateHorizontalBar();
            this.CreateVerticalBar();
            this.CurrentLayer = 0;
            this.Selected = false;
            this.GridColor = new Color(0, 0, 0, 80);
            this.mapCreator = new Tools.MapCreator();
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void SetupMap(int width, int height)
        {

            this.MapWidth = width;
            this.MapHeight = height;

            Layers = new int[3][,];

            for (int i = 0; i < 3; i++)
            {
                Layers[i] = new int[width, height];
            }
            flags = new int[width, height];

            this.HorizontalBar.SetContentWidth(width * 30);
            this.VerticalBar.SetContentHeight(height * 30);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void SetupMap(int[][,] layers, int[,] flags, string tileset)
        {

            this.MapWidth = layers[0].GetLength(0);
            this.MapHeight = layers[0].GetLength(1);
            this.Layers = layers;
            this.flags = flags;

            this.HorizontalBar.SetContentWidth(this.MapWidth * 30);
            this.VerticalBar.SetContentHeight(this.MapHeight * 30);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void SaveMap(String filename)
        {
            
            System.IO.FileStream stream = File.Open(filename, FileMode.OpenOrCreate);
            //XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            BinaryFormatter serializer = new BinaryFormatter();
            ///SaveData data = new SaveData();

            //data.layers = this.Layers;

            serializer.Serialize(stream, this.Layers);
            serializer.Serialize(stream, this.flags);
            stream.Close();
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public int GetLayer()
        {
            return this.CurrentLayer;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public int GetmapWidth()
        {
            return this.MapWidth;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public int GetmapHeight()
        {
            return this.MapHeight;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void DecreaseLayer()
        {
            if (this.CurrentLayer > 0)
            {
                this.CurrentLayer--;
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void IncreaseLayer()
        {
            if (this.CurrentLayer < 3)
                this.CurrentLayer++;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void SetTile(int x, int y, int layer, int id)
        {
            this.Layers[layer][x, y] = id;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public override Boolean CanSelect()
        {
            if (this.Layers == null)
                return false;
            return base.CanSelect();
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void Fill(Point origin)
        {
            List<Point> stack = new List<Point>();
            List<Point> FillPoints = new List<Point>();
            stack.Add(origin);
            FillPoints.Add(origin);
            Point CurrentPoint = origin;
            int OriginId = this.Layers[this.CurrentLayer][origin.X, origin.Y];
            this.Layers[this.CurrentLayer][origin.X, origin.Y] = this.editor.TilesetPanel.GetTile();
            Random random = new Random();

            while (stack.Count > 0)
            {
                List<Point> neighbours = new List<Point>();

                for (int i = 0; i < 4; i++)
                {
                    int x = i == 0 ? -1 : i == 2 ? 1 : 0;
                    int y = i == 1 ? -1 : i == 3 ? 1 : 0;

                    Point testPoint = new Point(CurrentPoint.X - x, CurrentPoint.Y - y);

                    if (testPoint.X >= 0 && testPoint.X < this.MapWidth && 
                        testPoint.Y >= 0 && testPoint.Y < this.MapHeight)
                    {

                        if (this.Layers[this.CurrentLayer][testPoint.X, testPoint.Y] == OriginId)
                        {
                            Boolean notChecked = true;
                            for (int j = 0; j < FillPoints.Count - 1; j++)
                            {
                                if (FillPoints[j].X == testPoint.X && FillPoints[j].Y == testPoint.Y)
                                    notChecked = false;
                            }
                            if (notChecked)
                                neighbours.Add(testPoint);
                        }
                    }
                }

                if (neighbours.Count > 0)
                {
                    int randomPoint = random.Next(0, neighbours.Count - 1);
                    CurrentPoint = neighbours[randomPoint];
                    this.Layers[CurrentLayer][CurrentPoint.X, CurrentPoint.Y] =
                        this.editor.TilesetPanel.GetTile();
                    stack.Add(CurrentPoint);
                    FillPoints.Add(CurrentPoint);
                }
                else
                {
                    stack.RemoveAt(stack.Count - 1);
                    if (stack.Count > 0)
                        CurrentPoint = stack[stack.Count - 1];
                }
            }

        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private void UpdateDraw()
        {
            if (this.Selected)
            {
                if (Input.InputHandler.mouseLeftDown())
                {
                    if (CanSelect())
                    {
                        int x = (int)this.GetMouseRelativePosition().X / 30;
                        int y = (int)this.GetMouseRelativePosition().Y / 30;
                        if (x < this.MapWidth && y < this.MapHeight)
                            this.SetTile(x, y, this.CurrentLayer, this.editor.TilesetPanel.GetTile());
                    }
                }
                else
                {
                    this.Selected = false;
                }
            }
            else
            {
                if (this.CanSelect() && Input.InputHandler.mouseLeftTrigger())
                {
                    this.Selected = true;
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private void UpdateFill()
        {
            if (Selected)
            {
                if (Input.InputHandler.mouseLeftUp())
                {
                    if (this.CanSelect())
                    {
                        int x = (int)this.GetMouseRelativePosition().X / 30;
                        int y = (int)this.GetMouseRelativePosition().Y / 30;
                        if (x < this.MapWidth && y < this.MapHeight)
                            this.Fill(new Point(x, y));
                    }
                    this.Selected = false;
                }
            }
            else
            {
                if (this.CanSelect() && Input.InputHandler.mouseLeftTrigger())
                {
                    this.Selected = true;
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void UpdateFlagLayer()
        {
            if (Input.InputHandler.mouseLeftTrigger() && this.CanSelect())
            {
                int x = (int)this.GetMouseRelativePosition().X / 30;
                int y = (int)this.GetMouseRelativePosition().Y / 30;
                if (x < this.MapWidth && y < this.MapHeight) 
                {
                    if (this.flags[x, y] < 100)
                        this.flags[x, y] += 1;
                    else
                        this.flags[x, y] = 0;
                }

            }
            else if (Input.InputHandler.mouseRightTrigger() && this.CanSelect())
            {
                int x = (int)this.GetMouseRelativePosition().X / 30;
                int y = (int)this.GetMouseRelativePosition().Y / 30;
                if (x < this.MapWidth && y < this.MapHeight)
                {
                    if (flags[x, y] > 0)
                        this.flags[x, y] -= 1;
                    else
                        this.flags[x, y] = 100;
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();

            if (this.CurrentLayer == 3)
            {
                UpdateFlagLayer();
                return;
            }
            switch (this.editor.ControlPanel.GetTool())
            {
                 case 0:
                    UpdateDraw();
                    break;

                 case 1:
                    UpdateFill();
                    break;
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public override void DrawContent(Graphics.ExtendedSpriteBatch spriteBatch)
        {
            base.DrawContent(spriteBatch);

            if (this.Layers != null)
            {
                spriteBatch.FillRectangle(new Rectangle(this.padding + (int)this.ContentPosition.X,
                this.padding + (int)this.ContentPosition.Y, this.MapWidth * 30, this.MapHeight * 30), Color.White);

                this.DrawMap(new Vector2(this.padding + this.ContentPosition.X, this.padding + this.ContentPosition.Y));

                for (int x = 0; x <= this.MapWidth; x++)
                {
                    spriteBatch.FillRectangle(new Rectangle(((this.padding - 1) + x * 30) + (int)this.ContentPosition.X,
                        this.padding + (int)this.ContentPosition.Y, 2, this.MapHeight * 30), this.GridColor);
                }

                for (int y = 0; y <= this.MapHeight; y++)
                {
                    spriteBatch.FillRectangle(new Rectangle(this.padding + (int)this.ContentPosition.X,
                        ((this.padding - 1) + y * 30) + (int)this.ContentPosition.Y, this.MapWidth * 30, 2), this.GridColor);
                }
            }
            
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private void DrawMap(Vector2 position)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int x = 0; x < this.MapWidth; x++)
                {
                    int TX = (int)position.X + (x * 30);
                    if (TX <= -30)
                        continue;
                    if (TX >= this.BodyRect.Width - this.padding)
                        break;
                    for (int y = 0; y < this.MapHeight; y++)
                    {
                        int TY = (int)position.Y + (y * 30);
                        if (TY <= -30 + this.padding)
                            continue;
                        if (TY >= this.BodyRect.Height - this.padding)
                            break;
                        int id;
                        if (i == 3)
                        {
                            if (this.CurrentLayer == 3)
                            {
                                id = this.flags[x, y];
                                if (id != 0)
                                {
                                    int textX = TX + (int)((30 - this.editor.spriteBatch.MeasureString(id.ToString()).X) / 2);
                                    this.editor.spriteBatch.DrawString(id.ToString(), new Vector2(textX, TY + 8), Color.Black);
                                }
                                continue;
                            }
                            else
                                break;
                        }
                        id = this.Layers[i][x, y];
                        if (id == 0) continue;
                        Color color = i > this.CurrentLayer ? new Color(255, 255, 255, 160) : Color.White;
                        this.editor.spriteBatch.Draw(this.editor.Tileset, new Rectangle(TX, 
                            TY, 30, 30), new Rectangle(id % 8 * 30, id / 8 * 30, 30, 30), 
                            color);
                    }
                }
            }

        }
    }
}
