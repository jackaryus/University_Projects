using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace locationserver
{
    public class Handle
    {
        Server server;
        Socket connection;
        Dictionary<string, int> HighScoreList;
        string[] parts;

        public Handle()
        {
        }

        public void initialiseHandle(Server inServer, Socket inConnection)
        {
            try
            {
                server = inServer;
                connection = inConnection;
                HighScoreList = server.HighScoreList;
                Thread clientThread = new Thread(doRequest);
                clientThread.Start();
            }
            catch
            {
                Console.WriteLine("Handle initialise failed");
            }
        }

        public void doRequest()
        {
            NetworkStream socketStream = new NetworkStream(connection);
            socketStream.ReadTimeout = 1000;
            socketStream.WriteTimeout = 1000;
            StreamWriter sw = new StreamWriter(socketStream);
            StreamReader sr = new StreamReader(socketStream);
            List<string> message = new List<string>();

            try
            {           
                // take and send score to and from clients
                while (sr.Peek() != -1)
                {
                    message.Add(sr.ReadLine());
                }
                parts = message[0].Split(new char[] { ' ' }, 2);
                if (parts.Length == 2)
                {
                    Console.WriteLine(message[0]);
                    if (HighScoreList.ContainsKey(parts[0]))
                    {
                        if (Convert.ToInt32(parts[1]) > HighScoreList[parts[0]])
                        {
                            HighScoreList[parts[0]] = Convert.ToInt32(parts[1]);
                            //sort
                            var sortList = HighScoreList.OrderByDescending(o => o.Value);
                            //change back to dictionary
                            HighScoreList = sortList.ToDictionary(pair => pair.Key, pair => pair.Value);
                            server.HighScoreList = HighScoreList;

                            //foreach (var item in HighScoreList)
                            //{
                                //Console.WriteLine(item.Key + " " + item.Value);
                            //}
                        }
                        else 
                        {
                            return;
                            //Console.WriteLine("update else");
                            //foreach (var item in HighScoreList)
                            //{
                                //Console.WriteLine(item.Key + " " + item.Value);
                            //}
                        }
                        
                    }
                    else
                    {
                        //Console.WriteLine("added new to dictionary");
                        //add new highscore to end of dictionary
                        HighScoreList.Add(parts[0], Convert.ToInt32(parts[1]));
                        //change from dictionary to sorted list by value
                        var sortList = HighScoreList.OrderByDescending(o => o.Value);
                        //change back to dictionary
                        HighScoreList = sortList.ToDictionary(pair => pair.Key, pair => pair.Value);
                        //remove highscore 6 to maintain top 5
                        HighScoreList.Remove(HighScoreList.ElementAt(5).Key);
                        server.HighScoreList = HighScoreList;
                        foreach (var item in HighScoreList)
                        {
                            Console.WriteLine(item.Key + " " + item.Value);
                        }
                    }
                    

                }
                if (parts.Length == 1)
                {
                    Console.WriteLine(parts[0]);
                    //writes each score to client
                    if (parts[0] == "request")
                    {
                        
                        foreach (var item in HighScoreList)
                        {
                            //Console.WriteLine(item.Key + " " + item.Value);
                            sw.WriteLine(item.Key + " " + item.Value);
                            //sw.Flush();
                        }
                        sw.Flush();

                    }
                    //new highscore check sending message back to client
                    /*else if (Convert.ToInt32(parts[1]) > HighScoreList.ElementAt(4).Value)
                    {
                        sw.WriteLine("highscore");
                        sw.Flush();
                    }
                    else
                    {
                        sw.WriteLine("!highscore");
                        sw.Flush();
                    }*/
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            server.save = true;

            Console.WriteLine("closing client");
            sr.Close();
            sw.Close();
            socketStream.Close();
            connection.Close();
        }
    }
}
