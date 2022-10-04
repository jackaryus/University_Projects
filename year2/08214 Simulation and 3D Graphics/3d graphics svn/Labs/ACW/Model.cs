using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labs.Utility;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Labs.ACW
{
    public class Model
    {
        private int[] mVBO_IDs = new int[2];
        private int mVAO_ID = 0;
        private int shaderID, Offset = 0;
        private float[] Vertices;
        private int[] Indices;

        public Model(string filePath, int shaderID)
        {
            this.shaderID = shaderID;
            ModelUtility modelUtility = ModelUtility.LoadModel(@filePath);
            Vertices = modelUtility.Vertices;
            Indices = modelUtility.Indices;
            mVAO_ID = GL.GenVertexArray();
            GL.GenBuffers(mVBO_IDs.Length, mVBO_IDs);
            LoadModel();
        }

        public Model(float[] vertices, int[] indices, int shaderID, int offset)
        {
            this.shaderID = shaderID;
            Vertices = vertices;
            Indices = indices;
            Offset = offset;
            mVAO_ID = GL.GenVertexArray();
            GL.GenBuffers(mVBO_IDs.Length, mVBO_IDs);
            LoadModel();
        }

        public void LoadModel()
        {
            GL.BindVertexArray(mVAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vertices.Length * sizeof(float)), Vertices, BufferUsageHint.StaticDraw);           
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[1]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(Indices.Length * sizeof(float)), Indices, BufferUsageHint.StaticDraw);
            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            int vPositionLocation = GL.GetAttribLocation(shaderID, "vPosition");
            //linking normals to shader
            int vNormalLocation = GL.GetAttribLocation(shaderID, "vNormal");

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), Offset);
            //enabled vertex normal on shader
            GL.EnableVertexAttribArray(vNormalLocation);
            //getting last 3 normal/rgb variables after the x,y,z
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            GL.BindVertexArray(0);
        }

        public void RenderModel(Matrix4 modelMatrix, Materials.Material material)
        {
            int uModel = GL.GetUniformLocation(shaderID, "uModel");
            GL.UniformMatrix4(uModel, true, ref modelMatrix);

            SetMaterial(material);

            GL.BindVertexArray(mVAO_ID);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
        }

        private void SetMaterial(Materials.Material material)
        {
            //material
            int uAmbientReflectivity = GL.GetUniformLocation(shaderID, "uMaterial.AmbientReflectivity");
            int uDiffuseReflectivity = GL.GetUniformLocation(shaderID, "uMaterial.DiffuseReflectivity");
            int uSpecularReflectivity = GL.GetUniformLocation(shaderID, "uMaterial.SpecularReflectivity");
            int uShininess = GL.GetUniformLocation(shaderID, "uMaterial.Shininess");
            GL.Uniform3(uAmbientReflectivity, material.ambMaterial);
            GL.Uniform3(uDiffuseReflectivity, material.diffMaterial);
            GL.Uniform3(uSpecularReflectivity, material.specMaterial);
            GL.Uniform1(uShininess, material.shininess);
        }
    }

}
