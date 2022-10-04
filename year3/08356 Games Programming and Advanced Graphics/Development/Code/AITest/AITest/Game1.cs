using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace AITest
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        AI aiPlayer;
        Sprite[] blockArray;
        Node[] nodeArray;

        Texture2D pixel;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 1000;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = Content.Load<Texture2D>("pixel");

            blockArray = new Sprite[]{
                new Sprite(new Rectangle(200,200,100,100), pixel), // Centre blocks
                new Sprite(new Rectangle(200, 400, 100, 100), pixel),
                new Sprite(new Rectangle(400, 400, 100, 100), pixel),
                new Sprite(new Rectangle(400, 200, 100, 100), pixel),
                new Sprite(new Rectangle(0,0, 600, 100), pixel), // Boundary blocks
                new Sprite(new Rectangle(0, 100, 100, 600), pixel),
                new Sprite(new Rectangle(600, 0, 100, 600), pixel),
                new Sprite(new Rectangle(100, 600, 600, 100), pixel)
            };

            nodeArray = new Node[]{
                new Node(new Vector2(150, 150)),
                new Node(new Vector2(350, 150)),
                new Node(new Vector2(550, 150)), // top3
                new Node(new Vector2(150, 350)),
                new Node(new Vector2(350, 350)),
                new Node(new Vector2(550, 350)), // middle 3
                new Node(new Vector2(150, 550)),
                new Node(new Vector2(350, 550)),
                new Node(new Vector2(550, 550)) // bottom 3
            };

            nodeArray[0].addConnection(nodeArray[1]);
            nodeArray[0].addConnection(nodeArray[3]);
            nodeArray[1].addConnection(nodeArray[2]);
            nodeArray[1].addConnection(nodeArray[4]);
            nodeArray[2].addConnection(nodeArray[5]);
            nodeArray[3].addConnection(nodeArray[4]);
            nodeArray[3].addConnection(nodeArray[6]);
            nodeArray[4].addConnection(nodeArray[5]);
            nodeArray[4].addConnection(nodeArray[7]);
            nodeArray[5].addConnection(nodeArray[8]);
            nodeArray[6].addConnection(nodeArray[7]);
            nodeArray[7].addConnection(nodeArray[8]);

            player = new Player(new Rectangle(100, 100, 30, 30), pixel, 10, blockArray);
            aiPlayer = new AI(new Rectangle(500, 500, 30, 30), pixel, nodeArray, player, blockArray);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Sprite[] temp = new Sprite[0];
            player.Update();
            aiPlayer.Update(player);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            player.Draw(spriteBatch, Color.Green);
            aiPlayer.Draw(spriteBatch, Color.Red);

            for (int i = 0; i < blockArray.Length; i++)
                blockArray[i].Draw(spriteBatch, Color.Yellow);
            for (int i = 0; i < nodeArray.Length; i++ )
                spriteBatch.Draw(pixel,nodeArray[i].intersectionRectangle, Color.Beige);
                spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
