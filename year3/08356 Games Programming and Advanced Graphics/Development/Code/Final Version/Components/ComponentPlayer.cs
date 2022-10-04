using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenGL_Game.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace OpenGL_Game.Components
{

    class ComponentPlayer : IComponent
    {
        public bool collisionsEnabled { get; set; }
        public bool KeyPickUP { get; set; }
        public bool takeDamage { get; set; }
        public KeyboardState previousKeyState { get; set; }
        public int maxHealth { get; set; }
        public int health { get; set; }
        public int keycount = 0;
        public bool portal;

        public Entity[] keyArray { get; set; }
        public List<Entity> entities { get; set; }
        public EntityManager keys { get; set; }
        private SystemManager keyManager { get; set; }

        AudioEmitter keyEmitter;
        AudioListener listener;
        SoundEffect keyEffect;
        SoundEffectInstance keyEffectInstance;
        AudioEmitter portalEmitter;
        SoundEffect portalEffect;
        SoundEffectInstance portalEffectInstance;

        public Entity removedKey { get; set; }

        Microsoft.Xna.Framework.Graphics.Model key;

        public ComponentPlayer(List<Entity> inEntities, Microsoft.Xna.Framework.Graphics.Model inkey)
        {
            //this sets up the players variables
            maxHealth = 5;
            health = 5;
            entities = inEntities;
            portal = false;
            collisionsEnabled = true;
            KeyPickUP = false;
            takeDamage = false;
            previousKeyState = Keyboard.GetState();
            key = inkey;
            

            //this sets up the sound handle for both key and portal and starts to play the closed portal sound
            keyEffect = MyGame.keySound;
            portalEffect = MyGame.portalclosedSound; 
            keyEffectInstance = keyEffect.CreateInstance();
            portalEffectInstance = portalEffect.CreateInstance();
            keyEffectInstance.IsLooped = false;
            portalEffectInstance.IsLooped = true;
            keyEmitter = new AudioEmitter();
            portalEmitter = new AudioEmitter();
            listener = new AudioListener();
            portalEmitter.Position = new Vector3(-27.4f, 0.0f, -20.0f);
            PortalSoundUpdate();
            portalEffectInstance.Play();
            
            //this sets up the keys that are needed and related to the player
            keys = new EntityManager();
            keyManager = new SystemManager();

            ISystem newSystem;
            newSystem = new SystemRender();
            keyManager.AddSystem(newSystem);

            Entity newEntity = new Entity("Room1Key");
            ComponentTransform transform = new ComponentTransform(new Vector3(20.0f, 3.0f, -20.0f), new Vector3(0.0f, 0.0f, (float)(Math.PI / 2.0f)), new Vector3(0.005f, 0.005f, 0.005f), new Vector3(0.0f, 0.0f, 0.0f));
            transform.setBoundingBoxCube(new Vector3(-0.5f, -2.0f, -0.5f), new Vector3(0.5f, 2.0f, 0.5f));
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentModel("Old_Key", key));
            newEntity.AddComponent(new ComponentTexture("Textures/key.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            keys.AddEntity(newEntity);

            newEntity = new Entity("Room2Key");
            transform = new ComponentTransform(new Vector3(20.0f, 3.0f, 20.0f), new Vector3(0.0f, 0.0f, (float)(Math.PI / 2.0f)), new Vector3(0.005f, 0.005f, 0.005f), new Vector3(0.0f, 0.0f, 0.0f));
            transform.setBoundingBoxCube(new Vector3(-0.5f, -2.0f, -0.5f), new Vector3(0.5f, 2.0f, 0.5f));
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentModel("Old_Key", key));
            newEntity.AddComponent(new ComponentTexture("Textures/key.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            keys.AddEntity(newEntity);

            newEntity = new Entity("Room3Key");
            transform = new ComponentTransform(new Vector3(-20.0f, 3.0f, 20.0f), new Vector3(0.0f, 0.0f, (float)(Math.PI / 2.0f)), new Vector3(0.005f, 0.005f, 0.005f), new Vector3(0.0f, 0.0f, 0.0f));
            transform.setBoundingBoxCube(new Vector3(-0.5f, -2.0f, -0.5f), new Vector3(0.5f, 2.0f, 0.5f));
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentModel("Old_Key", key));
            newEntity.AddComponent(new ComponentTexture("Textures/key.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            keys.AddEntity(newEntity);
        }

        public void PortalSoundUpdate()
        {
            // this does the 3d sound update for the portal
            listener.Position = MyGame.cameraPosition;
            Vector3 forward = new Vector3();
            float rot = MyGame.cameraRotation.X - (float)Math.PI / 2;
            forward.X = (float)Math.Cos(rot);
            forward.Z = (float)Math.Sin(rot);
            listener.Forward = forward;
            portalEffectInstance.Apply3D(listener, portalEmitter);
        }

        public ComponentPlayer(Entity[] inKeys)
        {
            keyArray = inKeys;
        }

        public void TakeHit()
        {
            // this method is used when the player takes a hit and will call the GameExit method if the players health goes to zero
            takeDamage = true;
            health = health - 1;
            //bullet.destroy();
            if (health <= 0)
            {
                portalEffectInstance.Stop();
                MyGame.GameExit();
            }
        }

        public void GetKey(Entity key)
        {
            // this method is used when a player picks a key up it adds to the player keycount then deletes the key, plays the key pickup sound and if the player now has 3 keys will swap the portal sound and play it
            KeyPickUP = true;
            keycount = keycount + 1;
            removedKey = key;
            keyEmitter.Position = MyGame.cameraPosition;
            listener.Position = MyGame.cameraPosition;
            keyEffectInstance.Apply3D(listener, keyEmitter);
            keyEffectInstance.Play();
            keys.removeEntity(key);
            if (keycount == 3)
            {
                portalEffectInstance.Stop();
                portal = true;
                portalEffect = MyGame.portalopenSound;
                portalEffectInstance = portalEffect.CreateInstance();
                portalEffectInstance.IsLooped = true;
                portalEmitter = new AudioEmitter();
                portalEmitter.Position = new Vector3(-27.4f, 0.0f, -20.0f);
                listener.Position = MyGame.cameraPosition;
                portalEffectInstance.Apply3D(listener, portalEmitter);
                portalEffectInstance.Play();
            }
        }

        public void EnterPortal()
        {
            // handle for if the player goes to portal before haing 3 keys and to stop sound if they do
            if (portal)
            {
                portalEffectInstance.Stop();
                
                MyGame.GameExit();
            }
        }

        public void DrawKeys()
        {
            //draw method for keys
            keyManager.ActionSystems(keys);
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_PLAYER; }
        }
    }
}
