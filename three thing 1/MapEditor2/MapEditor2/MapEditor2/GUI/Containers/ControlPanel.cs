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
    public class ControlPanel : Panel
    {
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        private Controls.NewMapButton NewMapButton;
        private Controls.LoadMapButton LoadMapButton;
        private Controls.SaveMapButton SaveMapButton;
        private Controls.LayerControl LayerControl;
        private Controls.Button ClearLayerButton;
        private Controls.ToggleButton[] tools;
        private int selectedTool;
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public ControlPanel(int x, int y, int width, int height, Editor editor)
            : base(x, y, width, height, editor)
        {
            this.NewMapButton = new Controls.NewMapButton(10, 10, 60, 30, "New Map", this, this.editor);
            this.LoadMapButton = new Controls.LoadMapButton(10, 50, 60, 30, "Load Map", this, this.editor);
            this.SaveMapButton = new Controls.SaveMapButton(80, 10, 60, 30, "Save Map", this, this.editor);
            this.LayerControl = new Controls.LayerControl(80, 50, this, this.editor);


            this.ClearLayerButton = new Controls.Button(150, 10, 60, 30, "Clear Layer", this, this.editor);

            this.tools = new Controls.ToggleButton[2];
            this.tools[0] = new Controls.ToggleButton(300, 10, 30, 30, "label", this, this.editor);
            this.tools[0].IsPressed = true;
            this.tools[1] = new Controls.ToggleButton(340, 10, 30, 30, "label", this, this.editor);
            this.selectedTool = 0;
            
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public int GetTool()
        {
            return this.selectedTool;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();

            this.NewMapButton.Update();
            this.LoadMapButton.Update();
            if (this.editor.CanSave)
            {
                this.SaveMapButton.Update();
                this.LayerControl.Update();

                this.ClearLayerButton.Update();

                if (this.ClearLayerButton.IsTriggered())
                {
                    for (int x = 0; x < this.editor.MapPanel.GetmapWidth(); x++)
                    {
                        for (int y = 0; y < this.editor.MapPanel.GetmapHeight(); y++)
                        {
                            this.editor.MapPanel.SetTile(x, y, this.editor.MapPanel.GetLayer(), 0);
                        }
                    }
                }
            }
            for (int i = 0; i < this.tools.Length; i++)
            {
                this.tools[i].Update();
                if (this.tools[i].IsTriggered())
                {
                    this.selectedTool = i;
                    for (int j = 0; j < this.tools.Length; j++)
                    {
                        if (this.tools[j] != this.tools[i])
                        {
                            this.tools[j].IsPressed = this.tools[i].IsPressed ? false : true;
                        }
                    }
                }

            }

        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public override void DrawContent(Graphics.ExtendedSpriteBatch spriteBatch)
        {
            base.DrawContent(spriteBatch);

            this.NewMapButton.Draw();
            this.LoadMapButton.Draw();
            this.SaveMapButton.Draw();
            this.LayerControl.Draw();
            this.ClearLayerButton.Draw();

            for (int i = 0; i < this.tools.Length; i++)
            {
                this.tools[i].Draw();

            }
            if (this.editor.MapPanel.GetLayer() == 0)
                spriteBatch.DrawString("background layer", new Vector2(120, 55), Color.Black);
            else if (this.editor.MapPanel.GetLayer() == 1)
                spriteBatch.DrawString("collision layer", new Vector2(120, 55), Color.Black);
            else if (this.editor.MapPanel.GetLayer() == 2)
                spriteBatch.DrawString("foreground layer", new Vector2(120, 55), Color.Black);
            else if (this.editor.MapPanel.GetLayer() == 3)
                spriteBatch.DrawString("flags layer", new Vector2(120, 55), Color.Black);

            spriteBatch.DrawString("Pencil \n tool", new Vector2(300, 40), Color.Black);
            spriteBatch.DrawString("Bucket fill \n tool", new Vector2(340, 40), Color.Black);

        }
    }
}