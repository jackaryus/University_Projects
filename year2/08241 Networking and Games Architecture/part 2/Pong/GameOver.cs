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
	public class GameOverScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		private TextureInfo m_ti, m_ti2;
		private Texture2D m_texture, m_texture2;
		private EditableText nameText;
		bool Localwin, highscore = false;
		SpriteUV labelText;
		Button enterButton;
		int screenWidth, screenHeight;
		string name;
		private Sce.PlayStation.HighLevel.UI.Scene m_uiScene;
		
		public GameOverScene (bool win)
		{
			Localwin = win;
			this.Camera.SetViewFromViewport();
			screenWidth = Director.Instance.GL.Context.GetViewport().Width;
			screenHeight = Director.Instance.GL.Context.GetViewport().Height;
			
			Sce.PlayStation.HighLevel.UI.Panel panel = new Panel();
			panel.Width = screenWidth;
			panel.Height = screenHeight;
			
			ImageBox background = new ImageBox();
			background.Width = panel.Width;
			background.Height = panel.Height;
			background.SetPosition(0.0f, 0.0f);
			
			if(win)			
				background.Image = new ImageAsset("/Application/images/winner.png", false);
			else
				background.Image = new ImageAsset("/Application/images/loser.png", false);
			
			panel.AddChildLast(background);
			
			if (win && Highscore.CheckHighscore(GameScene.lastScore) == true)
			{
				highscore = true;
				Image image = new Image(ImageMode.Rgba,new ImageSize(screenWidth, screenHeight),new ImageColor(0,0,0,0));
				Font font = new Font(FontAlias.System,50,FontStyle.Regular);
				string label = "Congratulations New HighScore!";
				int x = (screenWidth/2) - (font.GetTextWidth(label)/2);
				image.DrawText(label,new ImageColor(255,255,255,255),font,new ImagePosition(x,300));
				image.Decode();
				
				m_texture2 = new Texture2D(screenWidth, screenHeight, false, PixelFormat.Rgba);
				m_texture2.SetPixels(0, image.ToBuffer());
				m_ti2 = new TextureInfo(m_texture2);				
				labelText = new SpriteUV(m_ti2);
				labelText.Scale = m_ti2.TextureSizef;
				labelText.Pivot = new Vector2(0.5f,0.5f);
				labelText.Position = new Vector2(screenWidth/2, screenHeight/2);
				
				nameText = new EditableText();
				nameText.SetSize(300, 50);
				nameText.Text = "Enter Name Here";
				x = (screenWidth/2) - (int)(nameText.Width/2);
				nameText.SetPosition(x, 350);

				//image.Decode();				
				
				
				enterButton = new Button();
				enterButton.Name = "enterButton";
				enterButton.SetSize(100, 50);
				enterButton.Text = "Enter";
				enterButton.Alpha = 0.8f;
				x = (screenWidth/2) - (int)(enterButton.Width/2);
				enterButton.SetPosition(x, 400);
				enterButton.TouchEventReceived += OnEnterButton;
				
					
				panel.AddChildLast(nameText);
				this.AddChild(labelText);
				panel.AddChildLast(enterButton);
				font.Dispose();
				image.Dispose();
			}
			else if (win && Highscore.CheckHighscore(GameScene.lastScore) == false)
			{
				highscore = false;
				Image image = new Image(ImageMode.Rgba,new ImageSize(screenWidth, screenHeight),new ImageColor(0,0,0,0));
				Font font = new Font(FontAlias.System,50,FontStyle.Regular);
				string label = "No new Highscore";
				int x = (screenWidth/2) - (font.GetTextWidth(label)/2);
				image.DrawText(label,new ImageColor(255,255,255,255),font,new ImagePosition(x,300));
				image.Decode();
				
				m_texture2 = new Texture2D(screenWidth, screenHeight, false, PixelFormat.Rgba);
				m_texture2.SetPixels(0, image.ToBuffer());
				m_ti2 = new TextureInfo(m_texture2);				
				labelText = new SpriteUV(m_ti2);
				labelText.Scale = m_ti2.TextureSizef;
				labelText.Pivot = new Vector2(0.5f,0.5f);
				labelText.Position = new Vector2(screenWidth/2, screenHeight/2);
				
				enterButton = new Button();
				enterButton.Name = "enterButton";
				enterButton.SetSize(100, 50);
				enterButton.Text = "Enter";
				enterButton.Alpha = 0.8f;
				x = (screenWidth/2) - (int)(enterButton.Width/2);
				enterButton.SetPosition(x, 400);
				enterButton.TouchEventReceived += OnEnterButton;
				
				this.AddChild(labelText);
				panel.AddChildLast(enterButton);
				font.Dispose();
				image.Dispose();
			}
			else
			{				
				enterButton = new Button();
				enterButton.Name = "enterButton";
				enterButton.SetSize(100, 50);
				enterButton.Text = "Enter";
				enterButton.Alpha = 0.8f;
				int x = (screenWidth/2) - (int)(enterButton.Width/2);
				enterButton.SetPosition(x, 400);
				enterButton.TouchEventReceived += OnEnterButton;
				
				panel.AddChildLast(enterButton);
			}
			                                   
			m_uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			m_uiScene.RootWidget.AddChildLast(panel);
			UISystem.SetScene(m_uiScene);
			
			Scheduler.Instance.ScheduleUpdateForTarget(this,0,false);
			Touch.GetData(0).Clear();
		}
		
		
		public void OnEnterButton(object sender, TouchEventArgs e)
		{
			if (Localwin)
			{
				if (Highscore.CheckHighscore(GameScene.lastScore) == true)
				{
					name = nameText.Text;
					if (name == "" || name == "Enter Name Here" || name.Contains(" "))
					nameText.Text = "Enter Name Here";
					else
					{
						Highscore.UpdateHighscores(name, GameScene.lastScore);
						Director.Instance.ReplaceScene( new HighscoreScreen());
					}
				}
				else
				{
					Director.Instance.ReplaceScene( new HighscoreScreen());
				}
			}
			else 
				Director.Instance.ReplaceScene(new HighscoreScreen());
			
			
		}
		
		public override void Update (float dt)
		{
			base.Update (dt);
			UISystem.Update(Touch.GetData(0));
			if(Input2.GamePad0.Cross.Press)
			{
				if (Localwin)
				{
					if (highscore)
					{	
						name = nameText.Text;
						if (name == "" || name == "Enter Name Here" || name.Contains(" "))	
							nameText.Text = "Enter Name Here";
						Highscore.UpdateHighscores(name, GameScene.lastScore);
						Director.Instance.ReplaceScene( new HighscoreScreen());
					}
					else
					{
						Director.Instance.ReplaceScene( new HighscoreScreen());
					}
				}
				else
				{
					Director.Instance.ReplaceScene( new HighscoreScreen());
				}
			}
			
		}
		
		public override void Draw ()
		{
			base.Draw ();
			UISystem.Render();
		}
		
		~GameOverScene()
		{
			m_texture.Dispose();
			m_ti.Dispose ();
			m_texture2.Dispose();
			m_ti2.Dispose ();
		}
	}
}

