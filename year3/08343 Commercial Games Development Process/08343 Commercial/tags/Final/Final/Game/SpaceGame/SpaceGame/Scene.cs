// File Author: Daniel Masterson
using SpaceGame.Entities;
using SpaceGame.GameManagers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    /// <summary>
    /// A scene. Acts as a fence for groups of entites (Only the active scene's entities are updated and rendered)
    /// </summary>
    public abstract class Scene
    {
        List<Entity> entities = new List<Entity>();
        public Rectangle Bounds { get; protected set; }

        public void SceneStart()
        {
            OnSceneStart();
        }

        public void SceneDestroy()
        {
            OnSceneDestroy();

            for (int i = 0; i < entities.Count; ++i)
                EntityManager.Destroy(entities[i]);

            entities.Clear();
        }

        public void SceneUpdate(float delta)
        {
            OnSceneUpdate(delta);
        }

        public void SceneDraw(float delta)
        {
            OnSceneDraw(delta);
        }

        public virtual void OnSceneStart() { }
        public virtual void OnSceneDestroy() { }
        public virtual void OnSceneUpdate(float delta) { }
        public virtual void OnSceneDraw(float delta) { }

        public Entity Spawn(Entity entity, Vector2? position = null, Vector2? scale = null)
        {
            entities.Add(entity);
            EntityManager.Spawn(entity, position, scale);
            return entity;
        }

        public void Destroy(Entity entity)
        {
            entities.Remove(entity);
            EntityManager.Destroy(entity);
        }
    }
}
