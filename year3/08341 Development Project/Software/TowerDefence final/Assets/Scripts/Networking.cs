using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

public class Networking : MonoBehaviour {
    public static string addressip = "192.0.0.1";
    private static bool running;
    public Broadcaster broadcaster;
    public GameObject Spawn;
    static GameObject tower;

	void Start () 
    {
        running = true;
        Listener listner = new Listener();
        broadcaster = new Broadcaster(addressip);
        Thread listenerThread = new Thread(new ThreadStart(listner.Listen));      
        listenerThread.Start();
	}
	

	public static List<string> towersToSpawn = new List<string>();
	public static bool AddEnemy = false;
	void Update()
	{
		while (towersToSpawn.Count > 0) 
		{
			tower = GameObject.Find(towersToSpawn[0]);
			tower.GetComponent<BuildBlock>().MultiplayerHandle();
			towersToSpawn.RemoveAt(0);
		}

		if (AddEnemy) {
			Spawn.GetComponent<Spawn>().SpawnEnemy();
			AddEnemy = false;
		}
	}

    
    public class Broadcaster
    {     
        Socket sendingSocket;
        IPAddress sendAddress;
        IPEndPoint sendEndPoint;

        public Broadcaster(string ip)
        {
            sendingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sendAddress = IPAddress.Parse(ip);
            sendEndPoint = new IPEndPoint(sendAddress, 5000);
        }

        public void SendUDPMessage(string message)
        {
            byte[] send_buffer = Encoding.ASCII.GetBytes(message);
            try
            {
                sendingSocket.SendTo(send_buffer, sendEndPoint);

            }
            catch (Exception send_exception)
            {
                Console.WriteLine(" Exception {0}", send_exception.Message);
            }
        }
    }

    public class Listener
    {
        static int listenPort = 5000;
        public bool Ldone = false;
        public void Listen()
        {
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEnd = new IPEndPoint(IPAddress.Any, listenPort);
            string recievedData;
            byte[] receiveByteArray;
            listener.Client.ReceiveTimeout = 10000;
            while (running)
            {
                try
                {
                    receiveByteArray = listener.Receive(ref groupEnd);
                    recievedData = Encoding.ASCII.GetString(receiveByteArray, 0, receiveByteArray.Length);
					Debug.Log("udp message recieved: " + recievedData);
                    if (recievedData == "1")
                    {
						Networking.AddEnemy = true;
                    }
                    else if (recievedData != null)
                    {
						Networking.towersToSpawn.Add(recievedData);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
                
            }
            listener.Close();
            return;
        }
    }
}
