using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace locationserver
{
    public class Server
    {
        TcpListener listener;
        Socket serverConnection;
        public Dictionary<string, int> HighScoreList;
        public bool save = false;
        //Handle handle = new Handle();
        public Server()
        {
            createServer();
            runServer();
        }

        public void createServer()
        {
            try
            {
                IPAddress ip = IPAddress.Any;
                listener = new TcpListener(ip, 43);
                listener.Start();
                loadDictionary();
            }
            catch (Exception e)
            {
                Console.WriteLine("failed to start server");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        public void runServer()
        {
            while (true)
            {
                try
                {
                    serverConnection = listener.AcceptSocket();
                    try
                    {
                        Console.WriteLine("client connected");
                        //serverConnection.ReceiveTimeout = 1000;
                        //serverConnection.SendTimeout = 1000;
                        Handle handle = new Handle();
                        handle.initialiseHandle(this, serverConnection);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    //finally
                    //{
                    //serverConnection.Close();
                    //}
                }
                catch (Exception)
                {
                    Console.WriteLine("timeout2");

                }
                if (save)
                {
                    saveDictionary();
                }
            }
        }

        public void loadDictionary()
        {
            HighScoreList = new Dictionary<string, int>();
            if (File.Exists("HighScores.txt"))
            {
                string[] lines = File.ReadAllLines("HighScores.txt");
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(new char[] { ' ' });
                    HighScoreList.Add(parts[0], Convert.ToInt32(parts[1]));
                }
            }
            else
            {
                //for (int i = 0; i < 5; i++)
                //{
                    //HighScoreList.Add("Computer" + i, (5 - i));
                    saveDictionary();
                    loadDictionary();
                //}
            }
        }

        public void saveDictionary()
        {
            if (File.Exists("HighScores.txt"))
            {
                StreamWriter writer = new StreamWriter("HighScores.txt");
                for (int i = 0; i < HighScoreList.Count; i++)
                {
                    string name = HighScoreList.ElementAt(i).Key;
                    //string score = ToString( HighScoreList[name];
                    writer.WriteLine(name + " " + HighScoreList[name]);
                }
                writer.Close();
            }
            else
            {
                //File = "HighScores.txt";
                //File.Create("HighScores.txt");
                StreamWriter writer = new StreamWriter("HighScores.txt");
                //for (int i = 0; i < HighScoreList.Count; i++)
                //{
                    for (int i = 0; i < 5; i++)
                    {
                        //string name = HighScoreList.ElementAt(i).Key;
                        //string score = ToString( HighScoreList[name];
                        writer.WriteLine("Computer" + (i + 1) + " " + (5 - i));
                    }
                //}
                writer.Close();
            }
        }
    }
}
