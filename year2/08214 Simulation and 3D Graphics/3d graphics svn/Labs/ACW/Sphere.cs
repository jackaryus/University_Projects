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
    public class Sphere 
    {
        private Model msphere;
        private Matrix4 matrix;
        private float mradius; //mmass;
        private Materials.Material sphereMaterial;
        //static Vector3 gravity = new Vector3(0, -9.81f, 0);
        private Collider collider;
        public bool removed = false;
        private Vector3 mposition;
        public static int numberofobjects;

        public Sphere(Model sphere, Vector3 position, float radius, Materials.Material material, float density)
        {
            msphere = sphere;
            mradius = radius;
            mposition = position;
            //mmass = (float)(density * ((4f/3f) * Math.PI * (mradius * mradius * mradius)));
            sphereMaterial = material;
            matrix = Matrix4.CreateScale(radius) * Matrix4.CreateTranslation(position);
            collider = new Collider(Collider.CollisionType.sphere, position, Vector3.Zero, density, mradius);
            collider.radius = mradius;
            numberofobjects++;
            Console.WriteLine("number of balls =" + numberofobjects);
        }

        public void Update(float timestep)
        {
            if (collider.move(timestep))
            {
                Vector3 position = collider.GetPosition();
                mradius = collider.radius;
                matrix = Matrix4.CreateScale(mradius) * Matrix4.CreateTranslation(position);
                //matrix *= Matrix4.CreateTranslation(mSphereVelocity * timestep);
                if (this.mradius <= 0 || this.mposition.Y + this.mradius <= -400)
                {
                    this.Remove();
                }
            }
        }

        public void Render()
        {
            this.msphere.RenderModel(matrix, sphereMaterial);
        }

        public void Remove()
        {

            removed = true;
            ACWWindow.sphereList.Remove(this);
            collider.Remove();
            Console.WriteLine("number of balls =" + numberofobjects);
        }
    }  
}
