// File Author: Daniel Masterson
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    /// <summary>
    /// Details about a team
    /// (Note: Each player counts as a team for now, although this can provide support for multiple players on the same team)
    /// </summary>
    public class TeamInfo
    {
        public static List<TeamInfo> PotentialTeams = new List<TeamInfo>();

        public string Name { get; set; }
        public Color Color { get; set; }
        public string Avatar { get; set; }
        public string Ship { get; set; }

        public TeamInfo(string name, Color color, string avatar, string ship)
        {
            Name = name;
            Color = color;
            Avatar = avatar;
            Ship = ship;
        }

        public static void InitializeDefaultTeams()
        {
            PotentialTeams.Add(new TeamInfo("Human Allied Forces",  Color.Blue,     "captain",          "1"));
            PotentialTeams.Add(new TeamInfo("Crayshaw Collective",  Color.Red,      "psion",            "2"));
            PotentialTeams.Add(new TeamInfo("SamSpain.Com",         Color.Lime,     "scienceofficer",   "3"));
            PotentialTeams.Add(new TeamInfo("Freedom Squad",        Color.Magenta,  "husk",             "1"));
            PotentialTeams.Add(new TeamInfo("Yellow Devils",        Color.Orange,   "starpilot",        "2"));
            PotentialTeams.Add(new TeamInfo("Oceanic Drifters",     Color.Green,    "securityofficer",  "1"));
            PotentialTeams.Add(new TeamInfo("The NASA",             Color.Cyan,     "scienceofficer",   "2"));
            PotentialTeams.Add(new TeamInfo("Void Dominion",        Color.Purple,   "psion",            "3"));
        }
    }
}
