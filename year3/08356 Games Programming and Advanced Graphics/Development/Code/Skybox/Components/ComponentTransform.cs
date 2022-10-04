using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OpenGL_Game.Components
{
    class ComponentTransform : IComponent
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;


        public ComponentTransform(Vector3 pos,Vector3 rot, Vector3 sca)
        {
            Position = pos;
            Rotation = rot;
            Scale = sca;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_POSITION; }
        }
    }
}
