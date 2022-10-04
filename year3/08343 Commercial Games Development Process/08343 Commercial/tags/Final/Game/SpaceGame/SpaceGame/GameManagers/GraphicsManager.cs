// File Author: Daniel Masterson
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpaceGame.GameManagers
{
    /// <summary>
    /// Manages graphics, alongside assets like fonts and textures
    /// </summary>
    public class GraphicsManager : AbstractManager
    {
        static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public GraphicsManager()
        {
            fonts.Add("generic", Main.ContentManager.Load<SpriteFont>("Generic"));

            Texture2D nullTex = new Texture2D(Main.GraphicsDev, 16, 16);
            Color[] nullColArray = new Color[16 * 16];
            for (int i = 0; i < nullColArray.Length; i++)
            {
                if (i % 16 < 8)
                    nullColArray[i] = i < nullColArray.Length / 2 ? Color.Magenta : Color.Black;
                else
                    nullColArray[i] = i < nullColArray.Length / 2 ? Color.Black : Color.Magenta;
            }
            nullTex.SetData<Color>(nullColArray);
            textures.Add("null", nullTex);
            textures.Add("pixel", UtilityManager.Pixel);

            LoadFolder("Content\\Textures");
        }

        private void LoadFolder(string folder)
        {
            Texture2D tempTexture;

            foreach (string file in Directory.GetFiles(folder))
            {
                if (file.EndsWith("png"))
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                        tempTexture = Texture2D.FromStream(Main.GraphicsDev, fs);

                    if (tempTexture != null)
                    {
                        textures.Add(file.ToLower().Remove(file.LastIndexOf('.')).Remove(0, "Content\\Textures\\".Length), tempTexture);
                    }
                }
            }

            foreach (string newFolder in Directory.GetDirectories(folder))
                LoadFolder(newFolder);
        }

        public static void SetResolution(int newWidth, int newHeight)
        {
            if (newWidth <= 0 || newHeight <= 0)
                return;

            Main.GraphicsDeviceManager.PreferredBackBufferWidth = newWidth;
            Main.GraphicsDeviceManager.PreferredBackBufferHeight = newHeight;
            Main.Resolution = new Point(newWidth, newHeight);
            Main.GraphicsDeviceManager.ApplyChanges();

            EntityManager.NotifyResolutionChanged();
            PlayerManager.NotifyResolutionChanged();
        }

        public static void SetFullScreen(bool isFullscreen)
        {
            Main.GraphicsDeviceManager.IsFullScreen = isFullscreen;
            Main.GraphicsDeviceManager.ApplyChanges();
        }

        public static Texture2D GetTexture(string texture)
        {
            texture = texture.ToLower();

            if (textures.ContainsKey(texture))
                return textures[texture];
            else
                return textures["null"];
        }

        public static SpriteFont GetFont(string font)
        {
            if (fonts.ContainsKey(font))
                return fonts[font];
            else if (fonts.ContainsKey("generic"))
                return fonts["generic"];

            throw new ArgumentException("No fonts loaded");
        }
    }
}
