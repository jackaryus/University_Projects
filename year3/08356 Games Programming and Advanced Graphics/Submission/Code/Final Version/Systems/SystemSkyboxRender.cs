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
    class SystemSkyboxRender : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_SHADER | ComponentTypes.COMPONENT_SKYBOX);

        public string Name
        {
            get { return "SystemSkyboxRender"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent geometryComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY;
                });
                Geometry geometry = ((ComponentGeometry)geometryComponent).Geometry();

                IComponent positionComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 position = ((ComponentTransform)positionComponent).Position;
                Vector3 rotation = ((ComponentTransform)positionComponent).Rotation;
                Vector3 scale = ((ComponentTransform)positionComponent).Scale;
                
                Matrix world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(position.X,position.Y,position.Z);

                IComponent effectComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_SHADER;
                });
                Effect effect = ((ComponentShader)effectComponent).Effect;

                IComponent skyboxComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_SKYBOX;
                });
                TextureCube skybox = ((ComponentSkyBox)skyboxComponent).Skybox;

                Draw(world, geometry, effect, skybox);
            }
        }

        public void Draw(Matrix world, Geometry geometry, Effect effect, TextureCube skybox)
        {
            RasterizerState currentRS = new RasterizerState();
            RasterizerState skyboxRS = new RasterizerState();

            currentRS = MyGame.gameInstance.GraphicsDevice.RasterizerState;
            skyboxRS.CullMode = CullMode.CullClockwiseFace;
            currentRS = skyboxRS;

            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(MyGame.gameInstance.view);
            effect.Parameters["Projection"].SetValue(MyGame.gameInstance.projection);
            effect.Parameters["SkyBoxTexture"].SetValue(skybox);
            effect.Parameters["CameraPosition"].SetValue(MyGame.cameraPosition);
            effect.CurrentTechnique.Passes[0].Apply();

            MyGame.gameInstance.GraphicsDevice.SetVertexBuffer(geometry.VertexBuffer);

            MyGame.gameInstance.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, geometry.NumberOfTriangles);

            currentRS.CullMode = CullMode.CullCounterClockwiseFace;
            MyGame.gameInstance.GraphicsDevice.RasterizerState = currentRS;
        }

    }
}
