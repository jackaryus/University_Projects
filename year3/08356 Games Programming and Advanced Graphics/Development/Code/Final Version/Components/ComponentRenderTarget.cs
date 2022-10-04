using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Components
{
    class ComponentRenderTarget : IComponent
    {
        RenderTarget2D renderTarget;

        public ComponentRenderTarget(RenderTarget2D rt)
        {
            renderTarget = rt;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_RENDERTARGET; }
        }

        public RenderTarget2D RenderTarget
        {
            get {return renderTarget;}
        }
    }
}
