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
        float tick = 0.0f;

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
                
                IComponent velocityComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                });
                Vector3 velocity = ((ComponentVelocity)velocityComponent).Velocity;
                Vector3 rotationVelocity = ((ComponentVelocity)velocityComponent).RotationVelocity;
                
                Motion((ComponentTransform)positionComponent, velocity, rotationVelocity, entity.Name);
            }
        }

        public void Motion(ComponentTransform position, Vector3 velocity, Vector3 rotationVelocity, string entityName)
        {
            tick = tick + MyGame.dt;
            if (entityName == "RollingSquare")
            {
                Vector2 direction = new Vector2(20.0f + (3.0f * (float)Math.Cos(tick * 1.0f)), -20.0f + (3.0f * (float)Math.Sin(tick * 1.0f))) - new Vector2(position.Position.X, position.Position.Z); 
                position.Position.X = 20.0f + (3.0f * (float)Math.Cos(tick * 1.0f));
                position.Position.Y = 0.6f;
                position.Position.Z = -20.0f + (3.0f * (float)Math.Sin(tick * 1.0f));
                if (rotationVelocity != new Vector3(0, 0, 0))
                {
                    position.Rotation.X = -(float)Math.Atan2((double)direction.Y, (double)direction.X) + (float)Math.PI / 2;
                    position.Rotation += rotationVelocity;
                }
                //Console.WriteLine(position.Position);
                //Console.WriteLine();
            }
            else if (entityName == "BouncingOctahedron")
            {
                position.Position.Y = 2.5f + (2.0f * (float)Math.Cos(tick));
                //Console.WriteLine(position.Position);
            }
            else
            {
                position.Position = position.Position + (velocity * MyGame.dt);
                position.Rotation += rotationVelocity;
            }
            if (tick > 2 * Math.PI)
                tick = 0.0f;

            position.updateBox();
        }
    }
}
