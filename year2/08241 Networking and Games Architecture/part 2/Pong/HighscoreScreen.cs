using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;
using Sce.PlayStation.Core.Imaging;

namespace Pong
{
	public class HighscoreScreen : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		private TextureInfo m_ti, m_ti2;
		private Texture2D m_texture, m_texture2;
		SpriteUV highscoreScreen;
		
		public HighscoreScreen ()
		{
			this.Camera.SetViewFromViewport();
			
			m_texture = new Texture2D("Application/images/highscore.png",false); 
			//m_ti = new TextureInfo(m_texture);
			//SpriteUV titleScreen = new SpriteUV(m_ti);
			//titleScreen.Scale = m_ti.TextureSizef;
			//titleScreen.Pivot = new Vector2(0.5f,0.5f);
			//titleScreen.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width/2, Director.Instance.GL.Context.GetViewport().Height/2);
				
			m_ti = new TextureInfo(m_texture);
			SpriteUV titleScreen = new SpriteUV(m_ti);
			titleScreen.Scale = m_ti.TextureSizef;
			titleScreen.Pivot = new Vector2(0.5f,0.5f);
			titleScreen.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width/2, Director.Instance.GL.Context.GetViewport().Height/2);
			
			this.AddChild(titleScreen);
			
			for (int i = 0; i < 5; i++)
			{
				int screenWidth = Director.Instance.GL.Context.GetViewport().Width;
				int screenHeight = Director.Instance.GL.Context.GetViewport().Height;
				Image image = new Image(ImageMode.Rgba,new ImageSize(screenWidth, screenHeight),new ImageColor(0,0,0,0));
				Font font = new Font(FontAlias.System,50,FontStyle.Regular);
				string label = (i+1) + ". " + Highscore.GetList(i);
				int x = (screenWidth/2) - (font.GetTextWidth(label)/2);
				image.DrawText(label,new ImageColor(255,255,255,255),font,new ImagePosition(x, (200 + (i * 50))));
				image.Decode();
				m_texture2 = new Texture2D(screenWidth, screenHeight, false, PixelFormat.Rgba);
				m_texture2.SetPixels(0, image.ToBuffer());
				m_ti2 = new TextureInfo(m_texture2);
				font.Dispose();
				image.Dispose();
				highscoreScreen = new SpriteUV(m_ti2);
				highscoreScreen.Scale = m_ti2.TextureSizef;
				highscoreScreen.Pivot = new Vector2(0.5f,0.5f);
				highscoreScreen.Position = new Vector2(screenWidth/2, screenHeight/2);
				this.AddChild(highscoreScreen);
			}
			
			//this.AddChild(HighscoreScreen);
			
			//this.AddChild(titleScreen);
			Scheduler.Instance.ScheduleUpdateForTarget(this,0,false);

		}
		
		public override void Update (float dt)
		{
			base.Update (dt);
			int touchCount = Touch.GetData(0).ToArray().Length;
			if(Input2.GamePad0.Cross.Press)
			{
				Director.Instance.ReplaceScene( new TitleScene());
			}
		}
		
		~HighscoreScreen()
		{
			m_texture.Dispose();
			m_ti.Dispose ();
		}
	}
}

