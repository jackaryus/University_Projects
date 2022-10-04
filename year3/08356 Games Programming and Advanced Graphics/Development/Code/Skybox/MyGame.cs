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
        SystemManager systemManager;
        SpriteBatch spriteBatch;
        public static float dt;
        AudioEmitter emitter;
        AudioListener listener;
        SoundEffect soundEffect;
        SoundEffectInstance soundEffectInstance;
        public Vector3 cameraPosition, cameraRotation;
        float cameraMovementSpeed = 4.0f;

        public static MyGame gameInstance;

        public MyGame() : base()
        {
            gameInstance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            entityManager = new EntityManager();
            systemManager = new SystemManager();

        }

        private void CreateEntities()
        {
            Entity newEntity;

            newEntity = new Entity("Rotesy");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-20.0f, 0.01f, 27.5f), new Vector3(0.0f, -(float)Math.PI/2, 0.0f), new Vector3(3f, 3f, 3f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/planks.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            //skybox
            newEntity = new Entity("Skybox");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(100, 100, 100)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/CubeGeometry.txt"));
            newEntity.AddComponent(new ComponentShader("../Textures/skybox.mgfx"));
            newEntity.AddComponent(new ComponentSkyBox(new string[] { "../Textures/skybox_back6.png", "../Textures/skybox_front5.png", "../Textures/skybox_top3.png", "../Textures/skybox_bottom4.png", "../Textures/skybox_right1.png", "../Textures/skybox_left2.png" }));
            entityManager.AddEntity(newEntity);

            #region FloorPlan
            newEntity = new Entity("MiddleRoom");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0.0f, 0.0f, 0.0f),new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/RoomGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/planks.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Room1");
            newEntity.AddComponent(new ComponentTransform(new Vector3(20.0f, 0.0f, -20.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/RoomGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/planks.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Room2");
            newEntity.AddComponent(new ComponentTransform(new Vector3(20.0f, 0.0f, 20.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/RoomGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/planks.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Room3");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-20.0f, 0.0f, 20.0f), new Vector3(0.0f, 0.0f , 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/RoomGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/planks.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Room3Roof");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-20.0f, 5.0f, 20.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/RoomGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/planks.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Room4");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-20.0f, 0.0f, -20.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/RoomGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/planks.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("BottomT");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0.0f, 0.0f, 17.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/TRoomGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/planks.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("RightT");
            newEntity.AddComponent(new ComponentTransform(new Vector3(17.5f, 0.0f, 0.0f), new Vector3(((float)Math.PI / 2), 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/TRoomGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/planks.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("LeftT");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-17.5f, 0.0f, 0.0f), new Vector3((-(float)Math.PI / 2), 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/TRoomGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/planks.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TopT");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0.0f, 0.0f, -17.5f), new Vector3(((float)Math.PI), 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/TRoomGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/planks.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            
            #endregion

            #region Wallplan
            //may need to fix rotations so all the faces are facing the right way
            #region MiddleRoom
            newEntity = new Entity("MiddleRoomTopRightCornerWall_1");
            newEntity.AddComponent(new ComponentTransform(new Vector3(5.0f, 0.0f, -7.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("MiddleRoomTopRightCornerWall_2");
            newEntity.AddComponent(new ComponentTransform(new Vector3(7.5f, 0.0f, -5.0f), new Vector3((float)(Math.PI / 2), 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("MiddleRoomTopLeftCornerWall_1");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-5.0f, 0.0f, -7.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("MiddleRoomTopLeftCornerWall_2");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-7.5f, 0.0f, -5.0f), new Vector3((float)(Math.PI / 2), 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("MiddleRoomBottomRightCornerWall_1");
            newEntity.AddComponent(new ComponentTransform(new Vector3(5.0f, 0.0f, 7.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("MiddleRoomBottomRightCornerWall_2");
            newEntity.AddComponent(new ComponentTransform(new Vector3(7.5f, 0.0f, 5.0f), new Vector3((float)(Math.PI / 2), 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("MiddleRoomBottomLeftCornerWall_1");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-5.0f, 0.0f, 7.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("MiddleRoomBottomLeftCornerWall_2");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-7.5f, 0.0f, 5.0f), new Vector3((float)(Math.PI / 2), 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            #endregion

            #region TopTRoom
            newEntity = new Entity("TopTRoomTopWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0.0f, 0.0f, -22.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(5.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TopTRoomMiddleRightWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(7.5f, 0.0f, -17.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TopTRoomMiddleLeftWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-7.5f, 0.0f, -17.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TopTRoomBottomRightWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(2.5f, 0.0f, -12.5f), new Vector3((float)(Math.PI / 2), 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TopTRoomBottomLeftWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-2.5f, 0.0f, -12.5f), new Vector3((float)(Math.PI / 2), 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);
            #endregion
            //pos rot sca
            #region Middle room connecting corridors
            newEntity = new Entity("MiddleRoomRightCorridorBottom");
            newEntity.AddComponent(new ComponentTransform(new Vector3(12.5f, 0.0f, 2.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("MiddleRoomRightCorridorTop");
            newEntity.AddComponent(new ComponentTransform(new Vector3(12.5f, 0.0f, -2.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("MiddleRoomLeftCorridorBottom");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-12.5f, 0.0f, 2.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("MiddleRoomLeftCorridorTop");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-12.5f, 0.0f, -2.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("MiddleRoomBottomCorridorLeft");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-2.5f, 0.0f, 12.5f), new Vector3((float)Math.PI/2, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("MiddleRoomBottomCorridorRight");
            newEntity.AddComponent(new ComponentTransform(new Vector3(2.5f, 0.0f, 12.5f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            #endregion

            #region Surrounding corridors

            newEntity = new Entity("LeftCorridorLeftWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-22.5f, 0.0f, 0.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(5.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("BottomCorridorBottomWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(0.0f, 0.0f, 22.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(5.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("RightCorridorRightWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(22.5f, 0.0f, 0.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(5.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("BottomCorridorTopRightWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(7.5f, 0.0f, 17.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("BottomCorridorTopLeftWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-7.5f, 0.0f, 17.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("RightCorridorLeftTopWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(17.5f, 0.0f, -7.5f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("RightCorridorLeftBottomWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(17.5f, 0.0f, 7.5f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("LeftCorridorRightTopWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-17.5f, 0.0f, -7.5f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("LeftCorridorRightBottomWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-17.5f, 0.0f, 7.5f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(2.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            //bottom right room top left corner

            newEntity = new Entity("left");
            newEntity.AddComponent(new ComponentTransform(new Vector3(12.5f, 0.0f, 15.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("top");
            newEntity.AddComponent(new ComponentTransform(new Vector3(15.0f, 0.0f, 12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            //bottom left room top right corner

            newEntity = new Entity("right");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-12.5f, 0.0f, 15.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("top");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-15.0f, 0.0f, 12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            //top left room bottom right corner
            newEntity = new Entity("right");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-12.5f, 0.0f, -15.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("bottom");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-15.0f, 0.0f, -12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            //top left room bottom right corner
            newEntity = new Entity("right");
            newEntity.AddComponent(new ComponentTransform(new Vector3(12.5f, 0.0f, -15.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("bottom");
            newEntity.AddComponent(new ComponentTransform(new Vector3(15.0f, 0.0f, -12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("BottomRightRoomRightWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(27.5f, 0.0f, 20.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TopRightRoomRightWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(27.5f, 0.0f, -20.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("BottomLeftRoomLeftWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-27.5f, 0.0f, 20.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TopLeftRoomLeftWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-27.5f, 0.0f, -20.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TopLeftRoomTopWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-20.0f, 0.0f, -27.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TopRightRoomTopWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(20.0f, 0.0f, -27.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("BottomLeftRoomBottomWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-20.0f, 0.0f, 27.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("BottomRightRoomBottomWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(20.0f, 0.0f, 27.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(3.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TopRightRoomBottomRightWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(25.0f, 0.0f, -12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("BottomRightRoomTopRightWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(25.0f, 0.0f, 12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TopLeftRoomBottomLeftWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-25.0f, 0.0f, -12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("BottomLeftRoomTopLeftWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-25.0f, 0.0f, 12.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TopRightRoomTopLeftWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(12.5f, 0.0f, -25.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TopLeftRoomTopRightWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-12.5f, 0.0f, -25.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("BottomRightRoomBottomLeftWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(12.5f, 0.0f, 25.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("BottomLeftRoomBottomRightWall");
            newEntity.AddComponent(new ComponentTransform(new Vector3(-12.5f, 0.0f, 25.0f), new Vector3((float)Math.PI / 2, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentGeometry("Geometry/5x5WallGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/spaceship.png"));
            newEntity.AddComponent(new ComponentShader("../Textures/Lighting.mgfx"));
            entityManager.AddEntity(newEntity);

            #endregion

            #endregion

        }

        private void Camera(Vector3 cameraPosition, Vector3 rotation)
        {
            //private void Camera(Vector3 cameraPosition, Vector3 cameraLookAtVector, Vector3 cameraUpVector)
            //var cameraPosition = new Vector3(0, 20, 20);
            //var cameraLookAtVector = Vector3.Zero;
            //var cameraUpVector = Vector3.UnitZ;

            //view = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);
            view = Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, -cameraPosition.Z) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);
            float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 300;
            projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
        }

        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemPhysics();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemSkyboxRender();
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
            cameraPosition = new Vector3(0.0f, 2.0f, 0.0f);
            cameraRotation = new Vector3(0.0f, 0.0f, 0.0f);
            //Camera(cameraPosition, Vector3.Zero, Vector3.UnitZ);
            Camera(cameraPosition, cameraRotation);
            //new camera code
            /*
            var cameraPosition = new Vector3(0, 20, 20);
            var cameraLookAtVector = Vector3.Zero;
            var cameraUpVector = Vector3.UnitZ;

            view = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);
            float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 200;
            projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
             */

            //old
            //view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            //projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);

            CreateEntities();
            CreateSystems();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //soundEffect = Content.Load<SoundEffect>("Audio/buzz.wav");
            //soundEffectInstance = soundEffect.CreateInstance();
            //soundEffectInstance.IsLooped = true;

            //emitter = new AudioEmitter();
            //listener = new AudioListener();

            //emitter.Position = new Vector3(10.0f, 0.0f, 0.0f);
            //soundEffectInstance.Apply3D(listener, emitter);
            //soundEffectInstance.Play();
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
            dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //*
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                cameraRotation += new Vector3(-0.04f, 0.0f, 0.0f);
                Camera(cameraPosition, cameraRotation);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                cameraRotation += new Vector3(0.04f, 0.0f, 0.0f);
                Camera(cameraPosition, cameraRotation);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                //Vector3 direction = new Vector3((float)Math.Cos(MathHelper.ToRadians(cameraRotation.X)), 0.0f, (float)Math.Sin(MathHelper.ToRadians(cameraRotation.X)));
                Vector3 direction = new Vector3(0.0f, 0.0f, -cameraMovementSpeed);
                //direction.Normalize();
                Matrix rotMatrix = Matrix.CreateFromYawPitchRoll(cameraRotation.X, cameraRotation.Y, cameraRotation.Z);
                Matrix dirMatrix = Matrix.CreateTranslation(direction);
                direction = (dirMatrix * Matrix.Invert(rotMatrix)).Translation;
                cameraPosition += direction * dt * 2;
                //Console.WriteLine(dt);
                Camera(cameraPosition, cameraRotation);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                //Vector3 direction = new Vector3((float)Math.Cos(MathHelper.ToRadians(cameraRotation.X)), 0.0f, (float)Math.Sin(MathHelper.ToRadians(cameraRotation.X)));
                Vector3 direction = new Vector3(0.0f, 0.0f, cameraMovementSpeed);
                //direction.Normalize();
                Matrix rotMatrix = Matrix.CreateFromYawPitchRoll(cameraRotation.X, cameraRotation.Y, cameraRotation.Z);
                Matrix dirMatrix = Matrix.CreateTranslation(direction);
                direction = (dirMatrix * Matrix.Invert(rotMatrix)).Translation;
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

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
            {
                cameraRotation = new Vector3(0.0f, (float)Math.PI / 2, 0.0f);
                Camera(cameraPosition, cameraRotation);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad9))
            {
                cameraRotation = new Vector3(0.0f, 0.0f, 0.0f);
                Camera(cameraPosition, cameraRotation);
            }
            //*/
            // TODO: Add your update logic here
            //emitter.Position = new Vector3(emitter.Position.X - 0.025f,
            //                               emitter.Position.Y,
            //                               emitter.Position.Z);
            //soundEffectInstance.Apply3D(listener, emitter);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            systemManager.ActionSystems(entityManager);

            base.Draw(gameTime);
        }

    }
}
