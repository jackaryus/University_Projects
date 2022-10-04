// File Author: Adam Kadow
using Microsoft.Xna.Framework;
using SpaceGame.Entities;
using SpaceGame.GameManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Components
{
    /// <summary>
    /// Handles rendering of a player's avatar, as well as some elements of the customization system
    /// </summary>
    class AvatarComponent : Component
    {
        static bool initializedLists = false;
        public static List<string> avatarList = new List<string>();
        public static List<string> shipList = new List<string>();
        public static List<Color> colorList = new List<Color>();
        public int avatarNumber = 0;
        public int colorNumber = -1;
        public int shipNumber = 0;

        /// <summary>
        /// Handles rendering of a player's avatar, as well as some elements of the customization system
        /// </summary>
        /// <param name="owningPlayer">The player this avatar represents</param>
        public AvatarComponent(Player owningPlayer)
        {
            SetOwningPlayer(owningPlayer);

            if (!initializedLists)
            {
                avatarList.Add("captain");
                avatarList.Add("husk");
                avatarList.Add("psion");
                avatarList.Add("scienceofficer");
                avatarList.Add("securityofficer");
                avatarList.Add("starpilot");

                shipList.Add("1");
                shipList.Add("2");
                shipList.Add("3");

                colorList.Add(Color.Cyan);
                colorList.Add(Color.Blue);
                colorList.Add(Color.Purple);
                colorList.Add(Color.Magenta);
                colorList.Add(Color.Red);
                colorList.Add(Color.Orange);
                colorList.Add(Color.Yellow);
                colorList.Add(Color.Lime);
                colorList.Add(Color.Green);

                initializedLists = true;
            }

            for (int i = 0; i < colorList.Count; ++i)
            {
                if (owningPlayer.Team.Color == colorList[i])
                {
                    colorNumber = i;
                    break;
                }
            }

            if (owningPlayer.Team.Avatar == null)
                owningPlayer.Team.Avatar = avatarList[0];

            if (owningPlayer.Team.Ship == null)
                owningPlayer.Team.Ship = shipList[0];
        }

        /// <summary>
        /// Returns the current avatar texture
        /// </summary>
        /// <returns>The current avatar texture</returns>
        public string GetCurrentAvatar()
        {
            return OwningPlayer.Team.Avatar;
        }

        /// <summary>
        /// Increments the avatar texture number and returns the new avatar
        /// </summary>
        /// <returns>The next avatar texture</returns>
        public string GetNextAvatar()
        {
            avatarNumber += 1;
            if (avatarNumber > avatarList.Count - 1)
            {
                avatarNumber = 0;
            }

            OwningPlayer.Team.Avatar = avatarList[avatarNumber];
            return GetCurrentAvatar();
        }

        /// <summary>
        /// Returns the current ship texture
        /// </summary>
        /// <returns>The current ship texture</returns>
        public string GetCurrentShip()
        {
            return OwningPlayer.Team.Ship;
        }

        /// <summary>
        /// Increments the ship texture number and returns the new ship
        /// </summary>
        /// <returns>The next ship texture</returns>
        public string GetNextShip()
        {
            shipNumber += 1;
            if (shipNumber > shipList.Count - 1)
            {
                shipNumber = 0;
            }

            OwningPlayer.Team.Ship = shipList[shipNumber];
            return GetCurrentShip();
        }

        /// <summary>
        /// Returns the current player color
        /// </summary>
        /// <returns>The current player color</returns>
        public Color GetCurrentColor()
        {
            return OwningPlayer.Team.Color;
        }

        /// <summary>
        /// Increments the player color number and returns the new color
        /// </summary>
        /// <returns>The next player color</returns>
        public Color GetNextColor()
        {
            colorNumber += 1;
            if (colorNumber > colorList.Count - 1)
            {
                colorNumber = 0;
            }

            OwningPlayer.Team.Color = colorList[colorNumber];
            return GetCurrentColor();
        }

        protected override void OnRender(float delta)
        {
            Draw(GraphicsManager.GetTexture("avatar_" + OwningPlayer.Team.Avatar + "_u"), Bounds, null, Color.White * Alpha, Rotation, false);
            Draw(GraphicsManager.GetTexture("avatar_" + OwningPlayer.Team.Avatar + "_o"), Bounds, null, GetCurrentColor() * Alpha, Rotation, false);
        }
    }
}
