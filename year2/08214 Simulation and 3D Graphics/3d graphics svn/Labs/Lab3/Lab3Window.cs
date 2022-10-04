using Labs.Utility;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace Labs.Lab3
{
    public class Lab3Window : GameWindow
    {
        public Lab3Window()
            : base(
                800, // Width
                600, // Height
                GraphicsMode.Default,
                "Lab 3 Lighting and Material Properties",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, // major
                3, // minor
                GraphicsContextFlags.ForwardCompatible
                )
        {
        }

        private int[] mVBO_IDs = new int[7];
        private int[] mVAO_IDs = new int[4];
        private ShaderUtility mShader;
        private ModelUtility mSphereModelUtility, mDragonModelUtility, mCylinderModelUtility;
        private Matrix4 mView, mSphereModel, mGroundModel, mDragonModel, mCylinderModel;
        private int uAmbientReflectivity, uDiffuseReflectivity, uSpecularReflectivity;
        private int uShininess;



        protected override void OnLoad(EventArgs e)
        {
            // Set some GL state
            GL.ClearColor(Color4.Black);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            mShader = new ShaderUtility(@"Lab3/Shaders/vPassThrough.vert", @"Lab3/Shaders/fLighting.frag");
            GL.UseProgram(mShader.ShaderProgramID);
            int vPositionLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vPosition");
            //linking normals to shader
            int vNormalLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vNormal");

            GL.GenVertexArrays(mVAO_IDs.Length, mVAO_IDs);
            GL.GenBuffers(mVBO_IDs.Length, mVBO_IDs);

            float[] vertices = new float[] {-10, 0, -10,0,1,0,
                                             -10, 0, 10,0,1,0,
                                             10, 0, 10,0,1,0,
                                             10, 0, -10,0,1,0,};

            GL.BindVertexArray(mVAO_IDs[0]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            //enabled vertex normal on shader
            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            //getting last 3 normal/rgb variables after the x,y,z
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            mSphereModelUtility = ModelUtility.LoadModel(@"Utility/Models/sphere.bin");
            mDragonModelUtility = ModelUtility.LoadModel(@"Utility/Models/model.bin");
            mCylinderModelUtility = ModelUtility.LoadModel(@"utility/Models/cylinder.bin");

            //sphere
            GL.BindVertexArray(mVAO_IDs[1]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mSphereModelUtility.Vertices.Length * sizeof(float)), mSphereModelUtility.Vertices, BufferUsageHint.StaticDraw);           
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[2]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mSphereModelUtility.Indices.Length * sizeof(float)), mSphereModelUtility.Indices, BufferUsageHint.StaticDraw);

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mSphereModelUtility.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mSphereModelUtility.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            //enabled vertex normal on shader
            GL.EnableVertexAttribArray(vNormalLocation);
            //getting last 3 normal/rgb variables after the x,y,z
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            //dragon
            GL.BindVertexArray(mVAO_IDs[2]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[3]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mDragonModelUtility.Vertices.Length * sizeof(float)), mDragonModelUtility.Vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[4]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mDragonModelUtility.Indices.Length * sizeof(float)), mDragonModelUtility.Indices, BufferUsageHint.StaticDraw);

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mDragonModelUtility.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mDragonModelUtility.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            //enabled vertex normal on shader
            GL.EnableVertexAttribArray(vNormalLocation);
            //getting last 3 normal/rgb variables after the x,y,z
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            //cylinder
            GL.BindVertexArray(mVAO_IDs[3]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[5]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mCylinderModelUtility.Vertices.Length * sizeof(float)), mCylinderModelUtility.Vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[6]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mCylinderModelUtility.Indices.Length * sizeof(float)), mCylinderModelUtility.Indices, BufferUsageHint.StaticDraw);

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mCylinderModelUtility.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mCylinderModelUtility.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            //enabled vertex normal on shader
            GL.EnableVertexAttribArray(vNormalLocation);
            //getting last 3 normal/rgb variables after the x,y,z
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            GL.BindVertexArray(0);

            mView = Matrix4.CreateTranslation(0, -1.5f, 0);
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);

            mGroundModel = Matrix4.CreateTranslation(0, 0, -5f);
            mSphereModel = Matrix4.CreateTranslation(-2, 1, -5f);
            mDragonModel = Matrix4.CreateTranslation(2, 1, -5f);
            mCylinderModel = Matrix4.CreateTranslation(2, 0, -5f);

            //point light source code
            //int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID,"uLightPosition");
            //Vector4 lightPosition = Vector4.Transform(new Vector4(0, 2, -8.5f, 1), mView);
            //GL.Uniform4(uLightPositionLocation, lightPosition);

            //eye position
            int uEyePositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            GL.Uniform4(uEyePositionLocation, new Vector4(mView.ExtractTranslation(), 1));

            //light properties stuff
            //light
            int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.Position"); 
            Vector4 lightPosition = new Vector4(0, 5, -8.5f, 1); 
            lightPosition = Vector4.Transform(lightPosition, mView); 
            GL.Uniform4(uLightPositionLocation, lightPosition);

            int uAmbientLightLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.AmbientLight"); 
            Vector3 colour = new Vector3(1.0f, 1.0f, 1.0f); 
            GL.Uniform3(uAmbientLightLocation, colour);

            int uDiffuseLightLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.DiffuseLight");
            GL.Uniform3(uDiffuseLightLocation, colour);

            int uSpecularLightLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.SpecularLight");
            GL.Uniform3(uSpecularLightLocation, colour);

            //material
            uAmbientReflectivity = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.AmbientReflectivity");
            uDiffuseReflectivity = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.DiffuseReflectivity");
            uSpecularReflectivity = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.SpecularReflectivity");
            uShininess = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.Shininess");            
            
            base.OnLoad(e);
            
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(this.ClientRectangle);
            if (mShader != null)
            {
                int uProjectionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.5f, 100);
                GL.UniformMatrix4(uProjectionLocation, true, ref projection);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (e.KeyChar == 'w') 
            {
                moveForward();
            }
            if (e.KeyChar == 's')
            {
                moveBackwards();
            }
            if (e.KeyChar == 'a')
            {
                rotateView(-0.025f);          
            }
            if (e.KeyChar == 'd')
            {
                rotateView(0.025f);
            }
            if (e.KeyChar == 'z')
            {
                rotateGround(-0.025f);
            }
            if (e.KeyChar == 'x')
            {
                rotateGround(0.025f);
            }
            if (e.KeyChar == 'c')
            {
                rotateSphere(-0.025f);
            }
            if (e.KeyChar == 'v')
            {
                rotateSphere(0.025f);
            }

            
        }

        private void rotateSphere(float rotation)
        {
            Vector3 s = mSphereModel.ExtractTranslation();
            Matrix4 translation = Matrix4.CreateTranslation(s);
            Matrix4 inverseTranslation = Matrix4.CreateTranslation(-s);
            mSphereModel = mSphereModel * inverseTranslation * Matrix4.CreateRotationY(rotation) * translation;
        }

        private void rotateGround(float rotation)
        {
            Vector3 g = mGroundModel.ExtractTranslation();
            Matrix4 translation = Matrix4.CreateTranslation(g);
            Matrix4 inverseTranslation = Matrix4.CreateTranslation(-g);
            mGroundModel = mGroundModel * inverseTranslation * Matrix4.CreateRotationY(rotation) * translation;
        }

        private void rotateView(float rotation)
        {
            mView = mView * Matrix4.CreateRotationY(rotation);
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);
            int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.Position");
            Vector4 lightPosition = Vector4.Transform(new Vector4(0, 5, -8.5f, 1), mView);
            GL.Uniform4(uLightPositionLocation, lightPosition);
        }

        private void moveBackwards()
        {
            mView = mView * Matrix4.CreateTranslation(0.0f, 0.0f, -0.05f);
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);
            int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.Position");
            Vector4 lightPosition = Vector4.Transform(new Vector4(0, 5, -8.5f, 1), mView);
            GL.Uniform4(uLightPositionLocation, lightPosition);
            int uEyePositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            GL.Uniform4(uEyePositionLocation, new Vector4(mView.ExtractTranslation(), 1));
        }

        private void moveForward()
        {
            mView = mView * Matrix4.CreateTranslation(0.0f, 0.0f, 0.05f);
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);
            int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.Position");
            Vector4 lightPosition = Vector4.Transform(new Vector4(0, 5, -8.5f, 1), mView);
            GL.Uniform4(uLightPositionLocation, lightPosition);
            int uEyePositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            GL.Uniform4(uEyePositionLocation, new Vector4(mView.ExtractTranslation(), 1));
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //ground
            int uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            GL.UniformMatrix4(uModel, true, ref mGroundModel);

            SetMaterial(0.05f, 0.05f, 0.05f, 0.5f, 0.5f, 0.5f, 0.7f, 0.7f, 0.7f, 0.078125f);

            GL.BindVertexArray(mVAO_IDs[0]);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            //sphere
            Matrix4 m = mSphereModel * mGroundModel;
            uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            GL.UniformMatrix4(uModel, true, ref m);

            SetMaterial(0.25f, 0.20725f, 0.20725f, 1f, 0.829f, 0.829f, 0.296648f, 0.296648f, 0.296648f, 0.088f);

            GL.BindVertexArray(mVAO_IDs[1]);
            GL.DrawElements(PrimitiveType.Triangles, mSphereModelUtility.Indices.Length, DrawElementsType.UnsignedInt, 0);

            //dragon
            m = mDragonModel * mGroundModel;
            GL.UniformMatrix4(uModel, true, ref m);

            SetMaterial(0.24725f, 0.1995f, 0.0745f, 0.75164f, 0.60648f, 0.22648f, 0.628281f, 0.555802f, 0.366065f, 0.4f);

            GL.BindVertexArray(mVAO_IDs[2]);
            GL.DrawElements(PrimitiveType.Triangles, mDragonModelUtility.Indices.Length, DrawElementsType.UnsignedInt, 0);

            //*
            //cylinder
            m = mCylinderModel * mGroundModel;
            GL.UniformMatrix4(uModel, true, ref m);

            SetMaterial(0.0f, 0.0f, 0.0f, 0.5f, 0.0f, 0.0f, 0.7f, 0.6f, 0.6f, 0.25f);

            GL.BindVertexArray(mVAO_IDs[3]);
            GL.DrawElements(PrimitiveType.Triangles, mCylinderModelUtility.Indices.Length, DrawElementsType.UnsignedInt, 0);
            
             //*/ 
            GL.BindVertexArray(0);
            this.SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.DeleteBuffers(mVBO_IDs.Length, mVBO_IDs);
            GL.DeleteVertexArrays(mVAO_IDs.Length, mVAO_IDs);
            mShader.Delete();
            base.OnUnload(e);
        }

        private void SetMaterial(float ambRed, float ambGreen, float ambBlue, float diffRed, float diffGreen, float diffBlue, float specRed, float specGreen, float specBlue, float Shininess)
        {
            Vector3 ambMaterial = new Vector3(ambRed, ambGreen, ambBlue);
            Vector3 diffMaterial = new Vector3(diffRed, diffGreen, diffBlue);
            Vector3 specMaterial = new Vector3(specRed, specGreen, specBlue);
            GL.Uniform3(uAmbientReflectivity, ambMaterial);
            GL.Uniform3(uDiffuseReflectivity, diffMaterial);
            GL.Uniform3(uSpecularReflectivity, specMaterial);
            GL.Uniform1(uShininess, Shininess * 128);
        }
    }
}
