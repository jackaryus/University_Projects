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
        public Vector3 Normal;
        public Vector3 boxDimension1;
        public Vector3 boxDimension2;
        public BoundingBox Box;


        public ComponentTransform(Vector3 pos,Vector3 rot, Vector3 sca, Vector3 norm)//, int Box)
        {
            Position = pos;
            Rotation = rot;
            Scale = sca;
            Normal = norm;
            Box = new BoundingBox(new Vector3(100,100,100), new Vector3(100,100,100));
        }

        public void setBoundingBoxWall()
        {
            // X walls.
            Vector3 bottomLeftBack = new Vector3(Position.X - (2.5f * Scale.X), 0, Position.Z - 0.25f);
            Vector3 topRightFront = new Vector3(Position.X + (2.5f * Scale.X), 5, Position.Z + 0.25f);
            if(Rotation.X != 0)
            {
                bottomLeftBack = new Vector3(Position.X - 0.25f, 0, Position.Z - (2.5f * Scale.X));
                topRightFront = new Vector3(Position.X + 0.25f, 5, Position.Z + (2.5f * Scale.X));
            }
            Box = new BoundingBox(bottomLeftBack,topRightFront);
        }

        public void setBoundingBoxCube(Vector3 min, Vector3 max)
        {
            boxDimension1 = min;
            boxDimension2 = max;
            Vector3 bottomLeftBack = Position + min;
            Vector3 topRightFront = Position + max;
            Box = new BoundingBox(bottomLeftBack, topRightFront);
        }

        public void updateBox()
        {
            Vector3 bottomLeftBack = Position + boxDimension1;
            Vector3 topRightFront = Position + boxDimension2;
            Box = new BoundingBox(bottomLeftBack, topRightFront);
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_POSITION; }
        }
    }
}
