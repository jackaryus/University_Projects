using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
public class Whois
{
    static void Main(string[] args)
    {       
        TcpClient client = new TcpClient();
        string hostIP = "whois.net.dcs.hull.ac.uk";
        int hostPort = 43;
        List<string> requestOutput = new List<string>();
        string updateOutput;
        List<string> argsList = args.ToList();
        string optionalHeaderLines = "";

        try
        {
            if (argsList.Contains("-h"))
            {
                hostIP = argsList[argsList.IndexOf("-h") + 1];
                argsList.RemoveAt(argsList.IndexOf("-h") + 1);
                argsList.Remove("-h");

            }
            if (argsList.Contains("-p"))
            {
                hostPort = int.Parse(argsList[argsList.IndexOf("-p") + 1]);
                argsList.RemoveAt(argsList.IndexOf("-p") + 1);
                argsList.Remove("-p");
            }
            client.Connect(hostIP, hostPort);
            StreamWriter sw = new StreamWriter(client.GetStream());
            StreamReader sr = new StreamReader(client.GetStream());
            client.ReceiveTimeout = 1000;
            client.SendTimeout = 1000;

            // if contains -h9 (html0.9)
            // remove -h9 and change request and update style
            #region protocol HTTP0.9
            if (argsList.Contains("-h9"))
            {
                argsList.Remove("-h9");
                //request:
                if (argsList.Count == 1)
                {
                    sw.WriteLine("GET /"+argsList[0]);
                    sw.Flush();
                    //add this to all to pass end of lab4test
                    
                    while (sr.Peek() != -1)
                    {                        
                        requestOutput.Add(sr.ReadLine());
                    }
                    if (requestOutput[0] == "HTTP/0.9 404 Not Found\r\n")
                    {
                        Console.WriteLine(requestOutput[0]);
                    }
                    else if (requestOutput.Contains("<html>"))
                    {
                        string htmlResult = "";
                        for (int i = 0; i < requestOutput.Count; i++)
                        {
                            htmlResult += requestOutput[i] + "\r\n";
                        }
                        Console.WriteLine(argsList[0] + " is " + htmlResult);
                    }
                    else
                    {
                        Console.WriteLine(argsList[0] + " is " + requestOutput[requestOutput.Count - 1]);
                    }
                }
                //update:
                if (argsList.Count == 2)
                {
                    sw.WriteLine("PUT /"+argsList[0]+"\r\n\r\n"+argsList[1]);
                    sw.Flush();
                    updateOutput = sr.ReadToEnd();
                    if (updateOutput == "HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n")
                    {
                        Console.WriteLine(argsList[0] + " location changed to be " + argsList[1]);
                    }
                }
            }
            #endregion

            // else if contains -h0 (html1.0)
            //    remove -h0 and change request and update style
            #region protocol HTTP1.0
            else if (argsList.Contains("-h0"))
            { 
                argsList.Remove("-h0");
                //request:
                if (argsList.Count == 1)
                {
                    sw.WriteLine("GET /"+argsList[0]+" HTTP/1.0\r\n"+optionalHeaderLines);
                    sw.Flush();
                    while (sr.Peek() != -1)
                    {                     
                        requestOutput.Add(sr.ReadLine());
                    }
                    if (requestOutput[0] == "HTTP/1.0 404 Not Found\r\nContent-Type: text/plain\r\n\r\n")
                    {
                        Console.WriteLine(requestOutput[0]);
                    }
                    else if (requestOutput.Contains("<html>"))
                    {
                        int trimStart = requestOutput.IndexOf("<html>") - 1;
                        string htmlResult = "";
                        for (int i = trimStart; i < requestOutput.Count; i++ )
                        {
                            htmlResult += requestOutput[i] + "\r\n";
                        }
                            Console.WriteLine(argsList[0] + " is " + htmlResult);
                    }
                    else
                    {
                        Console.WriteLine(argsList[0] + " is " + requestOutput[requestOutput.Count - 1]);
                    }
                }
            //    update:
                if (argsList.Count == 2)
                {
                    sw.WriteLine("POST /"+argsList[0]+" HTTP/1.0\r\n Content-Length: "+argsList[1].Length+"\r\n"+ optionalHeaderLines +"\r\n"+argsList[1]);                               
                    sw.Flush();
                    updateOutput = sr.ReadToEnd();
                    if (updateOutput == "HTTP/1.0 200 OK\r\nContent-Type: text/plain\r\n\r\n")
                    {
                        Console.WriteLine(argsList[0] + " location changed to be " + argsList[1]);
                    }
                }
            }
            #endregion

            // else if contains -h1 (html1.1)
            //    remove -h1 and change request and update style
            #region protocol HTTP1.1
            else if (argsList.Contains("-h1"))
            {
                argsList.Remove("-h1");
            //    request:
                if (argsList.Count == 1)
                {
                    sw.WriteLine("GET /"+argsList[0]+" HTTP/1.1\r\nHost: "+hostIP+"\r\n"+optionalHeaderLines);                   
                    sw.Flush();
                    while (sr.Peek() != -1)
                    {
                        requestOutput.Add(sr.ReadLine());
                    }
                    if (requestOutput[0] == "HTTP/1.1 404 Not Found\r\nContent-Type: text/plain\r\n\r\n")
                    {
                        Console.WriteLine(requestOutput[0]);
                    }
                    else if (requestOutput.Contains("<html>"))
                    {
                        int trimStart = requestOutput.IndexOf("<html>") - 1;
                        string htmlResult = "";
                        for (int i = trimStart; i < requestOutput.Count; i++)
                        {
                            htmlResult += requestOutput[i] + "\r\n";
                        }
                        Console.WriteLine(argsList[0] + " is " + htmlResult);
                    }
                    else
                    {
                        Console.WriteLine(argsList[0] + " is " + requestOutput[requestOutput.Count-1]);
                    }
                }
            //    update:
                if (argsList.Count == 2)
                {
                    sw.WriteLine("POST /"+argsList[0]+" HTTP/1.1\r\nHost: "+hostIP+"\r\nContent-Length: "+argsList[1].Length+"\r\n"+optionalHeaderLines+"\r\n"+argsList[1]);                  
                    sw.Flush();
                    updateOutput = sr.ReadToEnd();
                    if (updateOutput == "HTTP/1.1 200 OK\r\nContent-Type: text/plain \r\n")
                    {
                        Console.WriteLine(argsList[0] + " location changed to be " + argsList[1]);
                    }
                }
            }
            #endregion

            #region protocol Whois
            else
            {
                switch (argsList.Count)
                {
                    //request
                    case 1:
                        //Thread.Sleep(20001);
                        sw.WriteLine(argsList[0]);
                        sw.Flush();
                        string whoisRequest;
                        whoisRequest = sr.ReadToEnd();
                        if (whoisRequest == "ERROR: no entries found\r\n")
                        {
                            Console.WriteLine(whoisRequest);
                        }
                        else
                        {
                            Console.WriteLine(argsList[0] + " is " + whoisRequest);
                        }
                        break;

                    //update
                    case 2:
                        sw.WriteLine(argsList[0]+" "+argsList[1]);
                        sw.Flush();
                        updateOutput = sr.ReadToEnd();
                        if (updateOutput == "OK\r\n")
                        {
                            Console.WriteLine(argsList[0] + " location changed to be " + argsList[1]);
                        }
                        break;
                    default:
                        Console.WriteLine("no arguments supplied");
                        break;
                }
            }
            #endregion

        }
        catch
        {
            Console.WriteLine("Timeout");
        }
    }
}