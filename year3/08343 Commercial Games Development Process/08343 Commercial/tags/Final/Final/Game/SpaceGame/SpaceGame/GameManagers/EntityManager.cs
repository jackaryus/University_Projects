// File Author: Daniel Masterson
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.GameManagers
{
    /// <summary>
    /// Manages entities
    /// </summary>
    public class EntityManager : AbstractManager
    {
        static List<Entity> spawningEntities = new List<Entity>();
        static List<Entity> liveEntities = new List<Entity>();
        static List<Entity> destroyingEntities = new List<Entity>();

        private static bool needUpdateZ = false;

        public static void Spawn(Entity entity, Vector2? position = null, Vector2? scale = null)
        {
            spawningEntities.Add(entity);
            entity.DoSpawn(position, scale);
            UpdateZ();
        }

        public static void Destroy(Entity entity)
        {
            destroyingEntities.Remove(entity);
            entity.DoDestroy();
        }

        public static void UpdateZ()
        {
            needUpdateZ = true;
        }

        public static void NotifyResolutionChanged()
        {
            for (int i = 0; i < liveEntities.Count; ++i)
                liveEntities[i].NotifyResolutionChanged();

            for (int i = 0; i < spawningEntities.Count; ++i)
                spawningEntities[i].NotifyResolutionChanged();

            for (int i = 0; i < destroyingEntities.Count; ++i)
                destroyingEntities[i].NotifyResolutionChanged();
        }

        public override void OnManagerUpdate(float delta)
        {
            MouseEvent mouseEvent = new MouseEvent();

            for (int i = liveEntities.Count - 1; i >= 0; --i) //Go backwards so input works with z-index
            {
                if (!mouseEvent.Handled)
                    liveEntities[i].HandleMouse(mouseEvent);

                liveEntities[i].Update(delta);
            }

            for (int i = 0; i < spawningEntities.Count; ++i)
                liveEntities.Add(spawningEntities[i]);

            for (int i = 0; i < destroyingEntities.Count; ++i)
                liveEntities.Remove(destroyingEntities[i]);

            if (needUpdateZ)
            {
                liveEntities.Sort((a, b) =>
                {
                    if (a.ZIndex < b.ZIndex)
                        return -1;
                    else if(a.ZIndex > b.ZIndex)
                        return 1;
                    return 0;
                });

                needUpdateZ = false;
            }

            spawningEntities.Clear();
            destroyingEntities.Clear();
        }

        public override void OnManagerDestroy()
        {
            for (int i = 0; i < liveEntities.Count; ++i)
                liveEntities[i].DoDestroy();

            liveEntities.Clear();
        }
    }
}
