// File Author: Daniel Masterson
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
using SpaceGame.GameManagers;

namespace SpaceGame
{
    /// <summary>
    /// Main game type
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        public const string GameTitle = "Solar Dominion";

        public static Main self;
        public static GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public static GraphicsDevice GraphicsDev { get; private set; }
        public static SpriteBatch SpriteBatch { get; private set; }
        public static Point Resolution { get; set; }
        public static ContentManager ContentManager { get; private set; }

        public Main()
        {
            self = this;
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            GraphicsDeviceManager.PreferMultiSampling = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            Window.Title  = "Solar Dominion";
        }

        protected override void LoadContent()
        {
            ContentManager = Content;
            GraphicsDev = GraphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Resolution = new Point(GraphicsDev.PresentationParameters.BackBufferWidth, GraphicsDev.PresentationParameters.BackBufferHeight);
            
            Managers.Initialize();
            SceneManager.LoadScene(new MainMenuScene());
            GraphicsManager.SetResolution(SettingsManager.GetSetting<int>("game.resolutionX"), SettingsManager.GetSetting<int>("game.resolutionY"));
        }

        protected override void UnloadContent()
        {
            Managers.ShutdownManagers();
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                if (Resolution.X != GraphicsDev.PresentationParameters.BackBufferWidth || Resolution.Y != GraphicsDev.PresentationParameters.BackBufferHeight)
                {
                    Resolution = new Point(GraphicsDev.PresentationParameters.BackBufferWidth, GraphicsDev.PresentationParameters.BackBufferHeight);
                    CameraManager.OnUpdateResolution();
                }

                Managers.UpdateManagers((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            Managers.RenderManagers((float)gameTime.ElapsedGameTime.TotalSeconds);
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
