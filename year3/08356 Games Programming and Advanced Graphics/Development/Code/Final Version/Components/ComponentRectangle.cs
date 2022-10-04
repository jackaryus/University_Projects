using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Components
{
    class ComponentRectangle : IComponent
    {
        Rectangle rectangle;

        public ComponentRectangle(Rectangle rec)
        {
            rectangle = rec;
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_RECTANGLE; }
        }
    }
}
