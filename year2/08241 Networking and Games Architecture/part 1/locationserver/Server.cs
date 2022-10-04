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
        public Dictionary<string, string> locationList;
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
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                listener = new TcpListener(ip, 43);
                listener.Start();
                loadDictionary();
            }
            catch
            {
                Console.WriteLine("failed to connect");
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
            locationList = new Dictionary<string, string>();
            if (File.Exists("Dictionary.txt"))
            {
                string[] lines = File.ReadAllLines("Dictionary.txt");
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(new char[] { ' ' });
                    locationList.Add(parts[0], parts[1]);
                }
            }
        }

        public void saveDictionary()
        {
            StreamWriter writer = new StreamWriter("Dictionary.txt");
            for (int i = 0; i<locationList.Count;i++)
            {
                string name = locationList.ElementAt(i).Key;
                string location = locationList[name];
                writer.WriteLine(name + " " + location);
            }
            writer.Close();
        }
    }
}
