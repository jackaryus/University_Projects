using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITest
{
    public class Node
    {
        public Vector2 position;
        public Rectangle intersectionRectangle;
        public List<Node> nodesConnected {get;set;}

        public Node(Vector2 inPos)
        {
            position = inPos;
            intersectionRectangle = new Rectangle((int)position.X, (int)position.Y, 10, 10);
            nodesConnected = new List<Node>();
        }

        public void addConnection(Node inNode)
        {
            nodesConnected.Add(inNode);
            inNode.nodesConnected.Add(this);
        }
    }
}
