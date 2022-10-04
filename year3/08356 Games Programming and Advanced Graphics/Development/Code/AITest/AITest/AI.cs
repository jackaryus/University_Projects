using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITest
{
    public class AI : Sprite
    {
        Node[] nodes;
        Node target;
        Node previousTarget;
        int delayCount;
        bool chasingPlayer;

        public AI(Rectangle inRect, Texture2D inTexture, Node[] inNodes, Sprite player, Sprite[] blocks)
            : base(inRect, inTexture, blocks)
        {
            nodes = inNodes;
            target = nodes[8];
            previousTarget = nodes[5];
            delayCount = 0;
            chasingPlayer = false;
        }

        public override void Update(Sprite player)
        {
            delayCount++;

            Vector2 playerPos = new Vector2(player.spriteRectangle.X, player.spriteRectangle.Y);
            Vector2 AIpos = new Vector2(spriteRectangle.X, spriteRectangle.Y);

            Vector2 targetVec = target.position;


            Vector2 targetPos = new Vector2(target.position.X, target.position.Y);

            Rectangle previousPosition = spriteRectangle;

            Rectangle intersectsRect = new Rectangle(spriteRectangle.X, spriteRectangle.Y, 1, 1);

            if (chasingPlayer)
                targetPos = playerPos;
            else if (intersectsRect.Intersects(target.intersectionRectangle)) // If it has reached the target update to a new one.
            {
                setTarget(AIpos, playerPos);
                delayCount = 0;
            }
            else
                pathfind(playerPos, AIpos);

            if (Vector2.Distance(target.position, AIpos) >= Vector2.Distance(playerPos, AIpos))
            {
                targetVec = playerPos;
                chasingPlayer = true;
            }
            else if (chasingPlayer)
            {
                resetTargets(AIpos);
                chasingPlayer = false;
            }

            // Movement.
            if (targetVec.X < spriteRectangle.X)
                spriteRectangle.X -= 2;
            else if (targetVec.X > spriteRectangle.X)
                spriteRectangle.X += 2;

            if (CollidesWithBlocks())
                spriteRectangle = previousPosition;
            else
                previousPosition = spriteRectangle;

            if (targetVec.Y < spriteRectangle.Y)
                spriteRectangle.Y -= 2;
            else if (targetVec.Y > spriteRectangle.Y)
                spriteRectangle.Y += 2;

            if (CollidesWithBlocks())
                spriteRectangle = previousPosition;
            else
                previousPosition = spriteRectangle;



        }

        private float Fx(Node head, Vector2 AIpos, Vector2 playerPos)
        {
            float fX;
            float gX = Vector2.Distance(head.position, AIpos);
            float hX = Vector2.Distance(head.position, playerPos);
            fX = gX + hX;
            return fX;
        }

        void setTarget(Vector2 AIpos, Vector2 playerPos)
        {
            Node bestFx = new Node(new Vector2(10000, 10000));

            // For every node the current target node is connected to.
            for (int i = 0; i < target.nodesConnected.Count; i++)
            {
                if (Fx(target.nodesConnected[i], AIpos, playerPos) < Fx(bestFx, AIpos, playerPos))
                {
                    bestFx = target.nodesConnected[i];
                }
            }
            previousTarget = target;
            target = bestFx;
            if (Vector2.Distance(target.position, AIpos) >= Vector2.Distance(playerPos, AIpos))
                chasingPlayer = true;
        }

        void resetTargets(Vector2 AIpos)
        {
            Node closest = nodes[0];
            Node second = closest;
            for (int i = 0; i < nodes.Length; i++)
            {
                if (Vector2.Distance(closest.position, AIpos) > Vector2.Distance(nodes[i].position, AIpos))
                {
                    second = closest;
                    closest = nodes[i];
                }
                else if(Vector2.Distance(second.position, AIpos) > Vector2.Distance(nodes[i].position, AIpos))
                {
                    second = nodes[i];
                }
            }
            target = closest;
            previousTarget = second;
        }



        float Method(Node head, Vector2 playerPos, Vector2 AIpos)
        {
            float gX = Vector2.Distance(AIpos, head.position);
            float hX = Vector2.Distance(playerPos, head.position);

            float bestFx = 1000000;
            Vector2 bestVec2 = new Vector2(0, 0);

            bool closer = false;

            foreach (Node a in head.nodesConnected)
            {
                if (Vector2.Distance(a.position, playerPos) < Vector2.Distance(head.position, playerPos))
                {
                    float recursionValue = Method(a, playerPos, head.position);
                    if (recursionValue < bestFx && Vector2.Distance(a.position, playerPos) < hX)
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
                hX = Vector2.Distance(playerPos, head.position);
            return gX + hX;
        }

        void pathfind(Vector2 playerPos, Vector2 AIpos)
        {
            float targetMethod = Method(target, playerPos, AIpos);
            float previousTargetMethod = Method(previousTarget, playerPos, AIpos);
            if (targetMethod > previousTargetMethod)
            {
                Node temp = previousTarget;
                previousTarget = target;
                target = temp;
            }
        }

    }
}
