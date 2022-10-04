using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Components
{
    class ComponentSkyBox : IComponent
    {
        TextureCube skybox;

        public ComponentSkyBox(string[] skyboxFaces)
        {
            Texture2D[] CubeFaces = new Texture2D[6];

            for (int i = 0; i < 6; i++)
            {
                CubeFaces[i] = ResourceManager.LoadTexture(skyboxFaces[i]);
            }
            CreateSkybox(CubeFaces);
        }
        public void CreateSkybox(Texture2D[] Faces)
        {
            skybox = new TextureCube(MyGame.gameInstance.GraphicsDevice, Faces[0].Width, false, SurfaceFormat.Color);
            Color[] faceColor;
            for (int i = 0; i < 6; i++)
            {
                faceColor = new Color[Faces[i].Width * Faces[i].Height];
                Faces[i].GetData<Color>(faceColor);
                skybox.SetData<Color>((CubeMapFace)i, faceColor);
            }
        }
                    
        public TextureCube Skybox
        {
            get { return skybox; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SKYBOX; }
        }
    }
}
