using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenGL_Game.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game
{
    class ComponentAI : IComponent
    {
        public enum targetChoice
        {
            player,
            corner1,
            corner2,
            corner3,
            corner4
        }
        public List<Nodes> nodes { get; set; }
        public List<Entity> walls { get; set; }
        public Nodes targetNode { get; set; }
        public Nodes previousTargetNode { get; set; }
        public float velocity { get; set; }
        public float rotationVelocity { get; set; }
        public Vector3 target { get; set; }
        public targetChoice targetState { get; set; }
        public int chasingPlayerMax { get; set; }
        public int chasingPlayerCount { get; set; }
        private bool enabled { get; set; }
        private KeyboardState previousInput { get; set; }
        public float reloadTime { get; set; }
        public float reloadCount { get; set; }
        public float shootingRange { get; set; }
        private float bulletSpeed { get; set; }
        public EntityManager bullets { get; set; }
        private SystemManager bulletHandler { get; set; }
        public Dictionary<Entity, int> bulletDictionary { get; set; }

        AudioEmitter bulletEmitter;
        AudioListener listener;
        SoundEffect bulletEffect;
        SoundEffectInstance bulletEffectInstance;

        public ComponentAI(List<Nodes> inNodes, List<Entity> inWalls, float inVelocity, float inRotationSpeed, float reload, float inBulletSpeed, float range)
        {
            nodes = inNodes;
            walls = inWalls;
            targetNode = nodes[5];
            previousTargetNode = nodes[0];
            velocity = inVelocity;
            rotationVelocity = inRotationSpeed;
            target = nodes[8].position;
            targetState = targetChoice.corner4;
            chasingPlayerCount = 0;
            chasingPlayerMax = 500;
            enabled = true;
            previousInput = Keyboard.GetState();
            bulletDictionary = new Dictionary<Entity, int>();
            bulletSpeed = inBulletSpeed;
            reloadTime = reload;
            reloadCount = 0;
            shootingRange = range;

            bulletEffect = MyGame.bulletSound;
            bulletEffectInstance = bulletEffect.CreateInstance();
            bulletEffectInstance.IsLooped = false;
            bulletEmitter = new AudioEmitter();
            listener = new AudioListener();

            bullets = new EntityManager();
            bulletHandler = new SystemManager();

            ISystem newSystem;
            newSystem = new SystemRender();
            bulletHandler.AddSystem(newSystem);
            newSystem = new SystemPhysics();
            bulletHandler.AddSystem(newSystem);
        }
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AI; }
        }
        /// <summary>
        /// Sets a path for the AI to follow.
        /// </summary>
        public void nextTarget()
        {
            switch (targetState)
            {
                case targetChoice.corner1:
                    target = nodes[8].position;
                    targetState = targetChoice.corner4;
                    break;
                case targetChoice.corner2:
                    target = nodes[6].position;
                    targetState = targetChoice.corner3;
                    break;
                case targetChoice.corner3:
                    target = nodes[0].position;
                    targetState = targetChoice.corner1;
                    break;
                case targetChoice.corner4:
                    target = nodes[2].position;
                    targetState = targetChoice.corner2;
                    break;
            }
        }
        public void setNewTarget()
        {
            Random rand = new Random();
            int choice = rand.Next(0, 4);
            switch (choice)
            {
                case 0:
                    target = nodes[8].position;
                    targetState = targetChoice.corner4;
                    break;
                case 1:
                    target = nodes[6].position;
                    targetState = targetChoice.corner3;
                    break;
                case 2:
                    target = nodes[0].position;
                    targetState = targetChoice.corner1;
                    break;
                case 3:
                    target = nodes[2].position;
                    targetState = targetChoice.corner2;
                    break;
            }
        }
        public bool isEnabled()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.D) && previousInput.IsKeyUp(Keys.D))
                enabled = !enabled;
            previousInput = keyState;
            return enabled;
        }
        public void createBullet(Vector3 direction, Vector3 position)
        {
            // pos rot sca norm
            Entity bullet = new Entity("bullet");
            ComponentTransform transform = new ComponentTransform(position, direction, new Vector3(0.1f, 0.1f, 0.1f), direction);
            transform.setBoundingBoxCube(new Vector3(-0.1f, -0.1f, -0.1f), new Vector3(0.1f, 0.1f, 0.1f));
            bullet.AddComponent(transform);
            bullet.AddComponent(new ComponentGeometry("Geometry/CubeGeometry.txt"));
            bullet.AddComponent(new ComponentTexture("Textures/bullet.png"));
            bullet.AddComponent(new ComponentVelocity(direction * bulletSpeed));
            bullet.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            bullets.AddEntity(bullet);
            bulletDictionary.Add(bullet, 0);

            bulletEmitter.Position = position;
            updateBulletSound();
            bulletEffectInstance.Play();
        }

        public void updateBulletSound()
        {
            listener.Position = MyGame.cameraPosition;
            Vector3 forward = new Vector3();
            float rot = MyGame.cameraRotation.X - (float)Math.PI;
            forward.X = (float)Math.Cos(rot);
            forward.Z = (float)Math.Sin(rot);
            listener.Forward = forward;
            bulletEffectInstance.Apply3D(listener, bulletEmitter);
        }

        public void updateBullets()
        {
            List<Entity> removeList = new List<Entity>();
            for (int i = bullets.Entities().Count - 1; i >= 0; i-- )
            {
                Entity bullet = bullets.Entities()[i];
                bulletDictionary[bullet] += 1;

                if (bulletDictionary[bullet] > 200)
                {
                    bullets.removeEntity(bullet);
                    removeList.Add(bullet);
                }
                 
                List<IComponent> bulletComponent = bullet.Components;
                IComponent bulletPos = bulletComponent.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                ComponentTransform bulletGeo = (ComponentTransform)bulletPos;

                foreach(Entity wall in walls)
                {
                    List<IComponent> components = wall.Components;

                    IComponent positionComponent = components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                    });
                    ComponentTransform wallGeo = (ComponentTransform)positionComponent;
                    if (bulletGeo.Box.Intersects(wallGeo.Box))
                    {
                        bullets.removeEntity(bullet);
                        removeList.Add(bullet);
                        break;
                    }
                }
            }
            foreach(Entity bullet in removeList)
            {
                bulletDictionary.Remove(bullet);
            }
            if (bullets.Entities().Count > 0)
                bulletHandler.ActionSystems(bullets);
        }
    }
}
