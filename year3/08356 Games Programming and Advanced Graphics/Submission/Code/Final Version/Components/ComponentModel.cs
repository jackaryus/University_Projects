using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Components
{
    class ComponentModel : IComponent
    {
        OpenGL_Game.Objects.Model modelobj;

        public ComponentModel(string ModelName, Microsoft.Xna.Framework.Graphics.Model model)
        {
            // this loads the model from file like the geometrys
            this.modelobj = ResourceManager.LoadModel(ModelName, model);
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_MODEL; }
        }

        public OpenGL_Game.Objects.Model model()
        {
            return modelobj;
        }
    }
}
