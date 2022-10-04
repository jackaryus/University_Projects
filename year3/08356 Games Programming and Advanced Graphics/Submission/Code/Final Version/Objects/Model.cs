using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OpenGL_Game.Objects
{
    public class Model
    {
        //List<VertexPositionTexture> vertices = new List<VertexPositionTexture>();
        //int numberOfTriangles;
        //VertexBuffer vertexBuffer;
        public Microsoft.Xna.Framework.Graphics.Model modelobj { get; set; }

        public Model(Microsoft.Xna.Framework.Graphics.Model key)
        {
            modelobj = key;
        }

        public void LoadObject(string filename)
        {
            try
            {
                modelobj = MyGame.gameInstance.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
