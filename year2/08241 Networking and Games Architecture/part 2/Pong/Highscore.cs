using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace Pong
{
	static public class Highscore
	{

		static TcpClient client;
        static private string hostIP; //= "127.0.0.1";
        static private int hostPort; //= 43;
		static StreamReader sr;
		static StreamWriter sw;
		static List<string> highscoreList = new List<string>();
		static bool connected = false;
		
		//public static Highscore current;
		
		//public Highscore()
		//{
			//current = this;
		//}
		
		static public void ConnectionInitialise ()
		{
			try
			{
				GetIPandPort("Application/IP.txt");
				client = new TcpClient();
				client.Connect(hostIP, hostPort);
            	sw = new StreamWriter(client.GetStream());
            	sr = new StreamReader(client.GetStream());
            	//client.ReceiveTimeout = 1000;
            	//client.SendTimeout = 1000;
				connected = true;
			}
			catch
			{
				connected = false;
			}
		}
		
		static public void UpdateHighscores (string name, int score)
		{
			ConnectionInitialise();
			if (connected)
			{
				sw.WriteLine(name + " " + score);
				sw.Flush();
				CloseConnections();
				RequestHighscores();
			}
			else
			{
				Console.WriteLine("could not connect to update");	
			}
		}
		
		static public void RequestHighscores ()
		{
			highscoreList.Clear();
			ConnectionInitialise();
			if (connected)
			{
				Console.WriteLine("connected");
				sw.WriteLine("request");
				sw.Flush();
				while (sr.Peek() !=-1)
				{
					highscoreList.Add(sr.ReadLine());
				}
				CloseConnections();
			}
			else
			{
				Console.WriteLine("could not connect");
				for (int i = 0; i < 5; i++)
				{
					highscoreList.Add("Computer" + i +"  "+ (5 - i));
				}
			}
		}
		
		static public bool CheckHighscore (int score)
		{
			string[] parts = highscoreList.Last().Split(new char[] {' '}, 2);
			if (score >= Convert.ToInt32(parts[1]))
			{
				return true;	
			}
			else
			{
				return false;
			}
		}
		
		static public string GetList (int i)
		{
			return highscoreList[i];
		}
		
		static void CloseConnections()
		{
			connected = false;
			sr.Close();
			sw.Close();
			client.Close();
		}
		static public void GetIPandPort(string filepath)
		{
			if (File.Exists(filepath))
			{
				StreamReader reader = new StreamReader(@filepath);
				string fileString = reader.ReadLine();
				string[] parts = fileString.Split(new char[] {':'}, 2);
				hostIP = parts[0];
				hostPort = Convert.ToInt32(parts[1]);
				reader.Close();
				//return fileIP;
			}
			else 
			{
				//File.Create();
				StreamWriter writer = new StreamWriter(@filepath);
				writer.WriteLine("127.0.0.1:43");
				writer.Close();
				GetIPandPort(filepath);
			}
		}
	}
}

