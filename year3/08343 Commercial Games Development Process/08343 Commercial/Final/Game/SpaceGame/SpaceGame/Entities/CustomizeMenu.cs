// File Author: Adam Kadow
using SpaceGame.Components;
using SpaceGame.GameManagers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceGame.Entities
{
    /// <summary>
    /// Player customization menu
    /// </summary>
    public class CustomiseMenu : Entity
    {
        LabelComponent title_LabelComp;
        ButtonComponent confirm_ButtonComp;
        LabelComponent playerOne_LabelComp, playerTwo_LabelComp, playerThree_LabelComp, playerFour_LabelComp, playerFive_LabelComp, playerSix_LabelComp, playerSeven_LabelComp, playerEight_LabelComp;

        AvatarComponent[] playerAvatarComps = new AvatarComponent[8];

        ImageComponent[] playerShip_U_ImageComp = new ImageComponent[8];
        ImageComponent[] playerShip_O_ImageComp = new ImageComponent[8];

        ButtonComponent[] playerAvatar_ButtonComp = new ButtonComponent[8];
        ButtonComponent[] playerShip_ButtonComp = new ButtonComponent[8];
        ButtonComponent[] playerColor_ButtonComp = new ButtonComponent[8];

        EGameType gameToSpawn;

        protected override void OnSpawn()
        {
            PlayerManager.ClearPlayers();
            for (int i = 0; i < (int)gameToSpawn; i++)
                PlayerManager.AddNewPlayer(i != 0, false);
            PlayerManager.DisablePlayers();

            // Setting up player labels
            title_LabelComp = new LabelComponent("Team Customisation", "generic", Color.Aqua, 5.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Bottom);
            title_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(title_LabelComp);

            playerOne_LabelComp = new LabelComponent("Player One", "generic", Color.Aqua, 2.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle);
            title_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(playerOne_LabelComp);

            playerTwo_LabelComp = new LabelComponent("Player Two", "generic", Color.Aqua, 2.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle);
            title_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(playerTwo_LabelComp);

            playerThree_LabelComp = new LabelComponent("Player Three", "generic", Color.Aqua, 2.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle);
            title_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(playerThree_LabelComp);

            playerFour_LabelComp = new LabelComponent("Player Four", "generic", Color.Aqua, 2.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle);
            title_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(playerFour_LabelComp);

            playerFive_LabelComp = new LabelComponent("Player Five", "generic", Color.Aqua, 2.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle);
            title_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(playerFive_LabelComp);

            playerSix_LabelComp = new LabelComponent("Player Six", "generic", Color.Aqua, 2.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle);
            title_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(playerSix_LabelComp);

            playerSeven_LabelComp = new LabelComponent("Player Seven", "generic", Color.Aqua, 2.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle);
            title_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(playerSeven_LabelComp);

            playerEight_LabelComp = new LabelComponent("Player Eight", "generic", Color.Aqua, 2.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle);
            title_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(playerEight_LabelComp);

            confirm_ButtonComp = new ButtonComponent("Confirm", "generic", Color.Aqua, 2.0f, Color.Blue * 0.5f, Color.Cyan * 0.5f, Color.Blue * 0.8f, () =>
            {
                SceneManager.LoadScene(new GameScene(gameToSpawn));
            });
            confirm_ButtonComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(confirm_ButtonComp);


            // Setting up player avatar, colour and ship options
            for (int i = 0; i < PlayerManager.Players.Count; i++)
            {
                int x = i; //Local copy for delegates

                RegisterComponent(playerAvatarComps[i] = new AvatarComponent(PlayerManager.Players[i])).SetZIndex(10);
                RegisterComponent(playerShip_U_ImageComp[i] = new ImageComponent("ship_" + playerAvatarComps[i].GetCurrentShip() + "_u")).SetZIndex(10);
                RegisterComponent(playerShip_O_ImageComp[i] = new ImageComponent("ship_" + playerAvatarComps[i].GetCurrentShip() + "_o")).SetZIndex(11).SetTint(playerAvatarComps[i].GetCurrentColor());

                RegisterComponent(playerAvatar_ButtonComp[i] = new ButtonComponent("", "generic", Color.Aqua, 0.5f, Color.Blue * 0.5f, Color.Cyan * 0.5f, Color.Blue * 0.8f, () =>
                {
                    playerAvatarComps[x].GetNextAvatar();
                }));

                RegisterComponent(playerShip_ButtonComp[i] = new ButtonComponent("", "generic", Color.Aqua, 0.5f, Color.Blue * 0.5f, Color.Cyan * 0.5f, Color.Blue * 0.8f, () =>
                {
                    playerAvatarComps[x].GetNextShip();
                    playerShip_U_ImageComp[x].SetTexture("ship_" + playerAvatarComps[x].GetCurrentShip() + "_u");
                    playerShip_O_ImageComp[x].SetTexture("ship_" + playerAvatarComps[x].GetCurrentShip() + "_o");
                }));

                RegisterComponent(playerColor_ButtonComp[i] = new ButtonComponent("Change Colour", "generic", Color.Aqua, 0.85f, Color.Blue * 0.5f, Color.Cyan * 0.5f, Color.Blue * 0.8f, () =>
                {
                    playerAvatarComps[x].GetNextColor();
                    playerShip_O_ImageComp[x].SetTint(playerAvatarComps[x].GetCurrentColor());
                }));
            }

            NotifyResolutionChanged();
        }

        public override void NotifyResolutionChanged()
        {
            title_LabelComp.SetPosition(new Vector2(Main.Resolution.X / 2, Main.Resolution.Y * 0.13f));
            playerOne_LabelComp.SetPosition(new Vector2(Main.Resolution.X * 0.12f, Main.Resolution.Y * 0.40f));
            playerTwo_LabelComp.SetPosition(new Vector2(Main.Resolution.X * 0.37f, Main.Resolution.Y * 0.40f));
            playerThree_LabelComp.SetPosition(new Vector2(Main.Resolution.X * 0.63f, Main.Resolution.Y * 0.40f));
            playerFour_LabelComp.SetPosition(new Vector2(Main.Resolution.X * 0.88f, Main.Resolution.Y * 0.40f));
            playerFive_LabelComp.SetPosition(new Vector2(Main.Resolution.X * 0.12f, Main.Resolution.Y * 0.80f));
            playerSix_LabelComp.SetPosition(new Vector2(Main.Resolution.X * 0.37f, Main.Resolution.Y * 0.80f));
            playerSeven_LabelComp.SetPosition(new Vector2(Main.Resolution.X * 0.63f, Main.Resolution.Y * 0.80f));
            playerEight_LabelComp.SetPosition(new Vector2(Main.Resolution.X * 0.88f, Main.Resolution.Y * 0.80f));
            confirm_ButtonComp.SetPositionScale(new Vector2(Main.Resolution.X * 0.4f, Main.Resolution.Y * 0.85f), new Vector2(256.0f, 64.0f));

            for (int i = 0; i < PlayerManager.Players.Count; i++)
            {
                playerAvatarComps[i].SetPositionScale(
                    new Vector2(Main.Resolution.X * (0.05f + 0.25f * (i % 4)), Main.Resolution.Y * (0.2f + 0.4f * (float)Math.Floor(i / 4.0f))),
                    new Vector2(75, 90));
                playerAvatar_ButtonComp[i].SetPositionScale(playerAvatarComps[i].Position, playerAvatarComps[i].Scale);

                playerShip_U_ImageComp[i].SetPositionScale(
                    new Vector2(Main.Resolution.X * (0.13f + 0.25f * (i % 4)), Main.Resolution.Y * (0.2f + 0.4f * (float)Math.Floor(i / 4.0f))),
                    new Vector2(75, 90));
                playerShip_O_ImageComp[i].SetPositionScale(playerShip_U_ImageComp[i].Position, playerShip_U_ImageComp[i].Scale);
                playerShip_ButtonComp[i].SetPositionScale(playerShip_U_ImageComp[i].Position, playerShip_U_ImageComp[i].Scale);

                playerColor_ButtonComp[i].SetPositionScale(
                    new Vector2(Main.Resolution.X * (0.09f + 0.25f * (i % 4)), Main.Resolution.Y * (0.345f + 0.4f * (float)Math.Floor(i / 4.0f))),
                    new Vector2(85, 21));
            }
        }


        public void SetGameToSpawn(EGameType gameType)
        {
            gameToSpawn = gameType;
        }
    }
}
