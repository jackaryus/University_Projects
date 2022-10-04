// File Author: Daniel Masterson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.GameManagers
{
    /// <summary>
    /// Manages the scenes
    /// </summary>
    public class SceneManager : AbstractManager
    {
        public static Scene ActiveScene { get; private set; }

        public static void LoadScene(Scene scene)
        {
            if (ActiveScene != null)
            {
                ActiveScene.SceneDestroy();
            }

            ActiveScene = scene;
            ActiveScene.SceneStart();
        }

        public override void OnManagerUpdate(float delta)
        {
            if (ActiveScene != null)
                ActiveScene.SceneUpdate(delta);
        }

        public override void OnManagerRender(float delta)
        {
            if (ActiveScene != null)
                ActiveScene.SceneDraw(delta);
        }

        public override void OnManagerDestroy()
        {
            if (ActiveScene != null)
                ActiveScene.SceneDestroy();
        }
    }
}
