using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using Microsoft.Xna.Framework.Input;

namespace OpenGL_Game.Systems
{
    class SystemPlayer : ISystem
    {
         const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_PLAYER);
         public string Name
         {
             get { return "SystemPlayer"; }
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

                 IComponent playerComponent = components.Find(delegate(IComponent component)
                 {
                     return component.ComponentType == ComponentTypes.COMPONENT_PLAYER;
                 });

                 ComponentTransform position = (ComponentTransform)positionComponent;

                 Vector3 previousPosition = position.Position;
                 position.Position = MyGame.cameraPosition;
                 position.Rotation = MyGame.cameraRotation;

                 ComponentPlayer player = (ComponentPlayer)playerComponent;

                 if (Keyboard.GetState().IsKeyDown(Keys.C) && player.previousKeyState.IsKeyUp(Keys.C))
                     player.collisionsEnabled = !player.collisionsEnabled;

                 collisions(previousPosition, position, player);
                 player.DrawKeys();
                 player.PortalSoundUpdate();

                 player.previousKeyState = Keyboard.GetState();
             }
         }

         private void collisions(Vector3 previousPosition, ComponentTransform transform, ComponentPlayer player)
         {
             Vector3 playerBotLeftFront = new Vector3(transform.Position.X - 0.5f, 0, transform.Position.Z - 0.5f);
             Vector3 playerTopRightBack = new Vector3(transform.Position.X + 0.5f, 3, transform.Position.Z + 0.5f);
             BoundingBox playerBox = new BoundingBox(playerBotLeftFront, playerTopRightBack);
             foreach (Entity entity in player.entities)
             {
                 List<IComponent> components = entity.Components;

                 IComponent positionComponent = components.Find(delegate(IComponent component)
                 {
                     return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                 });
                 ComponentTransform componentGeo = (ComponentTransform)positionComponent;

                 if (entity.Name.Contains("Portal"))
                 {
                     if (componentGeo.Box.Intersects(playerBox))
                         player.EnterPortal();
                 }
                 else if (componentGeo.Box.Intersects(playerBox) && player.collisionsEnabled)
                 {
                     transform.Position = previousPosition;
                     MyGame.cameraPosition = previousPosition;
                     continue;
                 }

                 // Bullet detection
                 if (entity.Name == "AI")
                 {
                     IComponent aiComponent = components.Find(delegate(IComponent component)
                     {
                         return component.ComponentType == ComponentTypes.COMPONENT_AI;
                     });
                     ComponentAI ai = (ComponentAI)aiComponent;

                     // For all the bullets in the AI.
                     foreach (Entity bullet in ai.bullets.Entities())
                     {
                         List<IComponent> bulletComponents = bullet.Components;
                         IComponent bulletPosition = bulletComponents.Find(delegate(IComponent component)
                         {
                             return component.ComponentType == ComponentTypes.COMPONENT_POSITION
;
                         });
                         ComponentTransform bulletPos = (ComponentTransform)bulletPosition;

                         if (bulletPos.Box.Intersects(playerBox))
                         {
                             player.TakeHit();
                             ai.bullets.Entities().Remove(bullet);
                             break;
                         }
                     }
                 }
             }

             foreach (Entity key in player.keys.Entities())
             {
                 List<IComponent> components = key.Components;

                 IComponent positionComponent = components.Find(delegate(IComponent component)
                 {
                     return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                 });
                 ComponentTransform componentGeo = (ComponentTransform)positionComponent;

                 if (componentGeo.Box.Intersects(playerBox))
                 {
                     player.GetKey(key);
                     break;
                 }
             }
         }
    }
}
