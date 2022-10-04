using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Components
{
    class ComponentTexture : IComponent
    {
        Texture2D texture;

        public ComponentTexture(string textureName)
        {
            texture = ResourceManager.LoadTexture(textureName);
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_TEXTURE; }
        }
    }
}
