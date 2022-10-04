// File Author: Adam Kadow
using SpaceGame.Entities;
using SpaceGame.GameManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    /// <summary>
    /// The menu for player customization
    /// </summary>
    class CustomiseMenuScene : Scene
    {
        EGameType gameType;
        CustomiseMenu cm;

        public CustomiseMenuScene(EGameType type)
        {
            gameType = type;
        }

        public override void OnSceneStart()
        {
            CameraManager.SetActiveCamera(new Camera());

            cm = new CustomiseMenu();
            cm.SetGameToSpawn(gameType);

            Spawn(new Background());
            Spawn(cm);
        }
    }
}
