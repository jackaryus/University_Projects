using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    class SystemPhysics : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_TEXTURE | ComponentTypes.COMPONENT_VELOCITY);

        public string Name
        {
            get { return "SystemPhysics"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                //Vector3 position = ((ComponentPosition)positionComponent).Position;
                //Matrix world = Matrix.CreateTranslation(position);
                
                IComponent velocityComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                });
                Vector3 velocity = ((ComponentVelocity)velocityComponent).Velocity;

                Motion((ComponentTransform)positionComponent, velocity);
            }
        }

        public void Motion(ComponentTransform position, Vector3 velocity)
        {
            position.Position = position.Position + (velocity * MyGame.dt);
        }

    }
}
