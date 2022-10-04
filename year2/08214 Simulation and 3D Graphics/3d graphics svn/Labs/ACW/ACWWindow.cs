using Labs.Utility;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Timers;
using System.Collections.Generic;

namespace Labs.ACW
{
    public class ACWWindow : GameWindow
    {
        public ACWWindow()
            : base(
                800, // Width
                600, // Height
                GraphicsMode.Default,
                "Assessed Coursework",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, // major
                3, // minor
                GraphicsContextFlags.ForwardCompatible
                )
        {
        }

        
        private ShaderUtility mShader;
        private Matrix4 mView;
        private Model sphere, cube, cylinder;
        private Vector4 lightPos = new Vector4(0, -10, 50, 1);
        private Timer mTimer;
        private float spawnTimer = 1;
        //private Vector3 gravity = new Vector3(0, -9.8f, 0);
        private float smallSphereRadius = 4.0f, largeSphereRadius = 7.0f, smallSphereDensity = 1000f, largeSphereDensity = 1200f;

        private Matrix4 smallCylinderMatrix1, smallCylinderMatrix2, largeCylinderMatrix1,
                        LargeCylinderMatrix2, smallUnalinedCylinderMatrix, largeUnalinedCylinderMatrix;
                    

        public static List<Sphere> sphereList = new List<Sphere>(); 

        private static Random randomNum = new Random();

        #region Cube vert and indices
        float[] vertices = new float[] {
                    //bottom face
                    -1f, -1f, -1f, 0f, 1f, 0f,
                    1f, -1f, -1f,  0f, 1f, 0f,
                    1f, -1f, 1f,   0f, 1f, 0f,
                    -1f, -1f, 1f,  0f, 1f, 0f,
                    //top face
                    -1f, 1f, -1f,  0f, -1f, 0f,
                    1f, 1f, -1f,   0f, -1f, 0f,
                    1f, 1f, 1f,    0f, -1f, 0f,
                    -1f, 1f, 1f,   0f, -1f, 0f,
                    //left face
                    -1f, -1f, 1f,  1f, 0f, 0f,
                    -1f, -1f, -1f, 1f, 0f, 0f,
                    -1f, 1f, -1f,  1f, 0f, 0f,
                    -1f, 1f, 1f,   1f, 0f, 0f,
                    //right face
                    1f, -1f, -1f,  -1f, 0f, 0f,
                    1f, -1f, 1f,   -1f, 0f, 0f,
                    1f, 1f, 1f,    -1f, 0f, 0f,
                    1f, 1f, -1f,   -1f, 0f, 0f,
                    //front face
                    -1f, -1f, -1f, 0f, 0f, 1f,
                    1f, -1f, -1f,  0f, 0f, 1f,
                    1f, 1f, -1f,   0f, 0f, 1f,
                    -1f, 1f, -1f,  0f, 0f, 1f,
                    //back face
                    1f, -1f, 1f,   0f, 0f, -1f,
                    -1f, -1f, 1f,  0f, 0f, -1f,
                    -1f, 1f, 1f,   0f, 0f, -1f,
                    1f, 1f, 1f,    0f, 0f, -1f
                };
        int[] indices = new int[] {
                    0, 1, 2,        0, 2, 3,
                    4, 6, 5,        4, 7, 6,
                    8, 10, 9,     8, 11, 10,
                    12, 14, 13,  12, 15, 14,
                    16, 18, 17,  16, 19, 18,
                    20, 22, 21,  20, 23, 22
                };
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color4.SkyBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            mShader = new ShaderUtility(@"ACW/Shaders/VertexShader.vert", @"ACW/Shaders/FragmentShader.frag");
            GL.UseProgram(mShader.ShaderProgramID);

            mView = Matrix4.CreateTranslation(0, 50f, -150f);
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);

            //camera position
            int uEyePositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            GL.Uniform4(uEyePositionLocation, new Vector4(mView.ExtractTranslation(), 1));

            //lightPos = new Vector4(uEyePositionLocation);

            //light properties stuff
            //light
            int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.Position");
            Vector4 lightPosition = new Vector4(lightPos);
            lightPosition = Vector4.Transform(lightPosition, mView);
            GL.Uniform4(uLightPositionLocation, lightPosition);

            int uAmbientLightLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.AmbientLight");
            Vector3 colour = new Vector3(1.0f, 1.0f, 1.0f);
            GL.Uniform3(uAmbientLightLocation, colour);

            int uDiffuseLightLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.DiffuseLight");
            GL.Uniform3(uDiffuseLightLocation, colour);

            int uSpecularLightLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.SpecularLight");
            GL.Uniform3(uSpecularLightLocation, colour);

            mTimer = new Timer();
            mTimer.Start();

            //cube = new Model(vertices, indices, mShader.ShaderProgramID, 0);
            cube = new Model(vertices, indices, mShader.ShaderProgramID, 0);
            sphere = new Model("Utility/Models/sphere.bin", mShader.ShaderProgramID);
            cylinder = new Model("Utility/Models/cylinder.bin", mShader.ShaderProgramID);

            sphereList.Add(new Sphere(sphere, new Vector3(randomNum.Next(1, 80) - 40, -randomNum.Next(50, 100) - 40, randomNum.Next(1, 80) - 40), smallSphereRadius, Materials.ForestGreen, smallSphereDensity));
            sphereList.Add(new Sphere(sphere, new Vector3(randomNum.Next(1, 80) - 40, -randomNum.Next(50, 100) - 40, randomNum.Next(1, 80) - 40), largeSphereRadius, Materials.Yellow, largeSphereDensity));

            Collider c = new Collider(Collider.CollisionType.ballofdeath, new Vector3(0, -350, 0), Vector3.Zero);
            c.radius = 35;

            smallCylinderMatrix1 = Matrix4.CreateScale(7.5f, 50f, 7.5f);
            smallCylinderMatrix1 *= Matrix4.CreateRotationZ((float)(Math.PI / 2));
            smallCylinderMatrix1 *= Matrix4.CreateTranslation(0, -125f,0);

            smallCylinderMatrix2 = Matrix4.CreateScale(7.5f, 50f, 7.5f);
            smallCylinderMatrix2 *= Matrix4.CreateRotationX((float)(Math.PI / 2));
            smallCylinderMatrix2 *= Matrix4.CreateTranslation(0, -125f, 0);

            largeCylinderMatrix1 = Matrix4.CreateScale(15f, 50f, 15f);
            largeCylinderMatrix1 *= Matrix4.CreateRotationZ((float)(Math.PI / 2));
            largeCylinderMatrix1 *= Matrix4.CreateTranslation(0, -175f, 0);

            LargeCylinderMatrix2 = Matrix4.CreateScale(15f, 50f, 15f);
            LargeCylinderMatrix2 *= Matrix4.CreateRotationX((float)(Math.PI / 2));
            LargeCylinderMatrix2 *= Matrix4.CreateTranslation(0, -175f, 0);

            smallUnalinedCylinderMatrix = Matrix4.CreateScale(10f, 70f, 10f);
            smallUnalinedCylinderMatrix *= Matrix4.CreateRotationZ((float)(Math.PI / 2));
            smallUnalinedCylinderMatrix *= Matrix4.CreateRotationY((float)(Math.PI / 4));
            smallUnalinedCylinderMatrix *= Matrix4.CreateTranslation(0, -250f, 0);

            largeUnalinedCylinderMatrix = Matrix4.CreateScale(15f, 75f, 15f);
            largeUnalinedCylinderMatrix *= Matrix4.CreateRotationZ((float)((Math.PI / 2) - (Math.PI / 6)));
            largeUnalinedCylinderMatrix *= Matrix4.CreateRotationY((float)(-(Math.PI / 4)));
            largeUnalinedCylinderMatrix *= Matrix4.CreateTranslation(0, -250f, 0);

            Collider cylinderCollision = new Collider(Collider.CollisionType.cylinder, smallCylinderMatrix1.ExtractTranslation(), Vector3.Zero);
            cylinderCollision.rotation = smallCylinderMatrix1.ExtractRotation();
            cylinderCollision.radius = 7.5f;
            cylinderCollision.length = 50;
            cylinderCollision = new Collider(Collider.CollisionType.cylinder, smallCylinderMatrix2.ExtractTranslation(), Vector3.Zero);
            cylinderCollision.rotation = smallCylinderMatrix2.ExtractRotation();
            cylinderCollision.radius = 7.5f;
            cylinderCollision.length = 50;
            cylinderCollision = new Collider(Collider.CollisionType.cylinder, largeCylinderMatrix1.ExtractTranslation(), Vector3.Zero);
            cylinderCollision.rotation = largeCylinderMatrix1.ExtractRotation();
            cylinderCollision.radius = 15f;
            cylinderCollision.length = 50;
            cylinderCollision = new Collider(Collider.CollisionType.cylinder, LargeCylinderMatrix2.ExtractTranslation(), Vector3.Zero);
            cylinderCollision.rotation = LargeCylinderMatrix2.ExtractRotation();
            cylinderCollision.radius = 15f;
            cylinderCollision.length = 50;
            cylinderCollision = new Collider(Collider.CollisionType.cylinder, smallUnalinedCylinderMatrix.ExtractTranslation(), Vector3.Zero);
            cylinderCollision.rotation = smallUnalinedCylinderMatrix.ExtractRotation();
            cylinderCollision.radius = 10f;
            cylinderCollision.length = 70;
            cylinderCollision = new Collider(Collider.CollisionType.cylinder, largeUnalinedCylinderMatrix.ExtractTranslation(), Vector3.Zero);
            cylinderCollision.rotation = largeUnalinedCylinderMatrix.ExtractRotation();
            cylinderCollision.radius = 15f;
            cylinderCollision.length = 75;

            base.OnLoad(e);
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
            if (e.KeyChar == 'r')
            {
                moveUp();
            }
            if (e.KeyChar == 'f')
            {
                moveDown();
            }
            if (e.KeyChar == 'e')
            {
                rotateTower(0.024f);
            }
            if (e.KeyChar == 'q')
            {
                rotateTower(-0.024f);
            }
        }

        private void rotateView(float rotation)
        {
            mView = mView * Matrix4.CreateRotationY(rotation);
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);
            int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.Position");
            Vector4 lightPosition = Vector4.Transform(lightPos, mView);
            GL.Uniform4(uLightPositionLocation, lightPosition);
            int uEyePositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            GL.Uniform4(uEyePositionLocation, new Vector4(mView.ExtractTranslation(), 1));
        }

        private void moveBackwards()
        {
            mView = mView * Matrix4.CreateTranslation(0.0f, 0.0f, -1f);
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);
            int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.Position");
            Vector4 lightPosition = Vector4.Transform(lightPos, mView);
            GL.Uniform4(uLightPositionLocation, lightPosition);
            int uEyePositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            GL.Uniform4(uEyePositionLocation, new Vector4(mView.ExtractTranslation(), 1));
        }

        private void moveForward()
        {
            mView = mView * Matrix4.CreateTranslation(0.0f, 0.0f, 1f);
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);
            int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.Position");
            Vector4 lightPosition = Vector4.Transform(lightPos, mView);
            GL.Uniform4(uLightPositionLocation, lightPosition);
            int uEyePositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            GL.Uniform4(uEyePositionLocation, new Vector4(mView.ExtractTranslation(), 1));
        }

        private void moveUp()
        {
            mView = mView * Matrix4.CreateTranslation(0.0f, -5f, 0.0f);
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);
            int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.Position");
            Vector4 lightPosition = Vector4.Transform(lightPos, mView);
            GL.Uniform4(uLightPositionLocation, lightPosition);
            int uEyePositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            GL.Uniform4(uEyePositionLocation, new Vector4(mView.ExtractTranslation(), 1));
        }

        private void moveDown()
        {
            mView = mView * Matrix4.CreateTranslation(0.0f, 5f, 0.0f);
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);
            int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.Position");
            Vector4 lightPosition = Vector4.Transform(lightPos, mView);
            GL.Uniform4(uLightPositionLocation, lightPosition);
            int uEyePositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            GL.Uniform4(uEyePositionLocation, new Vector4(mView.ExtractTranslation(), 1));
        }

        private void rotateTower(float rotation)
        {
            mView = Matrix4.CreateRotationY(rotation) * mView;
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);
            int uLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight.Position");
            Vector4 lightPosition = Vector4.Transform(lightPos, mView);
            GL.Uniform4(uLightPositionLocation, lightPosition);
            int uEyePositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            GL.Uniform4(uEyePositionLocation, new Vector4(mView.ExtractTranslation(), 1));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(this.ClientRectangle);
            if (mShader != null)
            {
                int uProjectionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.5f, 1000);
                GL.UniformMatrix4(uProjectionLocation, true, ref projection);
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
 	        base.OnUpdateFrame(e);
            
            float timestep = mTimer.GetElapsedSeconds();
            spawnTimer -= timestep;
            if (spawnTimer <= 0)
            {
                spawnTimer = 1;
                sphereList.Add(new Sphere(sphere, new Vector3(randomNum.Next(1, 80) - 40, -randomNum.Next(50, 100) + 40, randomNum.Next(1, 80) - 40), smallSphereRadius, Materials.ForestGreen, smallSphereDensity));
                sphereList.Add(new Sphere(sphere, new Vector3(randomNum.Next(1, 80) - 40, -randomNum.Next(50, 100) + 40, randomNum.Next(1, 80) - 40), largeSphereRadius, Materials.Yellow, largeSphereDensity));
                
            }
            for (int i = 0; i < sphereList.Count; i++)
            {
                Sphere s = sphereList[i];
                s.Update(timestep);
                if (s.removed == true)
                {
                    i--;
                }
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            Matrix4 matrix;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            //tower
            GL.CullFace(CullFaceMode.Front);
            for (int i = 0; i < 4; i++)
            {
                matrix = Matrix4.CreateScale(50f);
                matrix *= Matrix4.CreateTranslation(0, -50f, 0);
                matrix *= Matrix4.CreateTranslation(0, -(i * 100), 0);
                cube.RenderModel(matrix, Materials.Obsidian);
            }
            GL.CullFace(CullFaceMode.Back);

            //emitterbox
            foreach (Sphere s in sphereList)
            {
                s.Render();
            }

            //lvl1 gridbox axis alined
            //smallcylinder1
            cylinder.RenderModel(smallCylinderMatrix1, Materials.Silver);
            //smallcylinder2
            cylinder.RenderModel(smallCylinderMatrix2, Materials.Silver);
            //bigcylinder1
            cylinder.RenderModel(largeCylinderMatrix1, Materials.Silver);
            //bigcylinder2
            cylinder.RenderModel(LargeCylinderMatrix2, Materials.Silver);

            //lvl2 gridbox axis unalined
            //Smallcylinder
            cylinder.RenderModel(smallUnalinedCylinderMatrix, Materials.Silver);
            //bigcylinder
            cylinder.RenderModel(largeUnalinedCylinderMatrix, Materials.Silver);

            //sphere of doom
            matrix = Matrix4.CreateScale(35f);
            matrix *= Matrix4.CreateTranslation(0, -350, 0);
            sphere.RenderModel(matrix, Materials.Orange);

            this.SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }

    }
}
