using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Components
{
    class ComponentShader : IComponent
    {
        Effect effect;

        public ComponentShader(string effectName)
        {
            effect = ResourceManager.LoadEffect(effectName);
        }

        public Effect Effect
        {
            get { return effect; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SHADER; }
        }
    }
}
