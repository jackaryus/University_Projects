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
    class Collider
    {
        public static List<Collider> colliderList = new List<Collider>();

        public enum CollisionType {sphere, box, cylinder, ballofdeath}
        private CollisionType collisiontype;
        private Vector3 objectPosition, objectVelocity, PrevPosition;
        static Vector3 gravity = new Vector3(0, -9.81f, 0);
        public float radius, objectMass, objectDensity;
        public Quaternion rotation;
        public float length, restitution = 0.2f;

        public Collider(CollisionType type, Vector3 position, Vector3 velocity, float density , float radius)
        {
            collisiontype = type;
            objectPosition = position;
            objectVelocity = velocity;
            objectDensity = density;
            objectMass = (float)(objectDensity * ((4f / 3f) * Math.PI * (radius * radius * radius)));
            //objectVelocity.X = 10.0f;

            colliderList.Add(this);
        }

        public Collider (CollisionType type, Vector3 position, Vector3 velocity)
        {
            collisiontype = type;
            objectPosition = position;
            objectVelocity = velocity;
            //objectVelocity.X = 10.0f;

            colliderList.Add(this);
        }

        public bool move(float timestep)
        {
            PrevPosition = objectPosition;

            //objectVelocity += gravity * timestep * 5;
            objectPosition += objectVelocity * timestep;
            objectVelocity += gravity * timestep * 5;

            foreach (Collider c in colliderList)
            {
                if (c == this)
                {
                    continue;
                }
                else if (TestCollision(c))
                {
                    objectPosition = PrevPosition;
                    //return false;
                }
            }
            if (SphereonBoxCollision())
            {
                objectPosition = PrevPosition;
                //return false;
            }
            PrevPosition = objectPosition;
            return true;
        }

        public bool TestCollision(Collider other)
        {
            switch (other.collisiontype)
            {
                case CollisionType.sphere:
                    return SphereonSphereCollision(other);

                case CollisionType.cylinder:
                    return SphereonCylinderCollision(other);

                case CollisionType.ballofdeath:
                    return SphereonDeathCollision(other);
                  
            }
            return false;
        }

        public bool SphereonSphereCollision(Collider other)
        {
            
            Vector3 circleDistance = objectPosition - other.objectPosition;
            float totalRadius = radius + other.radius;
            if (circleDistance.Length <= totalRadius)
            {

                float momentumBefore = objectVelocity.Length * objectMass + other.objectVelocity.Length * other.objectMass;
                Vector3 normal = circleDistance.Normalized();

                if (Vector3.Dot(normal, objectVelocity) != 0)
                {
                    objectVelocity = (objectVelocity - Vector3.Dot(normal, objectVelocity) * normal);
                   
                }

                if (Vector3.Dot(-normal, other.objectVelocity) != 0)
                {
                    other.objectVelocity = (other.objectVelocity - Vector3.Dot(-normal, other.objectVelocity) * -normal);
                }

                Vector3 oldVelocity = objectVelocity;
                
                //elastic
                    objectVelocity = objectVelocity - (1 + 0.4f) * Vector3.Dot(normal, objectVelocity) * normal;
                //inelastic with mass (sticky balls)
                    //objectVelocity = (((objectMass - other.objectMass) / (objectMass + other.objectMass)) * objectVelocity) + (((2.0f * other.objectMass) / (objectMass + other.objectMass)) * other.objectVelocity);
                    //objectVelocity = ((((objectMass * objectVelocity) + (other.objectMass * other.objectVelocity) + ((restitution * other.objectMass) * (other.objectVelocity - objectVelocity))) / (objectMass - other.objectMass))) * normal;

                //elastic other
                    other.objectVelocity = other.objectVelocity - (1 + 0.4f) * Vector3.Dot(-normal, objectVelocity) * -normal;
                //inelastic with mass other (sticky balls)
                    //other.objectVelocity = (((other.objectMass - objectMass) / (other.objectMass + objectMass)) * other.objectVelocity) + (((2.0f * objectMass) / (other.objectMass + objectMass)) * oldVelocity);
                    //other.objectVelocity = ((((other.objectMass * other.objectVelocity) + (objectMass * oldVelocity) + ((restitution * objectMass) * (oldVelocity - other.objectVelocity))) / (other.objectMass - objectMass))) * -normal;
                
                float momentumAfter = objectVelocity.Length * objectMass + other.objectVelocity.Length * other.objectMass;
                //if(Math.Abs(momentumAfter - momentumBefore) > 0.000001f)
                //{
                    //int here;
                    //Console.WriteLine("here");
               // }
                return true;
            }
            return false;
        }

        public bool SphereonDeathCollision(Collider other)
        {
            Vector3 circleDistance = objectPosition - other.objectPosition;
            float totalRadius = radius + other.radius;
            if (circleDistance.Length < totalRadius)
            {
                Vector3 normal = circleDistance.Normalized();
                float intersection = totalRadius - circleDistance.Length;
                objectPosition += normal * intersection;
                radius -= intersection;
                objectMass = (float)(objectDensity * ((4f / 3f) * Math.PI * (radius * radius * radius)));
            }
            return false;
        }

        public bool SphereonCylinderCollision(Collider other)
        {
            Vector3 cylinderPosition = Vector3.Transform(other.objectPosition, other.rotation.Inverted());
            Vector3 spherePosition = Vector3.Transform(objectPosition, other.rotation.Inverted());
            if (spherePosition.Y - radius <= cylinderPosition.Y + other.length && spherePosition.Y >= cylinderPosition.Y - other.length)
            {
                Vector2 circleDistance = spherePosition.Xz - cylinderPosition.Xz;
                float totalRadius = radius + other.radius;
                if (circleDistance.Length < totalRadius)
                {
                    Vector3 normal = new Vector3(circleDistance.X, 0, circleDistance.Y).Normalized();
                    normal = Vector3.Transform(normal, Matrix4.CreateFromQuaternion(other.rotation));
                    normal.Normalize();
                    objectVelocity = objectVelocity - (1 + 0.4f) * Vector3.Dot(normal, objectVelocity) * normal;
                    return true;
                }
            }
            return false;
        }

        public bool SphereonBoxCollision()
        {
            if (objectPosition.X + radius > 50)
            {
                Vector3 normal = new Vector3(-1, 0, 0);
                objectVelocity = objectVelocity - (1 + 0.4f) * Vector3.Dot(normal, objectVelocity) * normal;
                return true;
            }
            if (objectPosition.X - radius < -50)
            {
                Vector3 normal = new Vector3(1, 0, 0);
                objectVelocity = objectVelocity - (1 + 0.4f) * Vector3.Dot(normal, objectVelocity) * normal;
                return true;
            }
            if (objectPosition.Y + radius > 0)
            {
                Vector3 normal = new Vector3(0, -1, 0);
                objectVelocity = objectVelocity - (1 + 0.4f) * Vector3.Dot(normal, objectVelocity) * normal;
                return true;
            }
            if (objectPosition.Y - radius < -400)
            {
                Vector3 normal = new Vector3(0, 1, 0);
                objectVelocity = objectVelocity - (1 + 0.4f) * Vector3.Dot(normal, objectVelocity) * normal;
                return true;
            }
            if (objectPosition.Z + radius > 50)
            {
                Vector3 normal = new Vector3(0, 0, -1);
                objectVelocity = objectVelocity - (1 + 0.4f) * Vector3.Dot(normal, objectVelocity) * normal;
                return true;
            }
            if (objectPosition.Z - radius < -50)
            {
                Vector3 normal = new Vector3(0, 0, 1);
                objectVelocity = objectVelocity - (1 + 0.4f) * Vector3.Dot(normal, objectVelocity) * normal;
                return true;
            }

            return false;
        }

        public Vector3 GetPosition()
        {
            return objectPosition;
        }

        public void Remove()
        {
            colliderList.Remove(this);
        }
    }
}
