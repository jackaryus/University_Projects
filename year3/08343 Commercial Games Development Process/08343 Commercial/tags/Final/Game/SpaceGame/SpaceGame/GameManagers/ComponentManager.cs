// File Author: Daniel Masterson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.GameManagers
{
    /// <summary>
    /// Manages components
    /// </summary>
    public class ComponentManager : AbstractManager
    {
        static List<Component> registeringComponents = new List<Component>();
        static List<Component> activeComponents = new List<Component>();
        static List<Component> unregisteringComponents = new List<Component>();

        private static bool needUpdateZ = false;

        public static void RegisterComponent(Component component, Entity owner)
        {
            registeringComponents.Add(component);
            component.Register(owner);
        }

        public static void UnregisterComponent(Component component)
        {
            unregisteringComponents.Add(component);
            component.Unregister();
        }

        public static void UpdateZ()
        {
            needUpdateZ = true;
        }

        public override void OnManagerUpdate(float delta)
        {
            MouseEvent mouseEvent = new MouseEvent();

            for (int i = 0; i < activeComponents.Count; ++i)
            {
                if (!mouseEvent.Handled)
                    activeComponents[i].HandleMouse(mouseEvent);

                activeComponents[i].Update(delta);
            }

            for (int i = 0; i < registeringComponents.Count; ++i)
                activeComponents.Add(registeringComponents[i]);

            for (int i = 0; i < unregisteringComponents.Count; ++i)
                activeComponents.Remove(unregisteringComponents[i]);

            if (needUpdateZ)
            {
                activeComponents.Sort((a, b) =>
                {
                    if (a.ZIndex < b.ZIndex)
                        return -1;
                    else if (a.ZIndex > b.ZIndex)
                        return 1;
                    return 0;
                });

                needUpdateZ = false;
            }

            registeringComponents.Clear();
            unregisteringComponents.Clear();
        }

        public override void OnManagerRender(float delta)
        {
            for (int i = 0; i < activeComponents.Count; ++i)
                activeComponents[i].Render(delta);
        }

        public override void OnManagerDestroy()
        {
            for (int i = 0; i < activeComponents.Count; ++i)
                activeComponents[i].Unregister();

            activeComponents.Clear();
        }
    }
}
