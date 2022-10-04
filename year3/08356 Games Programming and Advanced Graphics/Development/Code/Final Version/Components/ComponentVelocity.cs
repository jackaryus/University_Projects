using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OpenGL_Game.Components
{
    class ComponentVelocity : IComponent
    {
        Vector3 velocity;
        Vector3 rotationVelocity;

        public ComponentVelocity(float x, float y, float z)
        {
            velocity = new Vector3(x, y, z);
            rotationVelocity = new Vector3(0, 0, 0);
        }

        public ComponentVelocity(Vector3 v)
        {
            velocity = v;
            rotationVelocity = new Vector3(0, 0, 0);
        }

        public ComponentVelocity(Vector3 v, Vector3 rot)
        {
            velocity = v;
            rotationVelocity = rot;
        }

        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Vector3 RotationVelocity
        {
            get { return rotationVelocity; }
            set { rotationVelocity = value; }
        }
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_VELOCITY; }
        }
    }
}
