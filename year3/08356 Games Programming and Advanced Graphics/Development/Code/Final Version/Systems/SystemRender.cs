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
    class SystemRender : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_TEXTURE | ComponentTypes.COMPONENT_SHADER);
        const ComponentTypes modelMASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_TEXTURE | ComponentTypes.COMPONENT_SHADER | ComponentTypes.COMPONENT_MODEL);

        RasterizerState rasterizerState;
        public SystemRender()
        {
            rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
        }
        public string Name
        {
            get { return "SystemRender"; }
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

                Matrix world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(position.X, position.Y, position.Z);

                IComponent textureComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TEXTURE;
                });
                Texture2D texture = ((ComponentTexture)textureComponent).Texture;

                IComponent effectComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_SHADER;
                });
                Effect effect = ((ComponentShader)effectComponent).Effect;

                Draw(world, geometry, texture, effect);
            }
            else if ((entity.Mask & modelMASK) == modelMASK)
            {
                List<IComponent> components = entity.Components;
                IComponent modelComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_MODEL;
                });
                OpenGL_Game.Objects.Model model = ((ComponentModel)modelComponent).model();

                IComponent positionComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 position = ((ComponentTransform)positionComponent).Position;
                Vector3 rotation = ((ComponentTransform)positionComponent).Rotation;
                Vector3 scale = ((ComponentTransform)positionComponent).Scale;

                Matrix world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(position.X, position.Y, position.Z);

                IComponent textureComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TEXTURE;
                });
                Texture2D texture = ((ComponentTexture)textureComponent).Texture;

                IComponent effectComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_SHADER;
                });
                Effect effect = ((ComponentShader)effectComponent).Effect;

                modelDraw(world, model, texture, effect);
            }

        }

        public void modelDraw(Matrix world, OpenGL_Game.Objects.Model model, Texture2D texture, Effect effect)
        {
            //BasicEffect basicEffect = new BasicEffect(MyGame.gameInstance.GraphicsDevice);
            //basicEffect.World = world;
            //basicEffect.View = MyGame.gameInstance.view;
            //basicEffect.Projection = MyGame.gameInstance.projection;
            //basicEffect.Texture = texture;
            //basicEffect.TextureEnabled = true;

            //effect.Parameters["UserTexture"].SetValue(texture);
            //effect.Parameters["WorldViewProj"].SetValue(world * MyGame.gameInstance.view * MyGame.gameInstance.projection);

            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(MyGame.gameInstance.view);
            effect.Parameters["Projection"].SetValue(MyGame.gameInstance.projection);
            effect.Parameters["Texture"].SetValue(texture);

            effect.Parameters["innerCone"].SetValue(0.97f);//cones for the spot lights 
            effect.Parameters["outerCone"].SetValue(0.99f);//both share same cone

            effect.Parameters["PointLightDistanceSquared"].SetValue(200f);//point
            effect.Parameters["SpotLightDistanceSquared1"].SetValue(9.0f);//blue 
            effect.Parameters["SpotLightDistanceSquared2"].SetValue(9.0f);//yellow

            effect.Parameters["PointLightPos"].SetValue(new Vector3(MyGame.cameraPosition.X,
                                                                     MyGame.cameraPosition.Y,
                                                                     MyGame.cameraPosition.Z));//point light from player
            effect.Parameters["SpotLightPos1"].SetValue(MyGame.spotLightPos1); //Blue spotlight pos
            effect.Parameters["SpotLightPos2"].SetValue(MyGame.spotLightPos2);//Yellow spotlight pos

            effect.Parameters["PointLightDiffuseColour"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));//rgb
            effect.Parameters["SpotLightDiffuseColour1"].SetValue(new Vector3(0.0f, 0.0f, 1.0f));//blue
            effect.Parameters["SpotLightDiffuseColour2"].SetValue(new Vector3(1.0f, 1.0f, 0.0f));//yellow

            effect.Parameters["PointLightDirection"].SetValue(new Vector3(0.0f, 0.0f, 0.0f));//no direction 
            effect.Parameters["SpotLightDirection1"].SetValue(new Vector3(0.0f, -1.0f, 0.0f));//from the roof down 
            effect.Parameters["SpotLightDirection2"].SetValue(new Vector3(0.0f, -1.0f, 0.0f));


            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            MyGame.gameInstance.GraphicsDevice.RasterizerState = rasterizerState;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                model.modelobj.Draw(world, MyGame.gameInstance.view, MyGame.gameInstance.projection);
            }
            //Console.WriteLine("draw model");
        }

        public void Draw(Matrix world, Geometry geometry, Texture2D texture, Effect effect)
        {

            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(MyGame.gameInstance.view);
            effect.Parameters["Projection"].SetValue(MyGame.gameInstance.projection);
            effect.Parameters["Texture"].SetValue(texture);

            effect.Parameters["innerCone"].SetValue(0.97f);//cones for the spot lights 
            effect.Parameters["outerCone"].SetValue(0.99f);//both share same cone

            effect.Parameters["PointLightDistanceSquared"].SetValue(40.0f);//point
            effect.Parameters["SpotLightDistanceSquared1"].SetValue(9.0f);//blue 
            effect.Parameters["SpotLightDistanceSquared2"].SetValue(9.0f);//yellow

            effect.Parameters["PointLightPos"].SetValue(new Vector3(MyGame.cameraPosition.X,
                                                                     MyGame.cameraPosition.Y,
                                                                     MyGame.cameraPosition.Z));//point light from player
            effect.Parameters["SpotLightPos1"].SetValue(MyGame.spotLightPos1); //Blue spotlight pos
            effect.Parameters["SpotLightPos2"].SetValue(MyGame.spotLightPos2);//Yellow spotlight pos

            effect.Parameters["PointLightDiffuseColour"].SetValue(new Vector3(1.0f, 1.0f, 1.0f));//rgb
            effect.Parameters["SpotLightDiffuseColour1"].SetValue(new Vector3(0.0f, 0.0f, 1.0f));//blue
            effect.Parameters["SpotLightDiffuseColour2"].SetValue(new Vector3(1.0f, 1.0f, 0.0f));//yellow

            effect.Parameters["PointLightDirection"].SetValue(new Vector3(0.0f, 0.0f, 0.0f));//no direction 
            effect.Parameters["SpotLightDirection1"].SetValue(new Vector3(0.0f, -1.0f, 0.0f));//from the roof down 
            effect.Parameters["SpotLightDirection2"].SetValue(new Vector3(0.0f, -1.0f, 0.0f));

            MyGame.gameInstance.GraphicsDevice.SetVertexBuffer(geometry.VertexBuffer);
            MyGame.gameInstance.GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                MyGame.gameInstance.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, geometry.NumberOfTriangles);
            }
        }

    }
}
