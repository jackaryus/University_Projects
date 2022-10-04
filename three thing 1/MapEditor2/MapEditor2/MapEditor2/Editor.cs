using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MapEditor2
{
    //======================================================================================================================
    //
    //======================================================================================================================
    public class Editor : Microsoft.Xna.Framework.Game
    {
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public GraphicsDeviceManager graphics;
        public Graphics.ExtendedSpriteBatch spriteBatch;
        public GUI.Containers.TilesetPanel TilesetPanel;
        public GUI.Containers.MapPanel MapPanel;
        public GUI.Containers.ControlPanel ControlPanel;
        public Boolean CanSave;
        public Texture2D Tileset;
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public Editor()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.CanSave = false;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.TilesetPanel = new GUI.Containers.TilesetPanel(0, 0, 244, 480, this);
            this.TilesetPanel.SetBackColor(Color.Gray);
            this.MapPanel = new GUI.Containers.MapPanel(264, 0, 516, 360, this);
            this.MapPanel.SetBackColor(Color.Gray);
            this.ControlPanel = new GUI.Containers.ControlPanel(264, 380, 536, 100, this);

            base.Initialize();
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void SetupMap(int width, int height)
        {
            this.TilesetPanel.SetTileset();
            this.MapPanel.SetupMap(width, height);
            this.CanSave = true;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        public void SetupMap(String filename)
        {
            this.TilesetPanel.SetTileset();
            System.IO.FileStream stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Read);
            BinaryFormatter serializer = new BinaryFormatter();

            this.MapPanel.SetupMap((int[][,])serializer.Deserialize(stream), (int[,])serializer.Deserialize(stream), "tileset");
            stream.Close();
            this.CanSave = true;
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        protected override void LoadContent()
        {
            spriteBatch = new Graphics.ExtendedSpriteBatch(GraphicsDevice);
            spriteBatch.LoadContent(this.Content);

            // TODO: use this.Content to load your game content here
            this.Tileset = this.Content.Load<Texture2D>("tileset");

        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            

        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            Input.InputHandler.update();
            this.TilesetPanel.Update();
            this.MapPanel.Update();
            this.ControlPanel.Update();

            base.Update(gameTime);
        }
        //------------------------------------------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------------------------------------------
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            this.TilesetPanel.Draw();
            this.MapPanel.Draw();
            this.ControlPanel.Draw();


            base.Draw(gameTime);
        }
    }
}
