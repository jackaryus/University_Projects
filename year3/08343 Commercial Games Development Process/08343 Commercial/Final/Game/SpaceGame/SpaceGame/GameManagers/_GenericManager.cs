// File Author: Daniel Masterson
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.GameManagers
{
    /// <summary>
    /// Base class for all managers
    /// </summary>
    public abstract class AbstractManager
    {
        public SpriteBatch SpriteBatch { get { return Main.SpriteBatch; } }

        public virtual void OnManagerUpdate(float delta) { }
        public virtual void OnManagerRender(float delta) { }
        public virtual void OnManagerDestroy() { }
    }

    /// <summary>
    /// Stores and updates all specified managers
    /// </summary>
    public static class Managers
    {
        public static CameraManager CameraManager { get; private set; }
        public static ComponentManager ComponentManager { get; private set; }
        public static DataManager DataManager { get; private set; }
        public static EntityManager EntityManager { get; private set; }
        public static GraphicsManager GraphicsManager { get; private set; }
        public static PlayerManager PlayerManager { get; private set; }
        public static InputManager InputManager { get; private set; }
        public static UtilityManager UtilityManager { get; private set; }
        public static SettingsManager SettingsManager { get; private set; }
        public static SceneManager SceneManager { get; private set; }

        private static List<AbstractManager> _managers = new List<AbstractManager>();
        private static bool _initialized = false;

        public static void Initialize()
        {
            if (_initialized)
                return;

            _managers.Add(UtilityManager = new UtilityManager());
            _managers.Add(CameraManager = new CameraManager());
            _managers.Add(ComponentManager = new ComponentManager());
            _managers.Add(DataManager = new DataManager());
            _managers.Add(EntityManager = new EntityManager());
            _managers.Add(GraphicsManager = new GraphicsManager());
            _managers.Add(PlayerManager = new PlayerManager());
            _managers.Add(InputManager = new InputManager());
            _managers.Add(SettingsManager = new SettingsManager());
            _managers.Add(SceneManager = new SceneManager());

            _initialized = true;
        }

        public static void UpdateManagers(float delta)
        {
            for (int i = 0; i < _managers.Count; ++i)
                _managers[i].OnManagerUpdate(delta);
        }

        public static void RenderManagers(float delta)
        {
            for (int i = 0; i < _managers.Count; ++i)
                _managers[i].OnManagerRender(delta);
        }

        public static void ShutdownManagers()
        {
            if (!_initialized)
                return;

            for (int i = 0; i < _managers.Count; ++i)
                _managers[i].OnManagerDestroy();

            _initialized = false;
        }
    }
}
