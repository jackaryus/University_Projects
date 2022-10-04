// File Author: Daniel Masterson
using SpaceGame.Components;
using SpaceGame.GameManagers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Entities
{
    public enum EResourceType
    {
        NONE,
        Fuel,
        Mineral,
        Antimatter,
    }

    /// <summary>
    /// Stores details about a resource type
    /// </summary>
    public class NodeResourceInfo
    {
        public static Dictionary<EResourceType, NodeResourceInfo> ResourceInfo = new Dictionary<EResourceType, NodeResourceInfo>();

        public string ResourceName;
        public string ResourceUnits;
        public float ResourceScore;
        public float MinCount; //Min initial resource count for a node
        public float MaxCount; //Max initial resource count for a node
        public float BaseMiningRate;
        public float MinRegen;
        public float MaxRegen;
        public float ChanceToSpawn;

        public NodeResourceInfo(string settingsMidfix)
        {
            string s = "resource." + settingsMidfix + ".";

            ResourceName = SettingsManager.GetSetting<string>(s + "name");
            ResourceUnits = SettingsManager.GetSetting<string>(s + "units");
            ResourceScore = SettingsManager.GetSetting<float>(s + "score");
            MinCount = SettingsManager.GetSetting<float>(s + "minResourceAmount");
            MaxCount = SettingsManager.GetSetting<float>(s + "maxResourceAmount");
            BaseMiningRate = SettingsManager.GetSetting<float>(s + "miningRate");
            MinRegen = SettingsManager.GetSetting<float>(s + "minRegenRate");
            MaxRegen = SettingsManager.GetSetting<float>(s + "maxRegenRate");
            ChanceToSpawn = SettingsManager.GetSetting<float>(s + "planetChance");
        }

        public static void AddNewResource(EResourceType type, NodeResourceInfo info)
        {
            ResourceInfo.Add(type, info);
        }
    }

    /// <summary>
    /// A planet in the game. These act as resource generators and as the game's territory
    /// </summary>
    public class Node : Entity
    {
        /// <summary>
        /// Describes a link with another node and stores our half of the line connecting to it
        /// </summary>
        public class NodeConnection
        {
            public Node other;
            public LineComponent halfLine;

            public NodeConnection(Node Other, LineComponent HalfLine)
            {
                other = Other;
                halfLine = HalfLine;
            }
        }

        public string Name { get; private set; }
        public Player CurrentOwner { get; private set; }
        public List<Ship> MiningShips { get; private set; } //Owner's ships orbiting the planet
        public List<Ship> IdleShips { get; private set; } //Other players ships orbiting the planet (But not capturing)

        public Player CapturingPlayer { get; private set; }
        public List<Ship> CapturingShips { get; private set; } //Capturing player's ships orbiting the planet

        ButtonComponent buttonComp;
        LabelComponent name_LabelComp, info_LabelComp;
        ProgressBarComponent capture_ProgressComp;
        ImageComponent imageComp;

        public List<NodeConnection> nodeConnections = new List<NodeConnection>();

        public EResourceType NodeType { get; private set; }
        public float MaxResourceCount { get; private set; }
        public float CurrentResourceCount { get; private set; }
        public float ResourceRegenRate { get; private set; }

        public const float CaptureDecayRate = 0.25f;
        public const float MinCaptureTime = 7.0f;
        public const float MaxCaptureTime = 15.0f;
        public float CaptureTime { get; private set; }
        public float CaptureProgress { get; private set; }

        static Node()
        {
            //Init resources
            NodeResourceInfo.AddNewResource(EResourceType.Fuel, new NodeResourceInfo("fuel"));
            NodeResourceInfo.AddNewResource(EResourceType.Mineral, new NodeResourceInfo("minerals"));
            NodeResourceInfo.AddNewResource(EResourceType.Antimatter, new NodeResourceInfo("antimatter"));
        }

        protected override void OnSpawn()
        {
            Name = GetRandomName();
            NodeType = EResourceType.NONE;
            CaptureTime = (float)(MinCaptureTime + UtilityManager.Random.NextDouble() * (MaxCaptureTime - MinCaptureTime));

            MiningShips = new List<Ship>();
            IdleShips = new List<Ship>();
            CapturingShips = new List<Ship>();

            foreach(KeyValuePair<EResourceType, NodeResourceInfo> kvp in NodeResourceInfo.ResourceInfo)
            {
                if (kvp.Key == EResourceType.Fuel) //Don't assign fuel, that acts as our default later
                    continue;

                if(UtilityManager.Random.NextDouble() < kvp.Value.ChanceToSpawn)
                {
                    NodeType = kvp.Key;
                }
            }

            if (NodeType == EResourceType.NONE) //Default to fuel if we weren't assigned a node
                NodeType = EResourceType.Fuel;

            buttonComp = new ButtonComponent("", "generic", Color.White, 1.0f, Color.Gray, Color.AliceBlue, Color.Blue, () =>
            {
                if (PlayerManager.LocalPlayer.SelectedObject is Ship)
                    ((Ship)PlayerManager.LocalPlayer.SelectedObject).SetTargetNode(this);
            });
            buttonComp
                .SetPositionScale(Position, Scale)
                .SetRenderSpace(ERenderSpace.WorldSpace)
                .SetTexture("glow")
                .SetAlpha(0.6f);
            RegisterComponent(buttonComp);

            name_LabelComp = new LabelComponent(Name, "generic", Color.White, 1.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Top);
            name_LabelComp
                .SetPosition(new Vector2(Center.X, Position.Y + Scale.Y + 2))
                .SetRenderSpace(ERenderSpace.WorldSpace);
            RegisterComponent(name_LabelComp);

            info_LabelComp = new LabelComponent("[INFO]", "generic", Color.White, 1.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Top);
            info_LabelComp
                .SetPosition(new Vector2(Center.X, Position.Y + Scale.Y + 32))
                .SetRenderSpace(ERenderSpace.WorldSpace)
                .SetVisible(false);
            RegisterComponent(info_LabelComp);

            capture_ProgressComp = new ProgressBarComponent(0.0f, 1.0f, Color.Cyan, Color.Blue, 0.2f, 1);
            capture_ProgressComp
                .SetPositionScale(new Vector2(Position.X, Position.Y + Scale.Y + 18), new Vector2(Scale.X, 12))
                .SetRenderSpace(ERenderSpace.WorldSpace)
                .SetVisible(false);
            RegisterComponent(capture_ProgressComp);

            imageComp = new ImageComponent("null");
            imageComp
                .SetPositionScale(Position + Scale * 0.2f, Scale * 0.6f)
                .SetRenderSpace(ERenderSpace.WorldSpace)
                .SetZIndex(1);
            RegisterComponent(imageComp);

            SetResourceType(NodeType);
            CurrentResourceCount = MaxResourceCount;
            SetOwningPlayer(null);
        }

        protected override void OnUpdate(float delta)
        {
            //Regen a few resources
            CurrentResourceCount = Math.Min(MaxResourceCount, CurrentResourceCount + delta * ResourceRegenRate);

            //Don't clog up geocentric orbit with dead satellites. That's space garbage you know!
            MiningShips.RemoveAll((s) => s.shipState == Ship.EShipState.Dead);
            IdleShips.RemoveAll((s) => s.shipState == Ship.EShipState.Dead);
            CapturingShips.RemoveAll((s) => s.shipState == Ship.EShipState.Dead);

            if (CapturingShips.Count == 0)
            {
                CapturingPlayer = null;
            }

            //Mine that planet!
            foreach (Ship miner in MiningShips)
            {
                if (miner.OwningPlayer != OwningPlayer)
                    continue;

                float minedResources = CurrentResourceCount;
                CurrentResourceCount = Math.Max(0.0f, CurrentResourceCount - delta * NodeResourceInfo.ResourceInfo[NodeType].BaseMiningRate);
                minedResources = minedResources - CurrentResourceCount;
                miner.OwningPlayer.AddResources(NodeType, minedResources);

                if(CurrentResourceCount == 0 && NodeType == EResourceType.Antimatter)
                {
                    SetResourceType(EResourceType.Mineral);
                }
            }

            //Someone capturing?
            if (CaptureProgress < CaptureTime)
            {
                //Decay the capture progress by defending ship count
                CaptureProgress = Math.Max(0.0f, CaptureProgress - delta * (CaptureDecayRate * (MiningShips.Count + 1)));

                for (int i = 0; i < CapturingShips.Count; ++i)
                {
                    if (CapturingShips[i].OwningPlayer != CapturingPlayer)
                    {
                        CapturingShips.RemoveAt(i);
                        --i;
                        continue;
                    }

                    CaptureProgress += delta * CapturingShips[i].CaptureModifier;
                }

                //The capture was successful
                if (CaptureProgress >= CaptureTime)
                {
                    CaptureProgress = 0;
                    SetOwningPlayer(CapturingPlayer);
                    CapturingPlayer.Notification("Successfully captured " + Name + "!", Color.Green);
                    CapturingPlayer = null;

                    for (int i = 0; i < CapturingShips.Count; ++i)
                        CapturingShips[i].Emote("happy");

                    IdleShips = MiningShips;
                    MiningShips = CapturingShips;
                    CapturingShips = new List<Ship>();
                }
            }

            //If capturing, display the correct progress bar
            if (CaptureProgress > 0)
            {
                float capturePercentage = CaptureProgress / CaptureTime;
                info_LabelComp.SetText("Capture Percentage (" + (capturePercentage * 100).ToString("F") + "%)").SetVisible(true);
                capture_ProgressComp.SetValue(capturePercentage, 1.0f).SetVisible(true);
            }
            else
            {
                float remainingResourcesPercentage = CurrentResourceCount / MaxResourceCount;
                info_LabelComp.SetText("Resources Left (" + (remainingResourcesPercentage * 100).ToString("F") + "%)").SetVisible(true);
                capture_ProgressComp.SetValue(remainingResourcesPercentage, 1.0f).SetVisible(true);
            }
        }

        /// <summary>Capture the planet</summary>
        /// <param name="player">The new owner of the planet</param>
        /// <returns>Always returns true, to accept the new owner</returns>
        public override bool OnSetOwningPlayer(Player player)
        {
            if (CurrentOwner == player)
                return true;

            if (CurrentOwner != null)
                CurrentOwner.RemoveOwnedNode(this);

            CurrentOwner = player;

            if (CurrentOwner == null)
            {
                buttonComp.IdleColor = Color.Gray;
            }
            else
            {
                CurrentOwner.AddOwnedNode(this);
                buttonComp.IdleColor = CurrentOwner.Team.Color;
            }

            for (int i = 0; i < nodeConnections.Count; ++i)
            {
                nodeConnections[i].halfLine.SetTint(buttonComp.IdleColor);
            }

            return true;
        }

        /// <summary>
        /// Gets a random planet name
        /// </summary>
        /// <returns>A random planet name</returns>
        protected string GetRandomName()
        {
            //This vaguely emulates a planetary naming scheme. Barely.

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 3; ++i)
                sb.Append((char)UtilityManager.Random.Next(65, 90)); //Random character

            sb.Append(' ');
            sb.Append(UtilityManager.Random.Next(0, 9999));

            return sb.ToString();
        }

        /// <summary>
        /// Checks whether this player can mind this planet (Based on connections and node ownership)
        /// </summary>
        /// <param name="newCapturer">The player to test</param>
        /// <returns>True if this player can mine, otherwise false</returns>
        public bool CanMine(Player newCapturer)
        {
            for (int i = 0; i < nodeConnections.Count; ++i)
            {
                if (nodeConnections[i].other.CurrentOwner == newCapturer)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// The supplied ship will either begin mining if the ship's owner owns this planet, 
        /// or enter an idle orbit (Or if AI, begin capturing)
        /// </summary>
        /// <param name="newMiner">The new potential mining ship</param>
        public void BeginMining(Ship newMiner)
        {
            if (MiningShips.Contains(newMiner))
                return;

            if (CanMine(newMiner.OwningPlayer))
            {
                if (CurrentOwner == null)
                {
                    SetOwningPlayer(newMiner.OwningPlayer);
                }
                else if (newMiner.OwningPlayer != CurrentOwner)
                {
                    if (newMiner.OwningPlayer is AIPlayer)
                    {
                        BeginCapture(newMiner);
                        newMiner.Emote("angry");
                    }
                    else
                    {
                        IdleShips.Add(newMiner);
                    }
                    return;
                }

                MiningShips.Add(newMiner);
            }
            else if (newMiner.OwningPlayer == PlayerManager.LocalPlayer)
            {
                newMiner.OwningPlayer.Notification(newMiner.Name + " cannot capture " + Name + " (No connections)!", Color.Red);
            }
        }

        /// <summary>
        /// Call this when a ship moves away from the planet. Removes from all ship lists and
        /// aborts capture if necessary
        /// </summary>
        /// <param name="ship">The ship that has left the planet</param>
        public void StopMining(Ship ship)
        {
            MiningShips.Remove(ship);
            IdleShips.Remove(ship);
            CapturingShips.Remove(ship);
            if (CapturingShips.Count == 0 && ship.OwningPlayer == CapturingPlayer)
                CapturingPlayer = null;
        }

        /// <summary>
        /// Begin capture of this planet
        /// </summary>
        /// <param name="newCapturer">The ship capturing the planet</param>
        public void BeginCapture(Ship newCapturer)
        {
            if (newCapturer.OwningPlayer == OwningPlayer)
                return;

            if (IdleShips.Contains(newCapturer))
                IdleShips.Remove(newCapturer);

            if (CapturingShips.Contains(newCapturer))
                return;

            if (CapturingPlayer != null && CapturingPlayer != newCapturer.OwningPlayer)
            {
                newCapturer.OwningPlayer.Notification(newCapturer.Name + " cannot capture " + Name + " (Already being captured)!", Color.Red);
                return;
            }

            CapturingShips.Add(newCapturer);

            if (CapturingPlayer == null)
            {
                OwningPlayer.Notification(newCapturer.OwningPlayer.Team.Name + " is trying to capture " + Name + "!", Color.Red);
                CapturingPlayer = newCapturer.OwningPlayer;
            }
        }

        /// <summary>
        /// Check whether this node is connected to another node
        /// </summary>
        /// <param name="other">The node to check against</param>
        /// <returns>True if connected, fals otherwise</returns>
        public bool IsConnectedTo(Node other)
        {
            for (int i = 0; i < nodeConnections.Count; ++i)
            {
                if (nodeConnections[i].other == other)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Connect this node to another
        /// </summary>
        /// <param name="other">The other node to connect to</param>
        /// <param name="connectedFromOther">Do not set to true! Used internally for ConnectTo</param>
        /// <returns>This node</returns>
        public Node ConnectTo(Node other, bool connectedFromOther = false)
        {
            if (!IsConnectedTo(other))
            {
                Vector2 myCenter = new Vector2(Bounds.Center.X, Bounds.Center.Y);
                Vector2 otherCenter = new Vector2(other.Bounds.Center.X, other.Bounds.Center.Y);
                Vector2 midpoint = Vector2.Lerp(myCenter, otherCenter, 0.5f);

                LineComponent lineComp;
                if(connectedFromOther)
                    lineComp = new LineComponent(myCenter, midpoint, Color.White, 5);
                else
                    lineComp = new LineComponent(midpoint, myCenter, Color.White, 5);

                lineComp.SetRenderSpace(ERenderSpace.WorldSpace).SetZIndex(-1);
                RegisterComponent(lineComp);

                nodeConnections.Add(new NodeConnection(other, lineComp));
            }
            if (!connectedFromOther)
            {
                other.ConnectTo(this, true);
            }

            return this;
        }

        public int GetConnectionCount()
        {
            return nodeConnections.Count;
        }

        public void SetResourceType(EResourceType newType)
        {
            NodeType = newType;

            MaxResourceCount = (float)(NodeResourceInfo.ResourceInfo[NodeType].MinCount + UtilityManager.Random.NextDouble() * (NodeResourceInfo.ResourceInfo[NodeType].MaxCount - NodeResourceInfo.ResourceInfo[NodeType].MinCount));
            ResourceRegenRate = (float)(NodeResourceInfo.ResourceInfo[NodeType].MinRegen + UtilityManager.Random.NextDouble() * (NodeResourceInfo.ResourceInfo[NodeType].MaxRegen - NodeResourceInfo.ResourceInfo[NodeType].MinRegen));

            string texture = "planet_";
            switch (NodeType)
            {
                default:
                case EResourceType.Mineral:
                    texture += "minerals";
                    break;
                case EResourceType.Fuel:
                    texture += "fuel";
                    break;
                case EResourceType.Antimatter:
                    texture += "antimatter";
                    break;
            }

            texture += "_" + UtilityManager.Random.Next(1, 4);
            imageComp.SetTexture(texture);
        }

        public string GetResourceTypeName()
        {
            return NodeResourceInfo.ResourceInfo[NodeType].ResourceName;
        }
    }
}
