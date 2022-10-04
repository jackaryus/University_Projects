using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    class SystemRenderTarget : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_RENDERTARGET);


        public string Name
        {
            get { return "SystemRenderTarget"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent renderTargetComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_RENDERTARGET;
                });
                RenderTarget2D renderTarget = ((ComponentRenderTarget)renderTargetComponent).RenderTarget;

                Draw(renderTarget);
            }
        }

        public void Draw(RenderTarget2D renderTarget)
        {

            MyGame.gameInstance.GraphicsDevice.SetRenderTarget(renderTarget);
            MyGame.gameInstance.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
            MyGame.gameInstance.spriteBatch.Draw(renderTarget, new Rectangle(1, 1, 100, 100), Color.White);
            MyGame.gameInstance.spriteBatch.End();
        }
    }
}
