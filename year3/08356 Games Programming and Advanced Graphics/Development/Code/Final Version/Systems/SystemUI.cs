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
    class SystemUI : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_RECTANGLE | ComponentTypes.COMPONENT_TEXTURE);//DONT NEED POSITION
        const ComponentTypes dotMASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_RECTANGLE | ComponentTypes.COMPONENT_TEXTURE | ComponentTypes.COMPONENT_UIVELOCITY);

        public string Name
         {
             get { return "SystemUI"; }
         }
         public void OnAction(Entity entity)
         {
            if ((entity.Mask & dotMASK) == dotMASK)
            {
                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector2 position = new Vector2(((ComponentTransform)positionComponent).Position.X, ((ComponentTransform)positionComponent).Position.Y);
                float rotation =((ComponentTransform)positionComponent).Rotation.Y;
                

                IComponent rectangleComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_RECTANGLE;
                });
                Rectangle rectangle = ((ComponentRectangle)rectangleComponent).Rectangle;

                IComponent textureComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TEXTURE;
                });
                Texture2D texture = ((ComponentTexture)textureComponent).Texture;

                Color col;
                if (position.X == 1)
                {
                    rectangle.X = (((int)MyGame.AIpos.X + 20) * 2) + (rectangle.Width / 2) + rectangle.Width;//+20 for camera offset *2 for scaling rec width/height for center of dot
                    rectangle.Y = (((int)MyGame.AIpos.Y + 20) * 2) + (rectangle.Height / 2) + rectangle.Height;
                    rotation = MyGame.AIrot;
                    col = Color.Red;
                }
                else
                {
                    rectangle.X = (((int)MyGame.cameraPosition.X + 20) * 2) + (rectangle.Width / 2) + rectangle.Width;//+20 for camera offset *2 for scaling rec width/height for center of dot
                    rectangle.Y = (((int)MyGame.cameraPosition.Z + 20) * 2) + (rectangle.Height / 2) + rectangle.Height;
                    rotation = MyGame.cameraRotation.X;
                    col = Color.White;
                }

                //Console.WriteLine("Rectangle X: " + (MyGame.cameraPosition.X + 20));
                //Console.WriteLine("Rectangle Y: " + (MyGame.cameraPosition.Z + 20));

                Draw(position, rectangle, texture, rotation, col);
            }
            else if ((entity.Mask & MASK) == MASK)
             {
                List<IComponent> components = entity.Components;
                 
                IComponent positionComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector2 position = new Vector2(((ComponentTransform)positionComponent).Position.X, ((ComponentTransform)positionComponent).Position.Y);
                float rotation = ((ComponentTransform)positionComponent).Rotation.X;
                int t = (int)((ComponentTransform)positionComponent).Scale.X;
                int u = (int)((ComponentTransform)positionComponent).Scale.Y;
                IComponent rectangleComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_RECTANGLE;
                });
                Rectangle rectangle = ((ComponentRectangle)rectangleComponent).Rectangle;

                IComponent textureComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TEXTURE;
                });
                Texture2D texture = ((ComponentTexture)textureComponent).Texture;
                Color col = Color.White;           
                if(position.Y == 1)
                {
                    rectangle.X = (int)MyGame.MmCubePos.X + t;
                    rectangle.Y = (int)MyGame.MmCubePos.Y + u;
                }
 

                Draw(position, rectangle, texture, rotation, col);
            }
         }

        public void Draw(Vector2 pos, Rectangle rec, Texture2D tex, float rot, Color col)
        {
            MyGame.gameInstance.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            //MyGame.gameInstance.spriteBatch.Draw(tex,rec,Color.White);
            Vector2 origin = new Vector2(tex.Width/2, tex.Height/2);
            MyGame.gameInstance.spriteBatch.Draw(tex, rec, null, col, rot, origin, SpriteEffects.None, 1);
            MyGame.gameInstance.spriteBatch.End();

        }
    }
}
