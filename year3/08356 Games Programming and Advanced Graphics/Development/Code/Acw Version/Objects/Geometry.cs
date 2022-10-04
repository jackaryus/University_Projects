using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OpenGL_Game.Objects
{
    public class Geometry
    {
        List<VertexPositionTexture> vertices = new List<VertexPositionTexture>();
        int numberOfTriangles;
        VertexBuffer vertexBuffer;

        public Geometry()
        {

        }

        public void LoadObject(string filename)
        {
            string line;

            try
            {
                FileStream fin = File.OpenRead(filename);
                StreamReader sr = new StreamReader(fin);
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    string[] values = line.Split(',');

                    if (values[0].StartsWith("NUM_OF_TRIANGLES"))
                    {
                        numberOfTriangles = int.Parse(values[0].Remove(0, "NUM_OF_TRIANGLES".Length));
                        continue;
                    }
                    if (values[0].StartsWith("//") || values.Length < 5) continue;

                    vertices.Add(new VertexPositionTexture(new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2])),
                                 new Vector2(float.Parse(values[3]), float.Parse(values[4]))));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            vertexBuffer = new VertexBuffer(MyGame.gameInstance.GraphicsDevice, typeof(VertexPositionTexture), vertices.Count, BufferUsage.None);
            vertexBuffer.SetData(vertices.ToArray());
        }

        public VertexBuffer VertexBuffer
        {
            get { return vertexBuffer; }
        }

        public int NumberOfTriangles
        {
            get { return numberOfTriangles; }
        }

        public VertexPositionTexture[] Vertices
        {
            get { return vertices.ToArray(); }
        }
    }
}
