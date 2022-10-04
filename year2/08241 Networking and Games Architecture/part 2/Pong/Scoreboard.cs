using System;

using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Pong
{
	public enum Results { PlayerWin, AiWin, StillPlaying };
	
	public class Scoreboard : Sce.PlayStation.HighLevel.GameEngine2D.SpriteUV
	{
		public int m_playerScore = 0;
		public int m_aiScore = 0;
		//public float timepassed = 0;
		int level = 1;
		int screenWidth = Director.Instance.GL.Context.GetViewport().Width;
		int screenHeight = Director.Instance.GL.Context.GetViewport().Height;
		
		public Scoreboard ()
		{
			this.TextureInfo = new TextureInfo();
			UpdateImage();
			
			this.Scale = this.TextureInfo.TextureSizef;
			this.Pivot = new Vector2(0.5f,0.5f);
			this.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width/2,
			                            Director.Instance.GL.Context.GetViewport().Height/2);
			
		}
		
		private void UpdateImage()
		{
			Image image = new Image(ImageMode.Rgba,new ImageSize(screenWidth, screenHeight),new ImageColor(0,0,0,0));
			Font font = new Font(FontAlias.System,50,FontStyle.Regular);
			string scoreLabel = m_playerScore + " - " + m_aiScore;
			string levelLabel = "Level - " + level;
			int x = (screenWidth/2) - (font.GetTextWidth(scoreLabel)/2);
			image.DrawText(scoreLabel, new ImageColor(255,255,255,255), font, new ImagePosition(x,(screenHeight/2)-25));
			x = (screenWidth/2) - (font.GetTextWidth(levelLabel)/2);
			image.DrawText(levelLabel, new ImageColor(255,255,255,255), font, new ImagePosition(x,(screenHeight/2)+25));
			image.DrawText(GameScene.timepassed.ToString("0.0"), new ImageColor(255,255,255,255), font, new ImagePosition(0, 0));
			image.Decode();

			var scoreTexture  = new Texture2D(screenWidth,screenHeight,false,PixelFormat.Rgba);
			if(this.TextureInfo.Texture != null)
				this.TextureInfo.Texture.Dispose();
			this.TextureInfo.Texture = scoreTexture;
			scoreTexture.SetPixels(0,image.ToBuffer());
			
			font.Dispose();
			image.Dispose();
		}
		public void Clear()
		{
			m_playerScore = m_aiScore = 0;
			UpdateImage();
		}
		
		public override void Update (float dt)
		{
			base.Update (dt);
			UpdateImage();
		}
		
		public void AddScore(bool player)
		{
			if(player)
			{
				m_playerScore++;
				if (m_playerScore%3 == 0)
				{
					level++;
					Ball.incrementVelocity(0.5f);
				}
				
			}
			else
				m_aiScore++;
			
			//float gameTime = Timer.GetElapsedSeconds();
			//timepassed += gameTime;

			
			UpdateImage();


		}
		
		public int GetPlayerScore()
		{
			return m_playerScore;	
		}
		
		public Results getWinner()
		{
			if(m_playerScore > m_aiScore) return Results.PlayerWin;
			if(m_aiScore > m_playerScore) return Results.AiWin;
			
			return Results.StillPlaying;
		}
	}
}

