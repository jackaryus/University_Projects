// File Author: Daniel Masterson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    /// <summary>
    /// An abstract entity component
    /// </summary>
    public abstract class Component : Entity
    {
        public bool IsRegistered { get; private set; }

        public void Register(Entity parent)
        {
            IsRegistered = true;
            SetParent(parent);
            OnRegister();
            DoSpawn();
        }

        public void Unregister()
        {
            IsRegistered = false;
            SetParent(null);
            DoDestroy();
        }

        protected virtual void OnRegister() { }
    }
}
