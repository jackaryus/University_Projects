﻿using OpenTK;
using System;
using OpenTK.Graphics;
using Labs.Utility;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Labs.Lab2
{
    public class Lab2_2Window : GameWindow
    {
        public Lab2_2Window()
            : base(
                800, // Width
                600, // Height
                GraphicsMode.Default,
                "Lab 2_2 Understanding the Camera",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, // major
                3, // minor
                GraphicsContextFlags.ForwardCompatible
                )
        {
        }

        private int[] mVBO_IDs = new int[2];
        private int mVAO_ID;
        private ShaderUtility mShader;
        private ModelUtility mModel;
        private Matrix4 mView;

        protected override void OnLoad(EventArgs e)
        {
            // Set some GL state
            GL.ClearColor(Color4.DodgerBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            mModel = ModelUtility.LoadModel(@"Utility/Models/lab22model.sjg");    
            mShader = new ShaderUtility(@"Lab2/Shaders/vLab22.vert", @"Lab2/Shaders/fSimple.frag");
            GL.UseProgram(mShader.ShaderProgramID);
            int vPositionLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vPosition");
            int vColourLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vColour");

            mVAO_ID = GL.GenVertexArray();
            GL.GenBuffers(mVBO_IDs.Length, mVBO_IDs);
            
            GL.BindVertexArray(mVAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mModel.Vertices.Length * sizeof(float)), mModel.Vertices, BufferUsageHint.StaticDraw);           
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[1]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mModel.Indices.Length * sizeof(float)), mModel.Indices, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mModel.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mModel.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vColourLocation);
            GL.VertexAttribPointer(vColourLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            GL.BindVertexArray(0);

            // added code
            mView = Matrix4.CreateTranslation(0, 0, -2);
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);

            int uProjectionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.5f, 110);
            GL.UniformMatrix4(uProjectionLocation, true, ref projection);

            base.OnLoad(e);
            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.BindVertexArray(mVAO_ID);

            int uModelLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");

            Matrix4 m1;
            for(int x = 0; x < 10; x++)
            {
                for(int y = 0; y < 10; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        m1 = Matrix4.CreateTranslation(x * 0.5f, y * 0.5f, z * 0.5f);
                        GL.UniformMatrix4(uModelLocation, true, ref m1);
                        GL.DrawElements(BeginMode.Triangles, mModel.Indices.Length, DrawElementsType.UnsignedInt, 0);
                    }
                }
            }

            /*
            m1 *= Matrix4.CreateRotationZ(0.8f);
            //Matrix4 r1 = Matrix4.CreateRotationZ(0.8f);
            GL.UniformMatrix4(uModelLocation, true, ref m1);
            //GL.UniformMatrix4(uModelLocation, true, ref r1);
            GL.DrawElements(BeginMode.Triangles, mModel.Indices.Length, DrawElementsType.UnsignedInt, 0);

            m1 = Matrix4.CreateRotationZ(0.8f);
            m1 *= Matrix4.CreateTranslation(-0.5f, 0, 0);
            //Matrix4 r2 = Matrix4.CreateRotationZ(0.8f);
            //GL.UniformMatrix4(uModelLocation2, true, ref r2);
            GL.UniformMatrix4(uModelLocation, true, ref m1);
            GL.DrawElements(BeginMode.Triangles, mModel.Indices.Length, DrawElementsType.UnsignedInt, 0);
            */

            GL.BindVertexArray(0);
            this.SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.DeleteBuffers(mVBO_IDs.Length, mVBO_IDs);
            GL.DeleteVertexArray(mVAO_ID);
            mShader.Delete();
            base.OnUnload(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            float cameraSpeed = 0.01f;
            base.OnKeyPress(e);
            if (e.KeyChar == 'a')
            {
                mView = mView * Matrix4.CreateTranslation(-cameraSpeed, 0, 0);
                MoveCamera();
            }

            if (e.KeyChar == 'd')
            {
                mView = mView * Matrix4.CreateTranslation(cameraSpeed, 0, 0);
                MoveCamera();
            }

            if (e.KeyChar == 'w')
            {
                mView = mView * Matrix4.CreateTranslation(0, 0, cameraSpeed);
                MoveCamera();
            }

            if (e.KeyChar == 's')
            {
                mView = mView * Matrix4.CreateTranslation(0, 0, -cameraSpeed);
                MoveCamera();
            }

            if (e.KeyChar == 'x')
            {
                mView = mView * Matrix4.CreateRotationY(cameraSpeed);
                MoveCamera();
            }

            if (e.KeyChar == 'z')
            {
                mView = mView * Matrix4.CreateRotationY(-cameraSpeed);
                MoveCamera();
            }
             
        }

        private void MoveCamera()
        {
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(this.ClientRectangle);
            if (mShader != null)
            {
                int uProjectionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.5f, 110);
                GL.UniformMatrix4(uProjectionLocation, true, ref projection);

                /*ortho view
                int uProjectionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
                int windowHeight = this.ClientRectangle.Height;
                int windowWidth = this.ClientRectangle.Width;

                if (windowHeight > windowWidth)
                {
                    if (windowWidth < 1) { windowWidth = 1; }
                    float ratio = windowHeight / windowWidth;
                    Matrix4 projection = Matrix4.CreateOrthographic(10, ratio * 10, -1, 1);
                    GL.UniformMatrix4(uProjectionLocation, true, ref projection);
                }
                else
                {
                    if (windowHeight < 1) { windowHeight = 1; }
                    float ratio = windowWidth / windowHeight;
                    Matrix4 projection = Matrix4.CreateOrthographic(10, ratio * 10, -1, 1);
                    GL.UniformMatrix4(uProjectionLocation, true, ref projection);
                }
                */
            }
        }
    }
}
