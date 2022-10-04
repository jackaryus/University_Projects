using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.Physics2D;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.UI;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Graphics;

namespace Pong
{
	public class GameScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		private Paddle m_player, m_ai;
		public static Ball m_ball;
		private PongPhysics m_physics;
		private Scoreboard m_scoreboard;
		private SoundPlayer m_pongBlipSoundPlayer;
		private Sound m_pongSound;
		public static int lastScore = 0;
		public static float timepassed = 0;
		int screenWidth = Director.Instance.GL.Context.GetViewport().Width;
		int screenHeight = Director.Instance.GL.Context.GetViewport().Height;
		public static bool gameAI;
		private TextureInfo m_ti;
		private Texture2D m_texture;
		//public static Timer m_timer;
		
		// Change the following value to true if you want bounding boxes to be rendered
		private static Boolean DEBUG_BOUNDINGBOXS = false;
		
		public GameScene (bool ai)
		{
			m_texture = new Texture2D("Application/images/background.png",false);
			m_ti = new TextureInfo(m_texture);
			SpriteUV background = new SpriteUV(m_ti);
			background.Scale = m_ti.TextureSizef;
			background.Pivot = new Vector2(0.5f,0.5f);
			background.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width/2.0f,
			                                  Director.Instance.GL.Context.GetViewport().Height/2.0f);
			this.AddChild(background);
			
			gameAI = ai;
			timepassed = 0;
			//m_time = new Timer();
			Timer.Start();
			this.Camera.SetViewFromViewport();
			m_physics = new PongPhysics();
			
			m_ball = new Ball(m_physics.SceneBodies[(int)PongPhysics.BODIES.Ball]);
			m_player = new Paddle(Paddle.PaddleType.PLAYER, 
			                     m_physics.SceneBodies[(int)PongPhysics.BODIES.Player]);
				m_ai = new Paddle(Paddle.PaddleType.AI, 
			                 m_physics.SceneBodies[(int)PongPhysics.BODIES.Ai]);
			m_scoreboard = new Scoreboard();
			//m_time = new Timer();
			
			this.AddChild(m_scoreboard);
			this.AddChild(m_ball);
			this.AddChild(m_player);
			//if (numOfPlayers == 2)
				//this.AddChild(m_player2);
			//else
				this.AddChild(m_ai);
			//this.AddChild(m_timer);
			
			// This is debug routine that will draw the physics bounding box around the players paddle
			if(DEBUG_BOUNDINGBOXS)
			{
				this.AdHocDraw += () => {
					var bottomLeftPlayer = m_physics.SceneBodies[(int)PongPhysics.BODIES.Player].AabbMin;
					var topRightPlayer = m_physics.SceneBodies[(int)PongPhysics.BODIES.Player].AabbMax;
					Director.Instance.DrawHelpers.DrawBounds2Fill(
						new Bounds2(bottomLeftPlayer*PongPhysics.PtoM,topRightPlayer*PongPhysics.PtoM));

					var bottomLeftAi = m_physics.SceneBodies[(int)PongPhysics.BODIES.Ai].AabbMin;
					var topRightAi = m_physics.SceneBodies[(int)PongPhysics.BODIES.Ai].AabbMax;
					Director.Instance.DrawHelpers.DrawBounds2Fill(
						new Bounds2(bottomLeftAi*PongPhysics.PtoM,topRightAi*PongPhysics.PtoM));

					var bottomLeftBall = m_physics.SceneBodies[(int)PongPhysics.BODIES.Ball].AabbMin;
					var topRightBall = m_physics.SceneBodies[(int)PongPhysics.BODIES.Ball].AabbMax;
					Director.Instance.DrawHelpers.DrawBounds2Fill(
						new Bounds2(bottomLeftBall*PongPhysics.PtoM,topRightBall*PongPhysics.PtoM));
				};
			}			
			
				Image image = new Image(ImageMode.Rgba,new ImageSize(screenWidth, screenHeight),new ImageColor(0,0,0,0));
				Font font = new Font(FontAlias.System,50,FontStyle.Regular);
				string label = timepassed.ToString("0.0");
				//int x = (screenWidth/2) - (font.GetTextWidth(label)/2); (200 + (i * 50))
				image.DrawText(label,new ImageColor(255,255,255,255),font,new ImagePosition(0, 0));
				image.Decode();
				Texture2D timerTexture = new Texture2D(screenWidth, screenHeight, false, PixelFormat.Rgba);
				timerTexture.SetPixels(0, image.ToBuffer());
				TextureInfo m_tiTimer = new TextureInfo(timerTexture);
				SpriteUV timerText = new SpriteUV(m_tiTimer);
				timerText.Scale = m_tiTimer.TextureSizef;
				timerText.Pivot = new Vector2(0.5f,0.5f);
				timerText.Position = new Vector2(screenWidth/2, screenHeight/2);
			
				font.Dispose();
				image.Dispose();
			
			//Now load the sound fx and create a player
			m_pongSound = new Sound("/Application/audio/pongblip.wav");
			m_pongBlipSoundPlayer = m_pongSound.CreatePlayer();
			
			Scheduler.Instance.ScheduleUpdateForTarget(this,0,false);
		}
		
		private void ResetBall()
		{
			//Move ball to screen center and release in a random directory
			m_physics.SceneBodies[(int)PongPhysics.BODIES.Ball].Position = 
				new Vector2(Director.Instance.GL.Context.GetViewport().Width/2.0f,
				            Director.Instance.GL.Context.GetViewport().Height/2.0f) / PongPhysics.PtoM;
			
			System.Random rand = new System.Random();
			float angle = (float)rand.Next(0,360);
		
			if((angle%90) <= 15) angle += 15.0f;
		
			m_physics.SceneBodies[(int)PongPhysics.BODIES.Ball].Velocity = 
				new Vector2(0.0f,5.0f).Rotate(PhysicsUtility.GetRadian(angle));
		}
		
		public override void Update (float dt)
		{
			base.Update (dt);
			
			if(Input2.GamePad0.Select.Press)
				Director.Instance.ReplaceScene(new MenuScene());
			// something!
			//Update the physics simulation
			m_physics.Simulate();
			
			bool ballIsInContact = false;
			//Now check if the ball hit either paddle, and if so, play the sound
			if(m_physics.QueryContact((uint)PongPhysics.BODIES.Ball,(uint)PongPhysics.BODIES.Player) ||
				m_physics.QueryContact((uint)PongPhysics.BODIES.Ball,(uint)PongPhysics.BODIES.Ai))
			{
				ballIsInContact = true;
				// This sound is annoying, so it is commented out!
//				if(m_pongBlipSoundPlayer.Status == SoundStatus.Stopped)
//					m_pongBlipSoundPlayer.Play();
			}
			
			//Check if the ball went off the top or bottom of the screen and update score accordingly
			Results result = Results.StillPlaying;
			bool scored = false;
			
			if(m_ball.Position.Y > Director.Instance.GL.Context.GetViewport().Height + m_ball.Scale.Y/2)
			{
				m_scoreboard.AddScore(true);
				scored = true;
			}
			if(m_ball.Position.Y < 0 - m_ball.Scale.Y/2)
			{
				m_scoreboard.AddScore(false);
				scored = true;
			}
			
			float gameTime = Timer.GetElapsedSeconds();
			timepassed += gameTime;
			this.m_scoreboard.Update(dt);
			// Did someone win?  If so, show the GameOver scene
			if (timepassed >= 60)
			{
				result = m_scoreboard.getWinner();
				if(result == Results.AiWin)
				{
					lastScore = m_scoreboard.m_playerScore;
					Director.Instance.ReplaceScene(new GameOverScene(false));			
				}
				if(result == Results.PlayerWin)
				{
					lastScore = m_scoreboard.m_playerScore;
					Director.Instance.ReplaceScene(new GameOverScene(true));			
				}
			}
			//If someone did score, but game isn't over, reset the ball position to the middle of the screen
			if(scored == true)
			{
				ResetBall ();
			}
			
			//Finally a sanity check to make sure the ball didn't leave the field.
			var ballPB = m_physics.SceneBodies[(int)PongPhysics.BODIES.Ball];
			
			if(ballPB.Position.X < -(m_ball.Scale.X/2f)/PongPhysics.PtoM ||
			   ballPB.Position.X > (Director.Instance.GL.Context.GetViewport().Width)/PongPhysics.PtoM)
			{
				ResetBall();
			}
			else if(!ballIsInContact)
			{
				m_ball.CheckVelocity();
			}
			

			
		}
		
		~GameScene(){
			m_pongBlipSoundPlayer.Dispose();
		}
	}
}

