using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Objects
{
    class Nodes
    {
        public Vector3 position;

        public List<Nodes> linkedNodes;

        public Nodes(Vector3 inPos)
        {
            position = inPos;
            linkedNodes = new List<Nodes>();
        }
        public void addConnection(Nodes inNode)
        {
            linkedNodes.Add(inNode);
            inNode.linkedNodes.Add(this);
        }
    }
}
