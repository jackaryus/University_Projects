using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OpenGL_Game.Components
{
    class ComponentUIvelocity : IComponent
    {
        Vector2 uiVelocity;

        public ComponentUIvelocity(float x, float y)
        {
            uiVelocity = new Vector2(x, y);
        }

        public ComponentUIvelocity(Vector2 v)
        {
            uiVelocity = v;
        }

        public Vector2 UIvelocity
        {
            get { return uiVelocity; }
            set { uiVelocity = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_UIVELOCITY; }
        }
    }
}
