// File Author: Daniel Masterson
using SpaceGame.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.GameManagers
{
    /// <summary>
    /// Manages all the players
    /// </summary>
    public class PlayerManager : AbstractManager
    {
        public static Player LocalPlayer { get; private set; }
        public static List<Player> Players { get; private set; }
        static bool runPlayerLogic = false;

        public PlayerManager()
        {
            Players = new List<Player>();
            TeamInfo.InitializeDefaultTeams();
        }

        public override void OnManagerUpdate(float delta)
        {
            if (!runPlayerLogic)
                return;

            for (int i = 0; i < Players.Count; ++i)
                Players[i].Update(delta);
        }

        public static Player AddNewPlayer(bool isAI, bool activateNow, TeamInfo team = null)
        {
            if(team == null)
                team = TeamInfo.PotentialTeams[Players.Count % TeamInfo.PotentialTeams.Count];
            Player p;
            if (isAI)
                p = new AIPlayer(team);
            else
                p = new Player(team);

            Players.Add(p);
            if (LocalPlayer == null)
                LocalPlayer = p;

            if(activateNow)
                p.DoSpawn();

            return p;
        }

        public static Player GetPlayerFromID(int id)
        {
            if (id < 0 || id >= Players.Count)
                return null;

            return Players[id];
        }

        public static int GetPlayerCount()
        {
            return Players.Count;
        }

        public static void ClearPlayers()
        {
            for (int i = 0; i < Players.Count; ++i)
                Players[i].DoDestroy();

            Players.Clear();
            LocalPlayer = null;
        }

        public static void NotifyResolutionChanged()
        {
            for (int i = 0; i < Players.Count; ++i)
                Players[i].NotifyResolutionChanged();
        }

        public override void OnManagerDestroy()
        {
            ClearPlayers();
        }

        public static void DisablePlayers()
        {
            runPlayerLogic = false;
        }

        public static void EnablePlayers()
        {
            runPlayerLogic = true;

            for (int i = 0; i < Players.Count; i++)
            {
                if (!Players[i].IsAlive)
                    Players[i].DoSpawn();
            }
        }
    }
}
