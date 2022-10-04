using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System;

namespace OpenGL_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MyGame : Game
    {
        public GraphicsDeviceManager graphics;
        public Matrix view, projection;
        EntityManager entityManager;
        List<Entity> walls;
        SystemManager systemManager;
        public SpriteBatch spriteBatch;
        public static float dt;
        Entity[] hearts = new Entity[5];
        Entity[] UIkey = new Entity[3];
        public static Vector3 cameraPosition, cameraRotation;
        float cameraMovementSpeed = 4.0f;
        private MouseState previousMouseState;
        public static bool gameOver = false;
        public static Vector2 AIpos;
        public static float AIrot;
        public static Vector3 spotLightPos1 = new Vector3(-24.0f, 4.9f, 16.0f);
        public static Vector3 spotLightPos2 = new Vector3(-16.0f, 4.9f, 24.0f);
        float moveSpotLight1 = 0.02f;
        float moveSpotLight2 = 0.02f;
        public static Vector2 MmCubePos;

        private Texture2D gameOverTexture;

        Microsoft.Xna.Framework.Graphics.Model key;
        public static SoundEffect bulletSound;
        public static SoundEffect keySound;
        public static SoundEffect portalopenSound;
        public static SoundEffect portalclosedSound;


        public static MyGame gameInstance;

        public MyGame() : base()
        {
            gameInstance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            IsFixedTimeStep = false;
            walls = new List<Entity>();
            previousMouseState = Mouse.GetState();
        }

        private void CreateEntities()
        {
            Entity newEntity;

            #region UI
            int width = 110;
            int height = 110;
            newEntity = new Entity("Minimap");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/MiniMap.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(width/2 , height/2, width, height)));
            entityManager.AddEntity(newEntity);

            width = 10;
            height = 5;
            newEntity = new Entity("MmKey1");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/MMkey.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(95, 15, width, height)));
            entityManager.AddEntity(newEntity);
            UIkey[0] = newEntity;

            width = 5;
            height = 5;
            newEntity = new Entity("MmOctahedron");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/skybox_top3.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(95, 95, width, height)));
            entityManager.AddEntity(newEntity);

            width = 10;
            height = 5;
            newEntity = new Entity("MmKey2");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/MMkey.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(95, 95, width, height)));
            entityManager.AddEntity(newEntity);
            UIkey[1] = newEntity;

            width = 10;
            height = 5;
            newEntity = new Entity("MmKey3");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/MMkey.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(15, 95, width, height)));
            entityManager.AddEntity(newEntity);
            UIkey[2] = newEntity;

            width = 5;
            height = 5;
            newEntity = new Entity("MmRollingCube");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 1, 0), new Vector3(0, 0, 0), new Vector3(78, 35, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/skybox_top3.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(98, 15, width, height)));
            entityManager.AddEntity(newEntity);

            width = 4;
            height = 15;
            newEntity = new Entity("MmPortal");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/skybox_top3.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(2, 15, width, height)));
            entityManager.AddEntity(newEntity);

            width = 50;
            height = 50;
            newEntity = new Entity("Health1");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/healthNew.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(((width / 2) + gameInstance.GraphicsDevice.Viewport.Width) - width, ((gameInstance.GraphicsDevice.Viewport.Height / 7) * 7) - (height / 2), width, height)));
            entityManager.AddEntity(newEntity);
            hearts[0] = newEntity;

            newEntity = new Entity("Health2");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/healthNew.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(((width / 2) + gameInstance.GraphicsDevice.Viewport.Width) - width, ((gameInstance.GraphicsDevice.Viewport.Height / 7) * 6) - (height / 2), width, height)));
            entityManager.AddEntity(newEntity);
            hearts[1] = newEntity;

            newEntity = new Entity("Health3");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/healthNew.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(((width / 2) + gameInstance.GraphicsDevice.Viewport.Width) - width, ((gameInstance.GraphicsDevice.Viewport.Height / 7) * 5) - (height / 2), width, height)));
            entityManager.AddEntity(newEntity);
            hearts[2] = newEntity;

            newEntity = new Entity("Health4");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/healthNew.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(((width / 2) + gameInstance.GraphicsDevice.Viewport.Width) - width, ((gameInstance.GraphicsDevice.Viewport.Height / 7) * 4) - (height / 2), width, height)));
            entityManager.AddEntity(newEntity);
            hearts[3] = newEntity;

            newEntity = new Entity("Health5");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/healthNew.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(((width / 2) + gameInstance.GraphicsDevice.Viewport.Width) - width, ((gameInstance.GraphicsDevice.Viewport.Height / 7) * 3) - (height / 2), width, height)));
            entityManager.AddEntity(newEntity);
            hearts[4] = newEntity;


            width = 10;
            height = 10;
            newEntity = new Entity("PlayerDot");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(width / 2, height / 2, width, height)));
            newEntity.AddComponent(new ComponentUIvelocity(new Vector2(0, 0)));
            entityManager.AddEntity(newEntity);


            newEntity = new Entity("AIDot");
            newEntity.AddComponent(new ComponentTransform(new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
            newEntity.AddComponent(new ComponentTexture("../Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentRectangle(new Rectangle(width / 2, height / 2, width, height)));
            newEntity.AddComponent(new ComponentUIvelocity(new Vector2(0, 0)));
            entityManager.AddEntity(newEntity);

            #endregion 

            #region keys
            /*
            newEntity = new Entity("Room1Key");
            ComponentTransform transform = new ComponentTransform(new Vector3(20.0f, 3.0f, -20.0f), new Vector3(0.0f, 0.0f, (float)(Math.PI / 2.0f)), new Vector3(0.005f, 0.005f, 0.005f), new Vector3(0.0f, 0.0f, 0.0f));
            transform.setBoundingBoxCube(new Vector3(-0.5f, -2.0f, -0.5f), new Vector3(0.5f, 2.0f, 0.5f));
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentModel("Old_Key", key));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Room2Key");
            transform = new ComponentTransform(new Vector3(20.0f, 3.0f, 20.0f), new Vector3(0.0f, 0.0f, (float)(Math.PI / 2.0f)), new Vector3(0.005f, 0.005f, 0.005f), new Vector3(0.0f, 0.0f, 0.0f));
            transform.setBoundingBoxCube(new Vector3(-0.5f, -2.0f, -0.5f), new Vector3(0.5f, 2.0f, 0.5f));
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentModel("Old_Key", key));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Room3Key");
            transform = new ComponentTransform(new Vector3(-20.0f, 3.0f, 20.0f), new Vector3(0.0f, 0.0f, (float)(Math.PI / 2.0f)), new Vector3(0.005f, 0.005f, 0.005f), new Vector3(0.0f, 0.0f, 0.0f));
            transform.setBoundingBoxCube(new Vector3(-0.5f, -2.0f, -0.5f), new Vector3(0.5f, 2.0f, 0.5f));
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentModel("Old_Key", key));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
             * */
            #endregion

            newEntity = new Entity("Portal");
            ComponentTransform transform = new ComponentTransform(new Vector3(-27.4f, 0.0f, -20.0f), new Vector3((float)(Math.PI / 2), 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/mportal.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            #region shapes
            newEntity = new Entity("RollingSquare");
            transform = new ComponentTransform(new Vector3(20.0f, 0.5f, -20.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 0.0f));
            transform.setBoundingBoxCube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f));
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/CubeGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/plastic.png"));
            newEntity.AddComponent(new ComponentVelocity(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0, 0.1f, 0)));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("BouncingOctahedron");
            transform = new ComponentTransform(new Vector3(20.0f, 2.5f, 20.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 0.0f));
            transform.setBoundingBoxCube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f));
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/OctahedronGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/metal1.png"));
            newEntity.AddComponent(new ComponentVelocity(new Vector3(0.0f, 0.0f, 0.0f)));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            #endregion

            #region FloorPlan
            newEntity = new Entity("Floor");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f,0.0f,0.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/FloorGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/planks.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            #endregion

            #region Wallplan

            #region MiddleRoom
            newEntity = new Entity("MiddleRoomTopRightCornerWallDownFacing");
            transform = new ComponentTransform(new Vector3(5.0f, 0.0f, -7.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("MiddleRoomTopRightCornerWallLeftFacing");
            transform = new ComponentTransform(new Vector3(7.5f, 0.0f, -5.0f), new Vector3((float)(Math.PI / 2), 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("MiddleRoomTopLeftCornerWallDownFacing");
            transform = new ComponentTransform(new Vector3(-5.0f, 0.0f, -7.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);
            
            newEntity = new Entity("MiddleRoomTopLeftCornerWallRightFacing");
            transform = new ComponentTransform(new Vector3(-7.5f, 0.0f, -5.0f), new Vector3((float)(Math.PI / 2), 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("MiddleRoomBottomRightCornerWallUpFacing");
            transform = new ComponentTransform(new Vector3(5.0f, 0.0f, 7.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);
      
            newEntity = new Entity("MiddleRoomBottomRightCornerWallLeftFacing");
            transform = new ComponentTransform(new Vector3(7.5f, 0.0f, 5.0f), new Vector3((float)(Math.PI / 2), 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);
            
            newEntity = new Entity("MiddleRoomBottomLeftCornerWallUpFacing");
            transform = new ComponentTransform(new Vector3(-5.0f, 0.0f, 7.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("MiddleRoomBottomLeftCornerWallRightFacing");
            transform = new ComponentTransform(new Vector3(-7.5f, 0.0f, 5.0f), new Vector3((float)(Math.PI / 2), 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);
            #endregion

            #region TopTRoom 
            newEntity = new Entity("TopTRoomTopWallDownFacing");
            transform = new ComponentTransform(new Vector3(0.0f, 0.0f, -22.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(5.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("TopTRoomMiddleRightWallUpFacing");
            transform = new ComponentTransform(new Vector3(7.5f, 0.0f, -17.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);
            
            newEntity = new Entity("TopTRoomMiddleLeftWallUpFacing");
            transform = new ComponentTransform(new Vector3(-7.5f, 0.0f, -17.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("TopTRoomBottomRightWallLeftFacing");
            transform = new ComponentTransform(new Vector3(2.5f, 0.0f, -12.5f), new Vector3((float)(Math.PI / 2), 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("TopTRoomBottomLeftWallRightFacing");
            transform = new ComponentTransform(new Vector3(-2.5f, 0.0f, -12.5f), new Vector3((float)(Math.PI / 2), 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);
            #endregion

            #region BottomTRoom
            newEntity = new Entity("BottomTRoomTopLeftWallRightFacing");
            transform = new ComponentTransform(new Vector3(-2.5f, 0.0f, 12.5f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("BottomTRoomTopRightWallLeftFacing");
            transform = new ComponentTransform(new Vector3(2.5f, 0.0f, 12.5f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("BottomTRoomMiddleRightWallDownFacing");
            transform = new ComponentTransform(new Vector3(7.5f, 0.0f, 17.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("BottomTRoomMiddleLeftWallDownFacing");
            transform = new ComponentTransform(new Vector3(-7.5f, 0.0f, 17.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("BottomTRoomBottomWallUpFacing");
            transform = new ComponentTransform(new Vector3(0.0f, 0.0f, 22.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(5.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);
            #endregion

            #region RightTRoom
            newEntity = new Entity("RightTRoomBottomInsideWallUpFacing");
            transform = new ComponentTransform(new Vector3(12.5f, 0.0f, 2.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("RightTRoomTopInsideWallDownFacing");
            transform = new ComponentTransform(new Vector3(12.5f, 0.0f, -2.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("RightTRoomMiddleTopWallRightFacing");
            transform = new ComponentTransform(new Vector3(17.5f, 0.0f, -7.5f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("RightTRoomMiddleBottomWallRightFacing");
            transform = new ComponentTransform(new Vector3(17.5f, 0.0f, 7.5f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("RightTRoomOutsideWallLeftFacing");
            transform = new ComponentTransform(new Vector3(22.5f, 0.0f, 0.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(5.0f, 1.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);
            #endregion

            #region LeftTRoom
            newEntity = new Entity("LeftTRoomBottomInsideWallUpFacing");
            transform = new ComponentTransform(new Vector3(-12.5f, 0.0f, 2.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("LeftTRoomTopInsideWallDownFacing");
            transform = new ComponentTransform(new Vector3(-12.5f, 0.0f, -2.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("LeftTRoomMiddleTopWallLeftFacing");
            transform = new ComponentTransform(new Vector3(-17.5f, 0.0f, -7.5f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("LeftTRoomMiddleBottomWallLeftFacing");
            transform = new ComponentTransform(new Vector3(-17.5f, 0.0f, 7.5f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("LeftTRoomOutsideWallRightFacing");
            transform = new ComponentTransform(new Vector3(-22.5f, 0.0f, 0.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(5.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);
            #endregion

            #region Room1
            //bottom left corner
            newEntity = new Entity("Room1LeftBottomWallRightFacing");
            transform = new ComponentTransform(new Vector3(12.5f, 0.0f, -15.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room1LeftTopWallRightFacing");
            transform = new ComponentTransform(new Vector3(12.5f, 0.0f, -25.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room1BottomLeftWallUpFacing");
            transform = new ComponentTransform(new Vector3(15.0f, 0.0f, -12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room1BottomRightWallUpFacing");
            transform = new ComponentTransform(new Vector3(25.0f, 0.0f, -12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room1RightWallLeftFacing");
            transform = new ComponentTransform(new Vector3(27.5f, 0.0f, -20.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room1TopWallDownFacing");
            transform = new ComponentTransform(new Vector3(20.0f, 0.0f, -27.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);
            #endregion

            #region Room2
            //top left corner
            newEntity = new Entity("Room2LeftTopWallRightFacing");
            transform = new ComponentTransform(new Vector3(12.5f, 0.0f, 15.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room2LeftBottomWallRightFacing");
            transform = new ComponentTransform(new Vector3(12.5f, 0.0f, 25.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room2TopLeftWallDownFacing");
            transform = new ComponentTransform(new Vector3(15.0f, 0.0f, 12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room2TopRightWallDownFacing");
            transform = new ComponentTransform(new Vector3(25.0f, 0.0f, 12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room2RightWallLeftFacing");
            transform = new ComponentTransform(new Vector3(27.5f, 0.0f, 20.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room2BottomWallUpFacing");
            transform = new ComponentTransform(new Vector3(20.0f, 0.0f, 27.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);
            #endregion

            #region Room3
            //top right corner
            newEntity = new Entity("Room3RightTopWallLeftFacing");
            transform = new ComponentTransform(new Vector3(-12.5f, 0.0f, 15.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room3RightBottomWallLeftFacing");
            transform = new ComponentTransform(new Vector3(-12.5f, 0.0f, 25.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room3TopRightWallDownFacing");
            transform = new ComponentTransform(new Vector3(-15.0f, 0.0f, 12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room3TopLeftWallDownFacing");
            transform = new ComponentTransform(new Vector3(-25.0f, 0.0f, 12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room3LeftWallRightFacing");
            transform = new ComponentTransform(new Vector3(-27.5f, 0.0f, 20.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room3BottomWallUpFacing");
            transform = new ComponentTransform(new Vector3(-20.0f, 0.0f, 27.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);
            #endregion

            #region Room4
            //bottom right corner
            newEntity = new Entity("Room4RightBottomWallLeftFacing");
            transform = new ComponentTransform(new Vector3(-12.5f, 0.0f, -15.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room4RightTopWallLeftFacing");
            transform = new ComponentTransform(new Vector3(-12.5f, 0.0f, -25.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room4BottomRightWallUpFacing");
            transform = new ComponentTransform(new Vector3(-15.0f, 0.0f, -12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room4BottomLeftWallUpFacing");
            transform = new ComponentTransform(new Vector3(-25.0f, 0.0f, -12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room4LeftWallRightFacing");
            transform = new ComponentTransform(new Vector3(-27.5f, 0.0f, -20.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);

            newEntity = new Entity("Room4TopWallDownFacing");
            transform = new ComponentTransform(new Vector3(-20.0f, 0.0f, -27.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxWall();
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            walls.Add(newEntity);
            #endregion

            #endregion

            #region Creating Nodes
            List<Nodes> nodeList = new List<Nodes>();
            nodeList.Add(new Nodes(new Vector3(-20.0f, 0.0f, -20.0f)));
            nodeList.Add(new Nodes(new Vector3(0.0f, 0.0f, -20.0f)));
            nodeList.Add(new Nodes(new Vector3(20.0f, 0.0f, -20.0f)));
            nodeList.Add(new Nodes(new Vector3(-20.0f, 0.0f, 0.0f)));
            nodeList.Add(new Nodes(new Vector3(0.0f, 0.0f, 0.0f)));
            nodeList.Add(new Nodes(new Vector3(20.0f, 0.0f, 0.0f)));
            nodeList.Add(new Nodes(new Vector3(-20.0f, 0.0f, 20.0f)));
            nodeList.Add(new Nodes(new Vector3(0.0f, 0.0f, 20.0f)));
            nodeList.Add(new Nodes(new Vector3(20.0f, 0.0f, 20.0f)));
            #endregion
            #region Setting Node Connections
            nodeList[0].addConnection(nodeList[1]);
            nodeList[0].addConnection(nodeList[3]);
            nodeList[1].addConnection(nodeList[2]);
            nodeList[1].addConnection(nodeList[4]);
            nodeList[2].addConnection(nodeList[5]);
            nodeList[3].addConnection(nodeList[4]);
            nodeList[3].addConnection(nodeList[6]);
            nodeList[4].addConnection(nodeList[5]);
            nodeList[4].addConnection(nodeList[7]);
            nodeList[5].addConnection(nodeList[8]);
            nodeList[6].addConnection(nodeList[7]);
            nodeList[7].addConnection(nodeList[8]);
            #endregion
            newEntity = new Entity("AI");
            transform = new ComponentTransform(new Vector3(20.0f, 1.5f, 20.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 3.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f));
            transform.setBoundingBoxCube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f));
            newEntity.AddComponent(transform);
            newEntity.AddComponent(new ComponentGeometry("Geometry/CubeGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/robot.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            newEntity.AddComponent(new ComponentAI(nodeList, walls, 5.0f, 5.0f, 30.0f, 5.0f, 15.0f));
            entityManager.AddEntity(newEntity);

            //skybox
            newEntity = new Entity("Skybox");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(100, 100, 100),new Vector3(0.0f,0.0f,0.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/CubeGeometry.txt"));
            newEntity.AddComponent(new ComponentShader("../Textures/skybox.mgfx"));
            newEntity.AddComponent(new ComponentSkyBox(new string[] { "../Textures/skybox_back6.png", "../Textures/skybox_front5.png", "../Textures/skybox_top3.png", "../Textures/skybox_bottom4.png", "../Textures/skybox_right1.png", "../Textures/skybox_left2.png" }));//used diagram to figureout order
            entityManager.AddEntity(newEntity);

            //room3 second floor
            newEntity = new Entity("R3f");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-20.0f, 0.01f, 27.5f), new Vector3(0.0f, -(float)Math.PI / 2, 0.0f), new Vector3(3f, 3f, 3f),new Vector3(0.0f,0.0f,0.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/planks.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Player");
            newEntity.AddComponent(new ComponentTransform(cameraPosition, cameraRotation, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f)));
            newEntity.AddComponent(new ComponentPlayer(entityManager.Entities(), key));
            entityManager.AddEntity(newEntity);
        }

        private void Camera(Vector3 cameraPosition, Vector3 rotation)
        {
            if (cameraRotation.X > Math.PI)
                cameraRotation.X -= (float)(Math.PI * 2);
            else if (cameraRotation.X < -Math.PI)
                cameraRotation.X += (float)(Math.PI * 2);

            view = Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, -cameraPosition.Z) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);
        }

        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemPhysics();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemAI();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemPlayer();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemSkyboxRender();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemUI();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemRenderTarget();
            systemManager.AddSystem(newSystem);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
            float nearClipPlane = 0.1f;
            float farClipPlane = 200000;
            projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

            base.Initialize();

            NewGame();
            CreateSystems();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameOverTexture = Content.Load<Texture2D>("../Textures/gameover.png");

            key = Content.Load<Microsoft.Xna.Framework.Graphics.Model>("models/Old_Key");
            keySound = Content.Load<SoundEffect>("Audio/keysound.wav");
            bulletSound = Content.Load<SoundEffect>("Audio/lasersound.wav");
            portalopenSound = Content.Load<SoundEffect>("Audio/portalopen.wav");
            portalclosedSound = Content.Load<SoundEffect>("Audio/portalclosed.wav");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (mouse.Position.X != previousMouseState.Position.X)
            {
                cameraRotation += new Vector3(((mouse.Position.X - previousMouseState.Position.X) * dt) / 3.0f, 0.0f, 0.0f);
                Camera(cameraPosition, cameraRotation);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Vector3 direction = new Vector3(0.0f, 0.0f, -cameraMovementSpeed);
                Matrix rotMatrix = Matrix.CreateFromYawPitchRoll(cameraRotation.X, cameraRotation.Y, cameraRotation.Z);
                Matrix dirMatrix = Matrix.CreateTranslation(direction);
                direction = (dirMatrix * Matrix.Invert(rotMatrix)).Translation;
                Vector3 previousPosition = cameraPosition;
                cameraPosition += direction * dt * 2;

                //Console.WriteLine(dt);
                Camera(cameraPosition, cameraRotation);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Vector3 direction = new Vector3(0.0f, 0.0f, cameraMovementSpeed);
                Matrix rotMatrix = Matrix.CreateFromYawPitchRoll(cameraRotation.X, cameraRotation.Y, cameraRotation.Z);
                Matrix dirMatrix = Matrix.CreateTranslation(direction);
                direction = (dirMatrix * Matrix.Invert(rotMatrix)).Translation;
                Vector3 previousPosition = cameraPosition;
                cameraPosition += direction * dt * 2;
                //Console.WriteLine(dt);

                Camera(cameraPosition, cameraRotation);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
            {
                cameraPosition += Vector3.Down;
                Camera(cameraPosition, cameraRotation);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
            {
                cameraPosition += Vector3.Up;
                Camera(cameraPosition, cameraRotation);
            }

            KeyPickUps();

            spotLightPos1.X += moveSpotLight1;
            if (spotLightPos1.X < -24)
            {
                moveSpotLight1 = 0.02f;
            }
            if (spotLightPos1.X > -16)
            {
                moveSpotLight1 = -0.02f;
            }
            spotLightPos2.Z += moveSpotLight2;
            if (spotLightPos2.Z > 24)
            {
                moveSpotLight2 = -0.02f;
            }
            if (spotLightPos2.Z < 16)
            {
                moveSpotLight2 = 0.02f;
            }

            base.Update(gameTime);



            //Console.WriteLine(dt);

            Mouse.SetPosition(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

            previousMouseState = Mouse.GetState();
        }

        private void KeyPickUps()
        {
            foreach (Entity e in entityManager.Entities())
            {
                if (e.Name == "RollingSquare")
                {
                    List<IComponent> components = e.Components;

                    IComponent positionComponent = components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                    });
                    ComponentTransform cubePos = (ComponentTransform)positionComponent;
                    MmCubePos = new Vector2(cubePos.Position.X, cubePos.Position.Z);
                }
                if(e.Name == "AI")
                {
                    List<IComponent> components = e.Components;

                    IComponent positionComponent = components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                    });
                    ComponentTransform AI = (ComponentTransform)positionComponent;
                    AIpos = new Vector2(AI.Position.X,AI.Position.Z);
                    AIrot = -AI.Rotation.X - (float)Math.PI/2;
                }
                if (e.Name == "Player")
                {
                    List<IComponent> components = e.Components;

                    IComponent positionComponent = components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_PLAYER;
                    });
                    ComponentPlayer player = (ComponentPlayer)positionComponent;
                    if (player.KeyPickUP)
                    {
                        Entity newEntity;
                        int width = 80;
                        int height = 40;
                        if (player.keycount == 1)
                        {
                            newEntity = new Entity("UIkey1");
                            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
                            newEntity.AddComponent(new ComponentTexture("../Textures/MMkey.png"));
                            newEntity.AddComponent(new ComponentRectangle(new Rectangle((width / 2), ((gameInstance.GraphicsDevice.Viewport.Height / 10) * 10) - (height / 2), width, height)));
                            entityManager.AddEntity(newEntity);
                        }
                        if (player.keycount == 2)
                        {

                            newEntity = new Entity("UIkey2");
                            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
                            newEntity.AddComponent(new ComponentTexture("../Textures/MMkey.png"));
                            newEntity.AddComponent(new ComponentRectangle(new Rectangle((width / 2), ((gameInstance.GraphicsDevice.Viewport.Height / 10) * 9) - (height / 2), width, height)));
                            entityManager.AddEntity(newEntity);

                        }
                        if (player.keycount == 3)
                        {
                            newEntity = new Entity("UIkey3");
                            newEntity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0)));
                            newEntity.AddComponent(new ComponentTexture("../Textures/MMkey.png"));
                            newEntity.AddComponent(new ComponentRectangle(new Rectangle((width / 2), ((gameInstance.GraphicsDevice.Viewport.Height / 10) * 8) - (height / 2), width, height)));
                            entityManager.AddEntity(newEntity);
                        }
                        switch(player.removedKey.Name)
                        {
                            case "Room1Key":
                                entityManager.Entities().Remove(UIkey[0]);
                                break;
                            case "Room2Key":
                                entityManager.Entities().Remove(UIkey[1]);
                                break;                                
                            case "Room3Key":                          
                                entityManager.Entities().Remove(UIkey[2]);
                                break;
                        }
                        player.KeyPickUP = false;
                        break;
                    }

                    if(player.takeDamage)
                    {
                        entityManager.Entities().Remove(hearts[player.health]);
                        break;
                    }
                }
            }
        }
               
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (gameOver)
            {
                case false:
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    systemManager.ActionSystems(entityManager);

                    base.Draw(gameTime);
                    break;
                case true:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin();
                    spriteBatch.Draw(gameOverTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.End();
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        entityManager.resetEntities(); 
                        NewGame();
                    }
                    break;
            }
        }

        private void NewGame()
        {
            cameraPosition = new Vector3(-20.0f, 2.0f, -20.0f);
            cameraRotation = new Vector3(0.0f, 0.0f, 0.0f);
            Camera(cameraPosition, cameraRotation);
            gameOver = false;
            CreateEntities();
        }

        public static void GameExit()
        {
            gameOver = true;
            Console.WriteLine("Exit");
        }
    }
}
