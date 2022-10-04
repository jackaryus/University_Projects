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
        enum protocol { whois, http9, http0, http1 }
        private protocol serverState = protocol.whois;
        string name;
        string location;
        string[] parts;
        string optionalHeaderLines = "";
        Dictionary<string, string> locationList;

        public Handle()
        {
        }

        public void initialiseHandle(Server inServer, Socket inConnection)
        {
            try
            {
                server = inServer;
                connection = inConnection;
                locationList = server.locationList;
                Thread clientThread = new Thread(doRequest);
                clientThread.Start();
            }
            catch
            {
                
            }
        }

        public void doRequest()
        {
            NetworkStream socketStream = new NetworkStream(connection);
            socketStream.ReadTimeout = 1000;
            socketStream.WriteTimeout = 1000;
            StreamWriter sw = new StreamWriter(socketStream);
            StreamReader sr = new StreamReader(socketStream);

            try
            {
                List<string> message = new List<string>();
                while (sr.Peek() != -1)
                {
                    message.Add(sr.ReadLine());
                }

                if (message[0].StartsWith("GET /"))
                {
                    if (message[0].Contains(" HTTP/1.0"))
                    {
                        serverState = protocol.http0;
                    }
                    else if (message[0].Contains(" HTTP/1.1"))
                    {
                        serverState = protocol.http1;
                    }
                    else
                    {
                        serverState = protocol.http9;
                    }
                }
                else if (message[0].StartsWith("POST /"))
                {
                    if (message[0].Contains(" HTTP/1.0"))
                    {
                        serverState = protocol.http0;
                    }
                    if (message[0].Contains(" HTTP/1.1"))
                    {
                        serverState = protocol.http1;
                    }
                }
                else if (message[0].StartsWith("PUT /"))
                {
                    serverState = protocol.http9;
                }
                else
                {
                    serverState = protocol.whois;
                }


                switch (serverState)
                {
                    #region protocol HTTP0.9
                    case protocol.http9:
                        //request
                        if (message[0].StartsWith("GET /"))
                        {
                            name = message[0].Substring(5);
                            if (locationList.ContainsKey(name))
                            {
                                location = locationList[name]; ;
                                sw.WriteLine("HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n\r\n" + location);
                                sw.Flush();
                                Console.WriteLine("HTTP0.9 Request: " + name + " = " + location);
                            }
                            else
                            {
                                sw.WriteLine("HTTP/0.9 404 Not Found\r\nContent-Type: text/plain\r\n");
                                sw.Flush();
                            }
                        }
                        //update
                        if (message[0].StartsWith("PUT /"))
                        {
                            name = message[0].Substring(5);
                            location = message[2];
                            if (locationList.ContainsKey(name))
                            {
                                locationList[name] = location;
                                sw.WriteLine("HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n");
                                sw.Flush();
                                Console.WriteLine("HTTP0.9 Update: " + name + " = " + location);
                            }
                            else
                            {
                                locationList.Add(name, location);
                                sw.WriteLine("HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n");
                                sw.Flush();
                                Console.WriteLine("HTTP0.9 Creation: " + name + " = " + location);
                            }
                        }
                        break;
                    #endregion

                    #region protocol HTTP1.0
                    case protocol.http0:
                        //request
                        if (message[0].StartsWith("GET /"))
                        {
                            name = message[0].Substring(5);
                            if (locationList.ContainsKey(name))
                            {
                                location = locationList[name];
                                sw.WriteLine("HTTP/1.0 200 OK\r\nContent-Type: text/plain\r\n\r\n" + location);
                                sw.Flush();
                                Console.WriteLine("HTTP1.0 Request: " + name + " = " + location);
                            }
                            else
                            {
                                sw.WriteLine("HTTP/1.0 404 Not Found\r\nContent-Type: text/plain\r\n");
                                sw.Flush();
                            }
                        }
                        //update
                        if (message[0].StartsWith("POST /"))
                        {
                            name = message[0].Substring(6);
                            location = message[3];
                            if (locationList.ContainsKey(name))
                            {
                                locationList[name] = location;
                                sw.WriteLine("HTTP/1.0 200 OK\r\nContent-Type: text/plain\r\n");
                                sw.Flush();
                                Console.WriteLine("HTTP1.0 Update: " + name + " = " + location);
                            }
                            else
                            {
                                locationList.Add(name, location);
                                sw.WriteLine("HTTP/1.0 200 OK\r\nContent-Type: text/plain\r\n");
                                sw.Flush();
                                Console.WriteLine("HTTP1.0 Creation: " + name + " = " + location);
                            }
                        }
                        break;
                    #endregion

                    #region protocol HTTP1.1
                    case protocol.http1:
                        //request
                        if (message[0].StartsWith("GET /"))
                        {
                            name = message[0].Substring(5);
                            if (locationList.ContainsKey(name))
                            {
                                location = locationList[name];
                                sw.WriteLine("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n" + optionalHeaderLines + "\r\n" + location);
                                sw.Flush();
                                Console.WriteLine("HTTP1.1 Request: " + name + " = " + location);
                            }
                            else
                            {
                                sw.WriteLine("HTTP/1.1 404 Not Found\r\nContent-Type: text/plain\r\n" + optionalHeaderLines);
                                sw.Flush();
                            }
                        }
                        //update
                        if (message[0].StartsWith("POST /"))
                        {
                            name = message[0].Substring(6);
                            location = message[4];
                            if (locationList.ContainsKey(name))
                            {
                                locationList[name] = location;
                                sw.WriteLine("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n" + optionalHeaderLines);
                                sw.Flush();
                                Console.WriteLine("HTTP1.1 Update: " + name + " = " + location);
                            }
                            else
                            {
                                locationList.Add(name, location);
                                sw.WriteLine("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n" + optionalHeaderLines);
                                sw.Flush();
                                Console.WriteLine("HTTP1.1 Creation: " + name + " = " + location);
                            }
                        }
                        break;
                    #endregion

                    #region protocol Whois
                    default:
                        parts = message[0].Split(new char[] { ' ' }, 2);
                        //request
                        if (parts.Length == 1)
                        {
                            if (locationList.ContainsKey(parts[0]))
                            {
                                sw.WriteLine(locationList[parts[0]]);
                                sw.Flush();
                                Console.WriteLine("Whois Request: " + parts[0] + " = " + locationList[parts[0]]);
                            }
                            else
                            {
                                sw.WriteLine("ERROR: no entries found");
                                sw.Flush();
                            }
                        }
                        //update
                        else if (parts.Length == 2)
                        {
                            if (locationList.ContainsKey(parts[0]))
                            {
                                locationList[parts[0]] = parts[1];
                                sw.WriteLine("OK");
                                sw.Flush();
                                Console.WriteLine("Whois Update: " + parts[0] + " = " + parts[1]);
                            }
                            else
                            {
                                locationList.Add(parts[0], parts[1]);
                                sw.WriteLine("OK");
                                sw.Flush();
                                Console.WriteLine("Whois Creation: " + parts[0] + " = " + parts[0]);
                            }
                        }
                        break;
                    #endregion

                }
            }
            catch
            {
                Console.WriteLine("timeout");
            }

            server.save = true;

            sr.Close();
            sw.Close();
            socketStream.Close();
            connection.Close();
        }
    }
}
