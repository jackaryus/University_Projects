// File Author: Daniel Masterson
using SpaceGame.Entities;
using SpaceGame.GameManagers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    public enum EGameType
    {
        OneVsOne = 2,
        FourPlayer = 4,
        EightPlayer = 8,
    }

    /// <summary>
    /// The core gameplay scene that sets up the game and manages gameover states
    /// </summary>
    public class GameScene : Scene
    {
        List<Node> nodes = new List<Node>();
        EGameType gameType;

        public float Time { get; private set; }
        bool gameOver = false;

        public GameScene(EGameType type)
        {
            gameType = type;
        }

        public override void OnSceneStart()
        {
            DataManager.BeginLogging();
            DataManager.Output("### Beginning Game Setup begins at " + DateTime.Now.ToString("dd-MM-yyy HH:mm:ss") + " ###");

            CameraManager.SetActiveCamera(new EdgeScrollDragCamera());

            Spawn(new Background());

            int nodeCount = SettingsManager.GetSetting<int>("game.nodeCount");
            int levelWidth = SettingsManager.GetSetting<int>("game.levelWidth");
            int levelHeight = SettingsManager.GetSetting<int>("game.levelHeight");
            Time = SettingsManager.GetSetting<float>("game.timeLimit");

            Bounds = new Rectangle(0, 0, levelWidth + 64, levelHeight + 64);

            float nodeDistanceThreshold = SettingsManager.GetSetting<float>("game.minNodeDist"); //Minimum distance between each node
            float nodeDistanceThresholdSq = nodeDistanceThreshold * nodeDistanceThreshold;

            float nodeLinkThreshold = SettingsManager.GetSetting<float>("game.maxNodeDist"); //Maximum distance between each node to form a link
            float nodeLinkThresholdSq = nodeLinkThreshold * nodeLinkThreshold;

            int testCount = 0; //Used to test random node placement
            int maxTestCount = 64; //Maximum number of times we can test each node's random placement

            Vector2 testA, testB;
            bool foundCollidingNode = false;
            for (int i = 0; i < nodeCount; ++i)
            {
                testCount = 0;
                while (++testCount < maxTestCount)
                {
                    testA = new Vector2(UtilityManager.Random.Next(0, levelWidth), UtilityManager.Random.Next(0, levelHeight));
                    foundCollidingNode = false;

                    for (int j = 0; j < nodes.Count; ++j)
                    {
                        testB = new Vector2(nodes[j].Bounds.Center.X, nodes[j].Bounds.Center.Y);

                        if ((testA - testB).LengthSquared() < nodeDistanceThresholdSq) //Too close?
                        {
                            foundCollidingNode = true;
                            break;
                        }
                    }

                    if (!foundCollidingNode) //Not too close to any node. Add!
                    {
                        nodes.Add((Node)Spawn(new Node(), testA, new Vector2(100.0f, 100.0f)));
                        break;
                    }
                }
            }

            //Add links
            for (int i = 0; i < nodes.Count; ++i)
            {
                testA = new Vector2(nodes[i].Bounds.Center.X, nodes[i].Bounds.Center.Y);
                for (int j = 0; j < nodes.Count; ++j)
                {
                    testB = new Vector2(nodes[j].Bounds.Center.X, nodes[j].Bounds.Center.Y);

                    if ((testA - testB).LengthSquared() < nodeLinkThresholdSq) //Node close enough?
                    {
                        nodes[i].ConnectTo(nodes[j]);
                    }
                }
            }

            //Clean up orphans
            //Fixme: In some rare occasions, this doesn't work. Don't really know why
            int removedOrphans = 0;
            for (int i = 0; i < nodes.Count; ++i)
            {
                if (nodes[i].GetConnectionCount() == 0)
                {
                    nodes.RemoveAt(i);
                    --i;
                    ++removedOrphans;
                }
            }
            DataManager.Output(removedOrphans + " orphan nodes removed");
            DataManager.Output(nodes.Count + " nodes in level");


            //Assign random start locations
            int shipsPerPlayer = SettingsManager.GetSetting<int>("game.shipsPerPlayer");
            for (int i = 0; i < PlayerManager.GetPlayerCount() && i < nodes.Count; ++i)
            {
                int nodeID;
                do
                {
                    nodeID = UtilityManager.Random.Next(0, nodes.Count); //Get a random node

                    if (nodes[nodeID].CurrentOwner == null) //This random node is not owned?
                    {
                        nodes[nodeID].SetOwningPlayer(PlayerManager.GetPlayerFromID(i));
                        nodes[nodeID].SetResourceType(EResourceType.Fuel); //So players always have a source of fuel

                        for (int n = 0; n < nodes[nodeID].GetConnectionCount(); ++n) //All connected nodes are fuel
                        {
                            nodes[nodeID].nodeConnections[n].other.SetResourceType(EResourceType.Fuel); //Fuel is pretty important
                        }

                        if (i == 0)
                        {
                            CameraManager.ActiveCamera.SetCameraBounds(new RectangleF(
                                nodes[nodeID].Bounds.Center.X - Main.Resolution.X,
                                nodes[nodeID].Bounds.Center.Y - Main.Resolution.Y,
                                Main.Resolution.X * 2, Main.Resolution.Y * 2));
                        }

                        // Add the player's ships
                        for (int s = 1; s <= shipsPerPlayer; ++s)
                        {
                            Ship ship = new Ship();
                            Spawn(ship).SetOwningPlayer(PlayerManager.GetPlayerFromID(i));
                            ship.SetTargetNode(nodes[nodeID], true);
                        }

                        break;
                    }
                } while (true);
            }

            // Wake up dummy players created in the customization step
            PlayerManager.EnablePlayers();

            DataManager.Output("### Player settings ###");
            for (int i = 0; i < PlayerManager.Players.Count; ++i)
            {
                DataManager.Output("[" + i + " - " + (PlayerManager.Players[i] is AIPlayer ? "AI" : "Human") + "] " + PlayerManager.Players[i].Team.Name + " - Avatar = '" + PlayerManager.Players[i].Team.Avatar + "', Ship = '" + PlayerManager.Players[i].Team.Ship + "', Color = '" + PlayerManager.Players[i].Team.Color.ToString() + "'"); 
            }

            DataManager.Output("### Game begins ###");
            //And the game really does begin!
        }

        public override void OnSceneUpdate(float delta)
        {
            if (gameOver) //So we don't spam game over screens on the player side
                return;

            Time -= delta;

            if (Time <= 0) //Game is complete though a timeout?
            {
                Player winner = PlayerManager.Players[0];

                for (int i = 1; i < PlayerManager.Players.Count; i++)
                {
                    if (PlayerManager.Players[i].GetScore() > winner.GetScore())
                        winner = PlayerManager.Players[i];
                }

                for (int i = 0; i < PlayerManager.Players.Count; i++)
                {
                    PlayerManager.Players[i].GameOver(PlayerManager.Players[i] == winner ? EGameOverState.Win_Score : EGameOverState.Lose_Score);          
                }
            }

            //Game is still live, so lets check on the state of all the players
            int lostPlayers = 0, wonPlayers = 0;
            for (int i = 0; i < PlayerManager.Players.Count; i++)
            {
                if (PlayerManager.Players[i].PlayerState == EPlayerState.Lost)
                    lostPlayers++;
                else if (PlayerManager.Players[i].PlayerState == EPlayerState.Won)
                    wonPlayers++;
            }

            if (lostPlayers == PlayerManager.Players.Count - 1) //All players have lost except one? (This can occur through killall)
            {
                for (int i = 0; i < PlayerManager.Players.Count; i++)
                {
                    if (PlayerManager.Players[i].PlayerState != EPlayerState.Lost)
                    {
                        PlayerManager.Players[i].GameOver(EGameOverState.Win_KillAll);
                        ++wonPlayers;
                    }
                }
            }
            else if (wonPlayers > 0) //Someone has won, even if not everyone else has reported a loss
            {
                for (int i = 0; i < PlayerManager.Players.Count; i++)
                {
                    if (PlayerManager.Players[i].PlayerState != EPlayerState.InGame)
                    {
                        PlayerManager.Players[i].GameOver(EGameOverState.Lose_Score); //A sort of generic win
                        ++lostPlayers;
                    }
                }
            }

            if (lostPlayers + wonPlayers >= PlayerManager.Players.Count)
            {
                DataManager.Output("### All players have reached a game over state ###");
                DataManager.Output("### Game ended at " + DateTime.Now.ToString("dd-MM-yyy HH:mm:ss") + " ###");
                DataManager.EndLogging();
                gameOver = true;
            }
        }
    }
}
