// File Author: Daniel Masterson
using SpaceGame.Components;
using SpaceGame.Entities;
using SpaceGame.GameManagers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Entities
{
    public enum EPlayerState
    {
        InGame,
        Spectating,
        Lost,
        Won
    }

    public enum EGameOverState
    {
        Win_Score,
        Win_KillAll,
        Lose_Score,
        Lose_LostShips,
        Lose_LostPlanets
    }

    /// <summary>
    /// Any type of player. This class acts as a local player, but subclasses can remove the local player features
    /// </summary>
    public class Player : Entity
    {
        static Dictionary<EGameOverState, string> gameOverStrings = new Dictionary<EGameOverState, string>();

        public TeamInfo Team { get; private set; }
        protected List<Node> OwnedNodes = new List<Node>();

        public float CaptureRate { get; private set; }
        public float DefenseValue { get; private set; }
        public float UpgradeRate { get; private set; }

        public EPlayerState PlayerState { get; private set; }

        public Entity SelectedObject { get; private set; }
        protected List<Ship> ownedShips = new List<Ship>();
        protected Dictionary<EResourceType, float> resources = new Dictionary<EResourceType, float>();

        const float HUDOpacity = 0.8f;

        SolidColorComponent Resources_SolidColComp;
        LabelComponent Resources_LabelComp,
            TimeLeft_LabelComp,
            Notification_LabelComp;

        const float sidebarWidth = 256.0f;
        SolidColorComponent Sidebar_SolidColComp;
        AvatarComponent Sidebar_AvatarComp;
        LabelComponent
            Sidebar_PlayerName_LabelComp,
            Sidebar_Score_LabelComp,
            Sidebar_ShipCount_LabelComp,
            Sidebar_CurrentlySelected_LabelComp,
            Sidebar_CurrentlyOrbiting_LabelComp,
            Sidebar_ShipHP_LabelComp,
            Sidebar_ShipFuel_LabelComp,
            Sidebar_PlanetResourceType_LabelComp,
            Sidebar_PlanetResourceLevels_LabelComp,
            Sidebar_PlanetOwner_Label_LabelComp,
            Sidebar_PlanetOwner_Name_LabelComp,
            Sidebar_PlanetOwner_Opinion_LabelComp;
        ProgressBarComponent
            Sidebar_ShipHP_ProgressComp,
            Sidebar_ShipFuel_ProgressComp,            
            Sidebar_PlanetResourceLevels_ProgressComp;
        ButtonComponent
            Sidebar_CaptureNode_ButtonComp,
            Sidebar_AttackPlayer_ButtonComp;
        AvatarComponent
            Sidebar_PlanetOwner_AvatarComp;

        SolidColorComponent GameOver_SolidColComp;
        LabelComponent GameOver_LabelComp, GameOverSub_LabelComp;
        ButtonComponent MainMenu_ButtonComp;

        ButtonComponent EmoteHappy_ButtonComp, EmoteUnhappy_ButtonComp, EmoteAngry_ButtonComp,
            EmoteQuestion_ButtonComp, EmoteExclamation_ButtonComp, EmoteEllipses_ButtonComp;

        /// <summary>
        /// Creates a new player
        /// </summary>
        /// <param name="team">Player team information</param>
        public Player(TeamInfo team)
        {
            Team = team;
            PlayerState = EPlayerState.InGame;

            if(gameOverStrings.Count == 0)
            {
                gameOverStrings.Add(EGameOverState.Win_Score, "You had the highest score when time ran out!");
                gameOverStrings.Add(EGameOverState.Win_KillAll, "You destroyed all of the other factions!");

                gameOverStrings.Add(EGameOverState.Lose_Score, "You didn't have the highest score when time ran out");
                gameOverStrings.Add(EGameOverState.Lose_LostShips, "All your ships were destroyed");
                gameOverStrings.Add(EGameOverState.Lose_LostPlanets, "All your planets were captured");
            }
        }

        protected override void OnSpawn()
        {
            RegisterComponent(Resources_SolidColComp = new SolidColorComponent(Team.Color))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(200)
                .SetAlpha(HUDOpacity);

            RegisterComponent(Resources_LabelComp = new LabelComponent("Resources: NONE", "generic", Color.White, 1.0f, ETextHorizontalAlign.Left))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(201)
                .SetAlpha(HUDOpacity)
                .SetPosition(new Vector2(4.0f, 4.0f));

            RegisterComponent(TimeLeft_LabelComp = new LabelComponent("00:00", "generic", Color.White, 2.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(301);

            RegisterComponent(Notification_LabelComp = new LabelComponent("*NOTIFICATION*", "generic", Color.Orange, 2.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(301)
                .SetVisible(false);


            RegisterComponent(Sidebar_SolidColComp = new SolidColorComponent(Team.Color))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(200)
                .SetAlpha(HUDOpacity);

            RegisterComponent(Sidebar_AvatarComp = new AvatarComponent(this))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(201)
                .SetScale(new Vector2(sidebarWidth * 0.4f, sidebarWidth * 0.5f));

            RegisterComponent(Sidebar_PlayerName_LabelComp = new LabelComponent(Team.Name, "generic", Color.White, 1.0f))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(201);

            RegisterComponent(Sidebar_Score_LabelComp = new LabelComponent("Score: 0", "generic", Color.White, 1.0f))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(201);

            RegisterComponent(Sidebar_ShipCount_LabelComp = new LabelComponent("Ships: 0", "generic", Color.White, 1.0f))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(201);

            RegisterComponent(Sidebar_CurrentlySelected_LabelComp = new LabelComponent("Currently Selected: NONE", "generic", Color.White, 1.0f, ETextHorizontalAlign.Center))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(201);

            RegisterComponent(Sidebar_ShipHP_ProgressComp = new ProgressBarComponent(1.0f, 1.0f, Team.Color, Color.Gray))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(201)
                .SetVisible(false);

            RegisterComponent(Sidebar_ShipHP_LabelComp = new LabelComponent("", "generic", Color.White, 1.0f, ETextHorizontalAlign.Center))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202);

            RegisterComponent(Sidebar_ShipFuel_ProgressComp = new ProgressBarComponent(1.0f, 1.0f, Team.Color, Color.Gray))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(201)
                .SetVisible(false);

            RegisterComponent(Sidebar_ShipFuel_LabelComp = new LabelComponent("", "generic", Color.White, 1.0f, ETextHorizontalAlign.Center))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202);

            RegisterComponent(Sidebar_CurrentlyOrbiting_LabelComp = new LabelComponent("", "generic", Color.White, 1.0f, ETextHorizontalAlign.Center))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(201);

            RegisterComponent(Sidebar_PlanetResourceType_LabelComp = new LabelComponent("", "generic", Color.White, 1.0f, ETextHorizontalAlign.Center))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(201);

            RegisterComponent(Sidebar_PlanetResourceLevels_ProgressComp = new ProgressBarComponent(1.0f, 1.0f, Team.Color, Color.Gray))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(201)
                .SetVisible(false);

            RegisterComponent(Sidebar_PlanetResourceLevels_LabelComp = new LabelComponent("", "generic", Color.White, 1.0f, ETextHorizontalAlign.Center))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202);

            RegisterComponent(Sidebar_PlanetOwner_Label_LabelComp = new LabelComponent("PLANET OWNER:", "generic", Color.White, 1.2f))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202);

            RegisterComponent(Sidebar_PlanetOwner_Name_LabelComp = new LabelComponent("Name", "generic", Color.White))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202);

            RegisterComponent(Sidebar_PlanetOwner_Opinion_LabelComp = new LabelComponent("Neutral", "generic", Color.White))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202);

            RegisterComponent(Sidebar_PlanetOwner_AvatarComp = new AvatarComponent(this))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202)
                .SetScale(new Vector2(sidebarWidth * 0.4f, sidebarWidth * 0.5f));


            RegisterComponent(Sidebar_CaptureNode_ButtonComp = new ButtonComponent("Capture", "generic", Color.White, 1.0f, Color.Gray, Color.LightGray, Color.DimGray, () =>
                {
                    if (SelectedObject is Ship)
                    {
                        Ship ship = (Ship)SelectedObject;
                        DataManager.Output(this.Team.Name + " has begun to capture " + ship.targetNode.Name + " using ship " + ship.Name);
                        ship.targetNode.BeginCapture(ship);
                    }
                }))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202);

            RegisterComponent(Sidebar_AttackPlayer_ButtonComp = new ButtonComponent("Attack", "generic", Color.White, 1.0f, Color.Gray, Color.LightGray, Color.DimGray, () =>
                {
                    if (SelectedObject is Ship)
                        ((Ship)SelectedObject).BeginAttack();
                }))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202);


            RegisterComponent(EmoteHappy_ButtonComp = new ButtonComponent("emote_happy", Color.White, Color.LightYellow, Color.White * 0.7f, () =>
                {
                    if (SelectedObject is Ship)
                        ((Ship)SelectedObject).Emote("happy");
                }))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202)
                .SetScale(new Vector2(64.0f, 64.0f));

            RegisterComponent(EmoteUnhappy_ButtonComp = new ButtonComponent("emote_unhappy", Color.White, Color.LightYellow, Color.White * 0.7f, () =>
            {
                if (SelectedObject is Ship)
                    ((Ship)SelectedObject).Emote("unhappy");
            }))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202)
                .SetScale(new Vector2(64.0f, 64.0f));

            RegisterComponent(EmoteAngry_ButtonComp = new ButtonComponent("emote_angry", Color.White, Color.LightYellow, Color.White * 0.7f, () =>
            {
                if (SelectedObject is Ship)
                    ((Ship)SelectedObject).Emote("angry");
            }))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202)
                .SetScale(new Vector2(64.0f, 64.0f));

            RegisterComponent(EmoteQuestion_ButtonComp = new ButtonComponent("emote_question", Color.White, Color.LightYellow, Color.White * 0.7f, () =>
            {
                if (SelectedObject is Ship)
                    ((Ship)SelectedObject).Emote("question");
            }))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202)
                .SetScale(new Vector2(64.0f, 64.0f));

            RegisterComponent(EmoteExclamation_ButtonComp = new ButtonComponent("emote_exclamation", Color.White, Color.LightYellow, Color.White * 0.7f, () =>
            {
                if (SelectedObject is Ship)
                    ((Ship)SelectedObject).Emote("exclamation");
            }))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202)
                .SetScale(new Vector2(64.0f, 64.0f));

            RegisterComponent(EmoteEllipses_ButtonComp = new ButtonComponent("emote_ellipses", Color.White, Color.LightYellow, Color.White * 0.7f, () =>
            {
                if (SelectedObject is Ship)
                    ((Ship)SelectedObject).Emote("ellipses");
            }))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(202)
                .SetScale(new Vector2(64.0f, 64.0f));


            RegisterComponent(GameOver_SolidColComp = new SolidColorComponent(Color.Red * 0.3f))
                 .SetRenderSpace(ERenderSpace.ScreenSpace)
                 .SetZIndex(400)
                 .SetVisible(false);

            RegisterComponent(GameOver_LabelComp = new LabelComponent("GAME OVER", "generic", Color.Red, 8.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(401)
                .SetVisible(false);

            RegisterComponent(GameOverSub_LabelComp = new LabelComponent("UNKNOWN WIN STATE", "generic", Color.Red, 2.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(401)
                .SetVisible(false);

            RegisterComponent(MainMenu_ButtonComp = new ButtonComponent("Main Menu", "generic", Color.Blue, 4.0f, Color.Blue * 0.5f, Color.Cyan * 0.5f, Color.Blue * 0.8f, () =>
                {
                    SceneManager.LoadScene(new MainMenuScene());
                }))
                .SetRenderSpace(ERenderSpace.ScreenSpace)
                .SetZIndex(401)
                .SetVisible(false);

            NotifyResolutionChanged();
        }

        public override void NotifyResolutionChanged()
        {
            TimeLeft_LabelComp.SetPosition(new Vector2(Main.Resolution.X * 0.45f, 64.0f));
            Notification_LabelComp.SetPosition(new Vector2(Main.Resolution.X * 0.45f, 256.0f));

            Resources_SolidColComp.SetPositionScale(new Vector2(-8.0f, -8.0f), new Vector2(Main.Resolution.X + 8.0f - sidebarWidth, 32.0f));

            Sidebar_SolidColComp.SetPositionScale(new Vector2(Main.Resolution.X - sidebarWidth, -8.0f), new Vector2(sidebarWidth + 8.0f, Main.Resolution.Y + 16.0f));
            float leftX = Sidebar_SolidColComp.Position.X;
            float centerX = Sidebar_SolidColComp.Center.X;
            float rightX = Main.Resolution.X;

            Sidebar_AvatarComp.SetPosition(new Vector2(rightX - Sidebar_AvatarComp.Scale.X + 1.0f, 0.0f)); //Add 1 to avoid weird edge seam
            Sidebar_PlayerName_LabelComp.SetPosition(new Vector2(leftX + 4, 4.0f));
            Sidebar_Score_LabelComp.SetPosition(new Vector2(leftX + 4, 24.0f));
            Sidebar_ShipCount_LabelComp.SetPosition(new Vector2(leftX + 4, 44.0f));

            Sidebar_CurrentlySelected_LabelComp.SetPosition(new Vector2(centerX, Sidebar_AvatarComp.Position.Y + Sidebar_AvatarComp.Scale.Y + 4.0f));
            Sidebar_CurrentlyOrbiting_LabelComp.SetPosition(new Vector2(centerX, Sidebar_CurrentlySelected_LabelComp.Position.Y + 64.0f));
            Sidebar_ShipHP_ProgressComp.SetScale(new Vector2(sidebarWidth - 32.0f, 16.0f)).SetPosition(new Vector2(centerX - Sidebar_ShipHP_ProgressComp.Scale.X / 2.0f, Sidebar_CurrentlyOrbiting_LabelComp.Position.Y + 24.0f));
            Sidebar_ShipHP_LabelComp.SetPosition(new Vector2(centerX, Sidebar_ShipHP_ProgressComp.Position.Y));
            Sidebar_ShipFuel_ProgressComp.SetScale(new Vector2(sidebarWidth - 32.0f, 16.0f)).SetPosition(new Vector2(centerX - Sidebar_ShipFuel_ProgressComp.Scale.X / 2.0f, Sidebar_ShipHP_ProgressComp.Position.Y + 24.0f));
            Sidebar_ShipFuel_LabelComp.SetPosition(new Vector2(centerX, Sidebar_ShipFuel_ProgressComp.Position.Y));

            Sidebar_PlanetResourceType_LabelComp.SetPosition(new Vector2(centerX, Sidebar_ShipFuel_ProgressComp.Position.Y + 32.0f));
            Sidebar_PlanetResourceLevels_ProgressComp.SetScale(new Vector2(sidebarWidth - 32.0f, 16.0f)).SetPosition(new Vector2(centerX - Sidebar_PlanetResourceLevels_ProgressComp.Scale.X / 2.0f, Sidebar_PlanetResourceType_LabelComp.Position.Y + 24.0f));
            Sidebar_PlanetResourceLevels_LabelComp.SetPosition(new Vector2(centerX, Sidebar_PlanetResourceLevels_ProgressComp.Position.Y));

            Sidebar_PlanetOwner_Label_LabelComp.SetPosition(new Vector2(leftX + 4.0f, Sidebar_PlanetResourceLevels_ProgressComp.Position.Y + Sidebar_PlanetResourceLevels_ProgressComp.Scale.Y + 12.0f));
            Sidebar_PlanetOwner_Name_LabelComp.SetPosition(new Vector2(leftX + 4.0f, Sidebar_PlanetOwner_Label_LabelComp.Position.Y + 24.0f));
            Sidebar_PlanetOwner_Opinion_LabelComp.SetPosition(new Vector2(leftX + 4.0f, Sidebar_PlanetOwner_Name_LabelComp.Position.Y + 20.0f));
            Sidebar_PlanetOwner_AvatarComp.SetPosition(new Vector2(rightX - Sidebar_AvatarComp.Scale.X + 1.0f, Sidebar_PlanetOwner_Name_LabelComp.Position.Y));

            Sidebar_CaptureNode_ButtonComp.SetScale(new Vector2(sidebarWidth / 2 - 32.0f, 32.0f)).SetPosition(new Vector2(leftX + 16.0f, Sidebar_PlanetOwner_AvatarComp.Position.Y + Sidebar_PlanetOwner_AvatarComp.Scale.Y + 12.0f));
            Sidebar_AttackPlayer_ButtonComp.SetPositionScale(new Vector2(centerX + 16.0f, Sidebar_CaptureNode_ButtonComp.Position.Y), Sidebar_CaptureNode_ButtonComp.Scale);

            EmoteHappy_ButtonComp.SetPosition(new Vector2(leftX + 4.0f, Sidebar_AttackPlayer_ButtonComp.Position.Y + Sidebar_AttackPlayer_ButtonComp.Scale.Y + 12.0f));
            EmoteUnhappy_ButtonComp.SetPosition(new Vector2(centerX - EmoteAngry_ButtonComp.Scale.X / 2, EmoteHappy_ButtonComp.Position.Y));
            EmoteAngry_ButtonComp.SetPosition(new Vector2(rightX - EmoteAngry_ButtonComp.Scale.X - 4.0f, EmoteHappy_ButtonComp.Position.Y));

            EmoteQuestion_ButtonComp.SetPosition(new Vector2(leftX + 4.0f, EmoteUnhappy_ButtonComp.Position.Y + 72.0f));
            EmoteExclamation_ButtonComp.SetPosition(new Vector2(centerX - EmoteAngry_ButtonComp.Scale.X / 2, EmoteQuestion_ButtonComp.Position.Y));
            EmoteEllipses_ButtonComp.SetPosition(new Vector2(rightX - EmoteAngry_ButtonComp.Scale.X - 4.0f, EmoteQuestion_ButtonComp.Position.Y));

            GameOver_SolidColComp.SetPositionScale(Vector2.Zero, new Vector2(Main.Resolution.X, Main.Resolution.Y));
            GameOver_LabelComp.SetPosition(new Vector2(GameOver_SolidColComp.Scale.X / 2, GameOver_SolidColComp.Scale.Y * 0.35f));
            GameOverSub_LabelComp.SetPosition(new Vector2(GameOver_SolidColComp.Scale.X / 2, GameOver_SolidColComp.Scale.Y * 0.45f));
            MainMenu_ButtonComp.SetPositionScale(
                new Vector2(GameOver_SolidColComp.Scale.X / 2 - 164.0f, GameOver_SolidColComp.Scale.Y * 0.65f),
                new Vector2(328.0f, 64.0f));
        }

        protected override void OnUpdate(float delta)
        {
            if (PlayerState == EPlayerState.InGame)
            {
                TimeLeft_LabelComp.SetVisible(false);
                if (SceneManager.ActiveScene is GameScene)
                {
                    GameScene gs = (GameScene)SceneManager.ActiveScene;
                    if (gs.Time >= 0)
                    {
                        int minutes = (int)((gs.Time + 1.0f) / 60.0f);
                        int seconds = (int)((gs.Time + 1.0f) % 60.0f);
                        TimeLeft_LabelComp.SetText(minutes.ToString("d2") + ":" + seconds.ToString("d2")).SetVisible(true);
                    }
                }

                UpdateResourcesInfo();
                UpdateSidebar();

                bool hasWon = true;
                for (int i = 0; i < PlayerManager.Players.Count; ++i)
                {
                    if (PlayerManager.Players[i] == this)
                        continue;

                    if (PlayerManager.Players[i].OwnedNodes.Count > 0)
                    {
                        hasWon = false;
                        break;
                    }
                }

                if (hasWon)
                    GameOver(EGameOverState.Win_KillAll);
            }
        }

        /// <summary>
        /// Update the current resource details
        /// </summary>
        private void UpdateResourcesInfo()
        {
            if (resources.Count == 0)
            {
                Resources_LabelComp.SetText("Resources: NONE");
            }
            else
            {
                StringBuilder sb = new StringBuilder(); //I use this as its a tiny bit faster than a normal string;
                sb.Append("Resources: ");
                foreach (KeyValuePair<EResourceType, float> res in resources)
                {
                    sb.Append(NodeResourceInfo.ResourceInfo[res.Key].ResourceName);
                    sb.Append(" (");
                    sb.Append(res.Value.ToString("F"));
                    sb.Append(" ");
                    sb.Append(NodeResourceInfo.ResourceInfo[res.Key].ResourceUnits);
                    sb.Append(") | ");
                }
                Resources_LabelComp.SetText(sb.ToString());
            }
        }

        /// <summary>
        /// Updates the sidebar details
        /// </summary>
        private void UpdateSidebar()
        {
            bool selectedShip = SelectedObject is Ship;


            Sidebar_Score_LabelComp.SetText("Score: " + GetScore());
            Sidebar_ShipCount_LabelComp.SetText("Ships: " + ownedShips.Count);

            Sidebar_PlanetOwner_Label_LabelComp.SetVisible(false);
            Sidebar_PlanetOwner_Name_LabelComp.SetVisible(false);
            Sidebar_PlanetOwner_Opinion_LabelComp.SetVisible(false);
            Sidebar_PlanetOwner_AvatarComp.SetVisible(false);

            Sidebar_CaptureNode_ButtonComp.SetVisible(false);
            Sidebar_AttackPlayer_ButtonComp.SetVisible(false);

            EmoteHappy_ButtonComp.SetVisible(selectedShip);
            EmoteUnhappy_ButtonComp.SetVisible(selectedShip);
            EmoteAngry_ButtonComp.SetVisible(selectedShip);
            EmoteQuestion_ButtonComp.SetVisible(selectedShip);
            EmoteExclamation_ButtonComp.SetVisible(selectedShip);
            EmoteEllipses_ButtonComp.SetVisible(selectedShip);


            if (selectedShip)
            {
                Ship s = (Ship)SelectedObject;

                switch (s.shipState)
                {
                    case Ship.EShipState.Travelling:
                        Sidebar_CurrentlyOrbiting_LabelComp.SetText("Travelling to: " + s.targetNode.Name);
                        break;
                    case Ship.EShipState.Orbiting:
                        Sidebar_CurrentlyOrbiting_LabelComp.SetText("Orbiting: " + s.targetNode.Name);
                        break;
                    case Ship.EShipState.Attacking:
                        Sidebar_CurrentlyOrbiting_LabelComp.SetText("Attacking: " + s.targetNode.Name);
                        break;
                    default:
                        Sidebar_CurrentlyOrbiting_LabelComp.SetText("UNKNOWN: " + s.targetNode.Name);
                        break;
                }

                Sidebar_ShipHP_ProgressComp.SetValue(s.Health, s.MaxHealth).SetVisible(true);
                Sidebar_ShipHP_LabelComp.SetText("HP: " + (int)s.Health + "/" + (int)s.MaxHealth);
                Sidebar_ShipFuel_ProgressComp.SetValue(s.Fuel, s.MaxFuel).SetVisible(true);
                Sidebar_ShipFuel_LabelComp.SetText("Fuel: " + s.Fuel.ToString("F") + "/" + s.MaxFuel.ToString("F") + " " + NodeResourceInfo.ResourceInfo[EResourceType.Fuel].ResourceUnits);

                Sidebar_PlanetResourceType_LabelComp.SetText("Resource type: " + s.targetNode.GetResourceTypeName());
                Sidebar_PlanetResourceLevels_ProgressComp.SetValue(s.targetNode.CurrentResourceCount, s.targetNode.MaxResourceCount).SetVisible(true);
                Sidebar_PlanetResourceLevels_LabelComp.SetText(s.targetNode.CurrentResourceCount.ToString("F") + "/" + s.targetNode.MaxResourceCount.ToString("F") + " " + NodeResourceInfo.ResourceInfo[s.targetNode.NodeType].ResourceUnits);

                List<Ship> enemyShips = new List<Ship>();
                enemyShips.AddRange(s.targetNode.CapturingShips.Where((es) => es.OwningPlayer != this));
                enemyShips.AddRange(s.targetNode.IdleShips.Where((es) => es.OwningPlayer != this));
                enemyShips.AddRange(s.targetNode.MiningShips.Where((es) => es.OwningPlayer != this));

                if (s.shipState == Ship.EShipState.Orbiting)
                {
                    if (enemyShips.Count > 0)
                    {
                        Sidebar_AttackPlayer_ButtonComp.SetVisible(true);
                    }
                }

                if (s.shipState == Ship.EShipState.Orbiting || s.shipState == Ship.EShipState.Attacking)
                {
                    if (s.targetNode.OwningPlayer != null && s.targetNode.OwningPlayer != this)
                    {
                        Sidebar_PlanetOwner_Label_LabelComp.SetVisible(true);
                        Sidebar_PlanetOwner_Name_LabelComp.SetText(s.targetNode.OwningPlayer.Team.Name).SetVisible(true);
                        Sidebar_PlanetOwner_Opinion_LabelComp.SetText(s.targetNode.OwningPlayer.GetOpinion(this)).SetVisible(true);
                        Sidebar_PlanetOwner_AvatarComp.SetOwningPlayer(s.targetNode.OwningPlayer).SetVisible(true);
                    }

                    if (s.targetNode.CurrentOwner != null && s.targetNode.CurrentOwner != this && s.targetNode.CapturingPlayer == null && s.targetNode.CanMine(this))
                    {
                        Sidebar_CaptureNode_ButtonComp.SetVisible(true);
                    }
                }
            }
            else
            {
                Sidebar_CurrentlyOrbiting_LabelComp.SetText("");
                Sidebar_ShipHP_ProgressComp.SetVisible(false);
                Sidebar_ShipHP_LabelComp.SetText("");
                Sidebar_ShipFuel_ProgressComp.SetVisible(false);
                Sidebar_ShipFuel_LabelComp.SetText("");

                Sidebar_PlanetResourceType_LabelComp.SetText("");
                Sidebar_PlanetResourceLevels_ProgressComp.SetVisible(false);
                Sidebar_PlanetResourceLevels_LabelComp.SetText("");
            }
        }

        /// <summary>
        /// Displays a popup notification in orange
        /// </summary>
        /// <param name="text">The text to display</param>
        public void Notification(string text)
        {
            Notification(text, Color.Orange);
        }

        /// <summary>
        /// Displays a popup notification
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="color">The color to display the text in</param>
        public void Notification(string text, Color color)
        {
            if(Notification_LabelComp != null) //Only display if we have a UI
                Notification_LabelComp.SetText(text).SetTint(color).SetVisible(true).FadeOut(1.0f, false, 2.0f);
        }

        /// <summary>
        /// Returns the calculated player score
        /// </summary>
        /// <returns>The calculated player score</returns>
        public int GetScore()
        {
            int score = 0;

            foreach(KeyValuePair<EResourceType, float> kvp in resources)
            {
                score += (int)(NodeResourceInfo.ResourceInfo[kvp.Key].ResourceScore * kvp.Value);
            }

            return score;
        }

        /// <summary>
        /// Triggers a gameover state for the player
        /// </summary>
        /// <param name="reason">What type of gameover</param>
        public void GameOver(EGameOverState reason)
        {
            if (PlayerState == EPlayerState.Won || PlayerState == EPlayerState.Lost)
                return;

            PlayerState = (reason == EGameOverState.Win_KillAll || reason == EGameOverState.Win_Score) ? EPlayerState.Won : EPlayerState.Lost;
            if (PlayerManager.LocalPlayer == this)
            {
                GameOver_SolidColComp.SetVisible(true);
                GameOver_LabelComp.SetVisible(true);
                GameOverSub_LabelComp.SetVisible(true);
                MainMenu_ButtonComp.SetVisible(true);

                if (PlayerState == EPlayerState.Won)
                {
                    GameOver_SolidColComp.SetTint(Color.Green * 0.3f);
                    GameOver_LabelComp.SetText("YOU WON!").SetTint(Color.Green);
                    GameOverSub_LabelComp.SetTint(Color.Green);
                }

                GameOverSub_LabelComp.SetText(gameOverStrings[reason]);
            }

            if (PlayerState == EPlayerState.Won)
                DataManager.Output(Team.Name + " won the game with a score of " + GetScore() + " [" + reason.ToString() + "]");
            else
                DataManager.Output(Team.Name + " lost the game with a score of " + GetScore() + " [" + reason.ToString() + "]");
        }

        /// <summary>
        /// Sets this player's currently selected object
        /// </summary>
        /// <param name="obj">The new object to select</param>
        public void SetSelectedObject(Entity obj)
        {
            SelectedObject = obj;
            Sidebar_CurrentlySelected_LabelComp.SetText("Currently Selected: " + SelectedObject.ToString());
        }

        /// <summary>
        /// Add an owned node
        /// </summary>
        /// <param name="newNode">The newly owned node</param>
        public void AddOwnedNode(Node newNode)
        {
            if (!OwnedNodes.Contains(newNode))
            {
                OwnedNodes.Add(newNode);
                DataManager.Output(Team.Name + " gained node " + newNode.Name);
            }
        }

        /// <summary>
        /// Remove an owned node
        /// </summary>
        /// <param name="delNode">The newly lost node</param>
        public void RemoveOwnedNode(Node delNode)
        {
            if (OwnedNodes.Contains(delNode))
            {
                OwnedNodes.Remove(delNode);
                DataManager.Output(Team.Name + " lost node " + delNode.Name);
                Notification("Lost planet " + delNode.Name + "!", Color.Red);
            }

            if (OwnedNodes.Count == 0)
                GameOver(EGameOverState.Lose_LostPlanets);
        }

        /// <summary>
        /// Add an owned ship
        /// </summary>
        /// <param name="ship">The newly owned ship</param>
        public void AddOwnedShip(Ship ship)
        {
            if(!ownedShips.Contains(ship))
                ownedShips.Add(ship);
        }

        /// <summary>
        /// Remove an owned ship
        /// </summary>
        /// <param name="ship">The newly lost ship</param>
        public void RemoveOwnedShip(Ship ship)
        {
            ownedShips.Remove(ship);

            if (SelectedObject == ship)
                SelectedObject = null;

            if (ownedShips.Count == 0)
            {
                if(GameOverSub_LabelComp != null)
                    GameOverSub_LabelComp.SetText("All your ships were destroyed!");
                GameOver(EGameOverState.Lose_LostShips);
            }
        }

        /// <summary>
        /// Allows the AI to act when their ship is destroyed
        /// </summary>
        /// <param name="causer">The ship (if any) that destroyed my ship</param>
        public virtual void NotifyShipDestroyed(Ship causer)
        {
        }

        /// <summary>
        /// Do we own this resource?
        /// </summary>
        /// <param name="resourceType">The resource to check</param>
        /// <returns>True if we own this resource, otherwise false</returns>
        public bool HasResource(EResourceType resourceType)
        {
            return resources.ContainsKey(resourceType);
        }

        /// <summary>
        /// Adds some resources to our stash
        /// </summary>
        /// <param name="resourceType">The type of resource to add</param>
        /// <param name="num">The number of resources to add</param>
        /// <returns>The difference in owned resoruces (Incase we actually lost resources)</returns>
        public float AddResources(EResourceType resourceType, float num)
        {
            float oldResourceCount = 0;

            if (HasResource(resourceType))
            {
                oldResourceCount = resources[resourceType];
                resources[resourceType] += num;
            }
            else
            {
                resources.Add(resourceType, num);
            }

            float newResourceCount = Math.Max(0.0f, resources[resourceType]);

            if (newResourceCount <= 0.0f)
                resources.Remove(resourceType);

            return newResourceCount - oldResourceCount;
        }

        /// <summary>
        /// Returns the number of resources of a particular type
        /// </summary>
        /// <param name="resourceType">The resource type to check</param>
        /// <returns>The number of resources of the specified type</returns>
        public float GetResourceCount(EResourceType resourceType)
        {
            if (HasResource(resourceType))
                return resources[resourceType];

            return 0.0f;
        }

        /// <summary>
        /// Returns this player's affiliation with another player (Used for AI)
        /// </summary>
        /// <param name="other">The player we're getting an opinion for</param>
        /// <returns>This players opinion</returns>
        public virtual string GetOpinion(Player other)
        {
            return "Unknown";
        }
    }
}
