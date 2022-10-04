using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

using Sce.PlayStation.HighLevel.UI;

namespace Pong
{
	public class MenuScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		private Sce.PlayStation.HighLevel.UI.Scene m_uiScene;
		
		public MenuScene ()
		{
			this.Camera.SetViewFromViewport();
			Sce.PlayStation.HighLevel.UI.Panel dialog = new Panel();
			dialog.Width = Director.Instance.GL.Context.GetViewport().Width;
			dialog.Height = Director.Instance.GL.Context.GetViewport().Height;
			
			ImageBox ib = new ImageBox();
			ib.Width = dialog.Width;
			ib.Image = new ImageAsset("/Application/images/title.png", false);
			ib.Height = dialog.Height;
			ib.SetPosition(0.0f, 0.0f);
			
			Button buttonPlay = new Button();
			buttonPlay.Name = "buttonPlay";
			buttonPlay.Text = "Play Game";
			buttonPlay.Width = 300;
			buttonPlay.Height = 50;
			buttonPlay.Alpha = 0.8f;
			buttonPlay.SetPosition(dialog.Width/2.0f - buttonPlay.Width/2.0f, 220.0f); 
			buttonPlay.TouchEventReceived += OnButtonPlay;
			
			Button button2Play = new Button();
			button2Play.Name = "buttonPlay2";
			button2Play.Text = "2 Player";
			button2Play.Width = 300;
			button2Play.Height = 50;
			button2Play.Alpha = 0.8f;
			button2Play.SetPosition(dialog.Width/2.0f - buttonPlay.Width/2.0f, 450.0f); 
			button2Play.TouchEventReceived += OnButton2Play;
			
			dialog.AddChildLast(ib);
			dialog.AddChildLast(buttonPlay);
			dialog.AddChildLast(button2Play);
			m_uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			m_uiScene.RootWidget.AddChildLast(dialog);
			UISystem.SetScene(m_uiScene);
			Scheduler.Instance.ScheduleUpdateForTarget(this, 0, false);
		}
		
		public void OnButtonPlay(object sender, TouchEventArgs e)
		{
			
			Director.Instance.ReplaceScene(new GameScene(true));
		}
		
		public void OnButton2Play(object sender, TouchEventArgs e)
		{
			
			Director.Instance.ReplaceScene(new GameScene(false));
		}
		
		public override void Update (float dt)
		{
			base.Update (dt);
			UISystem.Update(Touch.GetData(0));
		}
		
		public override void Draw ()
		{
			base.Draw();
			UISystem.Render ();
		}
		
		~MenuScene()
		{
			
		}
	}
}

