// File Author: Daniel Masterson
using SpaceGame.Entities;
using SpaceGame.GameManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    /// <summary>
    /// The main menu scene
    /// </summary>
    public class MainMenuScene : Scene
    {
        public override void OnSceneStart()
        {
            PlayerManager.ClearPlayers();
            CameraManager.SetActiveCamera(new Camera());

            Spawn(new Background());
            Spawn(new MainMenu());
        }
    }
}
