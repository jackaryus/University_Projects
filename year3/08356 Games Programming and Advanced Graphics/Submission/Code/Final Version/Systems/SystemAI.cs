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
    class SystemAI : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_TEXTURE | ComponentTypes.COMPONENT_AI);

        public string Name
        {
            get { return "SystemAI"; }
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
                Vector3 position = ((ComponentTransform)positionComponent).Position;

                IComponent aiComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AI;
                });
                ComponentAI ai = ((ComponentAI)aiComponent);

                if (ai.isEnabled())
                    UpdateAI(ai, (ComponentTransform)positionComponent);

                ai.updateBulletSound();
            }
        }

        private void UpdateAI(ComponentAI ai, ComponentTransform aiPosition)
        {
            Vector3 finalTarget = ai.target;
            if (ai.targetState == ComponentAI.targetChoice.player)
            {
                finalTarget = MyGame.cameraPosition;
                ai.chasingPlayerCount++;
                ai.reloadCount++;
                if (ai.chasingPlayerCount > ai.chasingPlayerMax)
                {
                    ai.setNewTarget();
                    finalTarget = ai.target;
                    ai.chasingPlayerCount = 0;
                }
                if (Vector3.Distance(aiPosition.Position, MyGame.cameraPosition) < ai.shootingRange && ai.reloadCount > ai.reloadTime)
                {
                    ai.createBullet(aiPosition.Normal, aiPosition.Position);
                    ai.reloadCount = 0;
                }
            }

            if (inRangeOfTarget(ai.targetNode.position, aiPosition.Position))
                setTarget(aiPosition.Position, finalTarget, ai);
            else
                pathfind(finalTarget, aiPosition.Position, ai);

            Vector3 targetVec = ai.targetNode.position;
            // Player detection.
            bool detected = false;
            Vector3 toPlayerVec = MyGame.cameraPosition - aiPosition.Position;
            float dot = Vector3.Dot(Vector3.Normalize(toPlayerVec), aiPosition.Normal);
            if (playerDetected(dot,aiPosition.Position,MyGame.cameraPosition, ai))
            {
                aiPosition.Rotation.X = (float)Math.Atan2((double)toPlayerVec.X, (double)toPlayerVec.Z) + (float)Math.PI / 2;
                aiPosition.Normal = Vector3.Normalize(toPlayerVec);
                detected = true;
                ai.targetState = ComponentAI.targetChoice.player;
                ai.chasingPlayerCount = 0;
            }
            else
            {
                Vector3 toTargetVec = Vector3.Normalize(targetVec - aiPosition.Position);
                aiPosition.Normal = Vector3.Normalize(toTargetVec);
                // Set rotation.
                aiPosition.Rotation.X = (float)Math.Atan2((double)toTargetVec.X, (double)toTargetVec.Z) + (float)Math.PI / 2;
            }

            if (!detected)
            {
                // Movement
                if (aiPosition.Position.X < targetVec.X + 0.1f && aiPosition.Position.X > targetVec.X - 0.1f) // To stop jittering.
                    aiPosition.Position.X = targetVec.X;
                else if (aiPosition.Position.X < targetVec.X)
                    aiPosition.Position.X += ai.velocity * MyGame.dt;
                else if (aiPosition.Position.X > targetVec.X)
                    aiPosition.Position.X -= ai.velocity * MyGame.dt;
                if (aiPosition.Position.Z < targetVec.Z + 0.1f && aiPosition.Position.Z > targetVec.Z - 0.1f) // To stop jittering.
                    aiPosition.Position.Z = targetVec.Z;
                else if (aiPosition.Position.Z < targetVec.Z)
                    aiPosition.Position.Z += ai.velocity * MyGame.dt;
                else if (aiPosition.Position.Z > targetVec.Z)
                    aiPosition.Position.Z -= ai.velocity * MyGame.dt;
            }
            if (inRangeOfTarget(finalTarget, aiPosition.Position))
            {
                ai.nextTarget();
            }

            ai.updateBullets();
            aiPosition.updateBox();
        }
        
        private bool inRangeOfTarget(Vector3 target, Vector3 position)
        {
            if (Vector2.Distance(new Vector2(target.X, target.Z), new Vector2(position.X, position.Z)) < 1.5f)
                return true;
            else
                return false;
        }

        private bool playerDetected(float dot, Vector3 aiPosition, Vector3 playerPosition, ComponentAI ai)
        {
            Vector3 aiToPlayerVec = playerPosition - aiPosition;

            if (dot > 0.707f && aiToPlayerVec.Length() < ai.shootingRange)
            {
                foreach (Entity wall in ai.walls)
                {
                    List<IComponent> components = wall.Components;

                    IComponent positionComponent = components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                    });
                    ComponentTransform componentGeo = (ComponentTransform)positionComponent;
                    float intialDot = Vector3.Dot(Vector3.Normalize(aiPosition - componentGeo.Position), componentGeo.Normal);
                    float secondDot = Vector3.Dot(Vector3.Normalize(playerPosition - componentGeo.Position), componentGeo.Normal);

                    // Detect collision.
                    // Calculate Radius.
                    bool inZaxis;
                    float radius = 2.5f * componentGeo.Scale.X;
                    if (componentGeo.Normal.X != 0.0f)
                    {
                        inZaxis = false;
                    }
                    else
                    {
                        inZaxis = true;
                    }
                    if (intialDot > 0 && secondDot < 0)
                    {
                        // Scale the vector to the point at which it is in line with the wall.
                        float scale;
                        if (inZaxis)
                            scale = (componentGeo.Position.Z - aiPosition.Z)/ aiToPlayerVec.Z;
                        else
                            scale = (componentGeo.Position.X - aiPosition.X)/ aiToPlayerVec.X;
                        // Ensure scale is never negative.
                        if (scale < 0)
                            scale *= -1;

                        Vector3 vecAtWall = aiPosition + (aiToPlayerVec * scale);
                        // If the vector at the point of the wall is within the radius of the wall return false.
                        if (Vector2.Distance(new Vector2(vecAtWall.X, vecAtWall.Z), new Vector2(componentGeo.Position.X, componentGeo.Position.Z)) < radius)
                        {
                            return false;
                        }

                    }
                }
                return true;
            }
            return false;
        }

        private float Fx(Nodes head, Vector3 AIpos, Vector3 playerPos)
        {
            float fX;
            float gX = Vector3.Distance(head.position, AIpos);
            float hX = Vector3.Distance(head.position, playerPos);
            fX = gX + hX;
            return fX;
        }
        void setTarget(Vector3 AIpos, Vector3 playerPos, ComponentAI ai)
        {
            Nodes bestFx = new Nodes(new Vector3(10000, 0, 10000));

            // For every node the current target node is connected to.
            for (int i = 0; i < ai.targetNode.linkedNodes.Count; i++)
            {
                if (Fx(ai.targetNode.linkedNodes[i], AIpos, playerPos) < Fx(bestFx, AIpos, playerPos))
                {
                    bestFx = ai.targetNode.linkedNodes[i];
                }
            }
            ai.previousTargetNode = ai.targetNode;
            ai.targetNode = bestFx;
        }

        float Method(Nodes head, Vector3 playerPos, Vector3 AIpos)
        {
            float gX = Vector3.Distance(AIpos, head.position);
            float hX = Vector3.Distance(playerPos, head.position);

            float bestFx = 1000000;
            Vector3 bestVec2 = new Vector3(0, 0, 0);

            bool closer = false;

            foreach (Nodes a in head.linkedNodes)
            {
                if (Vector3.Distance(a.position, playerPos) < Vector3.Distance(head.position, playerPos))
                {
                    float recursionValue = Method(a, playerPos, head.position);
                    if (recursionValue < bestFx && Vector3.Distance(a.position, playerPos) < hX)
                    {
                        bestFx = recursionValue;
                        bestVec2 = a.position;
                        closer = true;
                    }
                }
            }

            if (closer)
            {
                hX = bestFx;
            }
            else
                hX = Vector3.Distance(playerPos, head.position);
            return gX + hX;
        }
        void pathfind(Vector3 playerPos, Vector3 AIpos, ComponentAI ai)
        {
            float targetMethod = Method(ai.targetNode, playerPos, AIpos);
            float previousTargetMethod = Method(ai.previousTargetNode, playerPos, AIpos);
            if (targetMethod > previousTargetMethod)
            {
                Nodes temp = ai.previousTargetNode;
                ai.previousTargetNode = ai.targetNode;
                ai.targetNode = temp;
            }
        }
    }
}
